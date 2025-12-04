using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    private int facingDirection = 1; 

    [Header("Jump Settings")]
    public float jumpHeight = 5f;
    public int extraJumpsValue = 1;
    private int extraJumps;
    
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;

[Header("Combat - Melee")]
    public KeyCode AttackKey = KeyCode.E; 
    public GameObject swordHitboxPrefab; 
    public Transform attackPoint;        
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.4f;
    private bool isAttacking = false;

    // --- NEW COMBAT - RANGED ---
    [Header("Combat - Ranged")]
    public KeyCode FireballKey = KeyCode.F;
    public GameObject fireballPrefab;    // Drag your Fireball Prefab here
    public Transform firePoint;          // Where the fireball spawns
    public float fireballCooldown = 2.0f; // Increased slightly since cast is long
    public float castDelay = 1.6f;        // TIME TO WAIT BEFORE SPAWNING (Set to 1.6)
    private float lastFireTime = -10f;

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

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private PlayerStats stats;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
        
        // Find stats script
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!canMove || isDashing) return;

       // --- FIREBALL INPUT ---
        if (Input.GetKeyDown(FireballKey))
        {
            // Check Unlock & Cooldown & Not Attacking
            // We add 'castDelay' to the cooldown check so you can't spam F while casting
            if (stats != null && stats.hasFireball && Time.time >= lastFireTime + fireballCooldown && !isAttacking)
            {
                StartCoroutine(CastFireballRoutine());
            }
        }

        // --- MELEE ATTACK INPUT ---
        if (Input.GetKeyDown(AttackKey) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }

        // --- JUMP LOGIC ---
        if (grounded) extraJumps = extraJumpsValue;

        if (Input.GetKeyDown(Spacebar))
        {
            if (grounded) Jump();
            else if (extraJumps > 0) { Jump(); extraJumps--; }
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
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; 
        rb.velocity = new Vector2(facingDirection * dashSpeed, 0f);
        if(anim != null) anim.SetTrigger("Roll");
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero; 
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // --- RANDOM ATTACK LOGIC ---
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 1. Pick a random attack (0, 1, or 2)
        int randomAttack = Random.Range(0, 3);

        // 2. Tell Animator which one to play
        if(anim != null) 
        {
            anim.SetInteger("AttackIndex", randomAttack);
            anim.SetTrigger("Attack");
        }

        // 3. Spawn Hitbox
        if (swordHitboxPrefab != null && attackPoint != null)
        {
            GameObject hitbox = Instantiate(swordHitboxPrefab, attackPoint.position, Quaternion.identity, transform);
            // It will auto-rotate with the player because 'transform' is the parent
            Destroy(hitbox, attackDuration);
        }

        // 4. Cooldown
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // --- FIREBALL ROUTINE (UPDATED) ---
    private IEnumerator CastFireballRoutine()
    {
        isAttacking = true; // Locks movement/input
        lastFireTime = Time.time;
        
        // Stop the player from sliding while casting (optional, feels better)
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 1. Play Animation
        if(anim != null) anim.SetTrigger("CastSpell");

        // 2. WAIT FOR ANIMATION (The fix!)
        yield return new WaitForSeconds(castDelay);

        // 3. Spawn Projectile
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            
            // Set direction
            Vector2 dir = new Vector2(facingDirection, 0); 
            ball.GetComponent<Fireball>().Setup(dir);
        }

        // 4. Brief recovery after spawn (optional, prevents instant running)
        yield return new WaitForSeconds(0.2f);
        
        isAttacking = false;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
}