using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public KeyCode Spacebar;
    public KeyCode Left;
    public KeyCode Right;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    
    private bool grounded;
    private Animator anim;
    private Rigidbody2D rb; // Added Rigidbody reference for better performance

    // New variable to control input
    public bool canMove = true; 

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Cache the rigidbody
    }

    void Update()
    {
        // If movement is disabled (because we are dead), stop physics and return
        if (!canMove) 
        {
            rb.velocity = Vector2.zero; 
            return;
        }

        if (Input.GetKeyDown(Spacebar) && grounded) 
        {
            Jump();
        }   

        if (Input.GetKey(Left)) 
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
       
            if (GetComponent<SpriteRenderer>()!= null) {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }  
        else if (Input.GetKey(Right)) 
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        
            if (GetComponent<SpriteRenderer>()!= null) {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        // Added this else to stop sliding when no key is pressed
        else 
        {
             rb.velocity = new Vector2(0, rb.velocity.y);
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Height", rb.velocity.y);
        anim.SetBool("Grounded", grounded);
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void FixedUpdate() {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
}