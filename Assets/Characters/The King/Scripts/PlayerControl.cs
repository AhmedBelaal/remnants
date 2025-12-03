using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool canMove = true; // Controlled by LevelManager for death
    private int facingDirection = 1; // 1 = Right, -1 = Left

    [Header("Jump Settings")]
    public float jumpHeight = 5f;
    public int extraJumpsValue = 1; // Set to 1 for Double Jump
    private int extraJumps;
    
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;

    [Header("Inputs")]
    public KeyCode Spacebar = KeyCode.Space;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode DashKey = KeyCode.LeftShift; 

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

    // Components
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
    }

    void Update()
    {
        // 1. Lock inputs if dead or currently dashing
        if (!canMove || isDashing) return;

        // --- DOUBLE JUMP LOGIC ---
        if (grounded)
        {
            extraJumps = extraJumpsValue; // Reset jumps when touching ground
        }

        if (Input.GetKeyDown(Spacebar))
        {
            if (grounded)
            {
                Jump();
            }
            else if (extraJumps > 0)
            {
                Jump();
                extraJumps--; // Use up one mid-air jump
                // Optional: You could play a different animation here if you wanted
            }
        }

        // --- DASH INPUT ---
        if (Input.GetKeyDown(DashKey) && canDash)
        {
            StartCoroutine(Dash());
            return;  
        }

        // --- MOVEMENT LOGIC ---
        if (Input.GetKey(Left))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            facingDirection = -1;
            if (sr != null) sr.flipX = true;
        }
        else if (Input.GetKey(Right))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            facingDirection = 1;
            if (sr != null) sr.flipX = false;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // --- ANIMATIONS ---
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Height", rb.velocity.y);
        anim.SetBool("Grounded", grounded);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // 1. Physics Setup: Remove gravity so we fly straight
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; 
        
        // 2. Apply Speed
        rb.velocity = new Vector2(facingDirection * dashSpeed, 0f);

        // 3. Play Animation
        if(anim != null) anim.SetTrigger("Roll");

        // 4. Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // 5. Reset Physics
        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero; // Stop momentum
        isDashing = false;

        // 6. Cooldown Timer
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
}