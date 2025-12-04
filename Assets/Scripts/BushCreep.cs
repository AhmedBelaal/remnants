using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushCreep : EnemyController
{
    private bool isTriggered = false;
    private Rigidbody2D rb;
    private bool grounded;
    
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    
    [Header("Animation")]
    public float appearAnimationDuration = 1f; // Duration of appear animation
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        // Initialize SpriteRenderer if not set
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // Handle sprite flipping
        if (player != null && sr != null)
        {
            LookAtPlayer();
        }
        
        if (isTriggered && canMove && player != null && grounded)
        {
            MoveTowardsPlayer();
        }
    }

    void FixedUpdate()
    {
        if (groundCheck != null)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    private void MoveTowardsPlayer()
    {
        if (rb == null) return;
        
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * maxSpeed, rb.velocity.y);
    
    if (anim != null)
    {
        anim.SetTrigger("Walk");
    }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                anim.SetTrigger("Activate");
                isTriggered = true;
                canMove = false;
                
                // Store player reference if not already set
                if (player == null)
                {
                    player = collision.transform;
                }
                
                // Wait for appear animation to complete
                StartCoroutine(WaitForAppearAnimation());
                
                // Safety timeout - force enable movement after 3 seconds
                StartCoroutine(SafetyTimeout());
            }
        }
    }
    
    private IEnumerator WaitForAppearAnimation()
    {
        // Wait for the appear animation to complete
        yield return new WaitForSeconds(appearAnimationDuration);
        
        canMove = true;
    }
    
    private IEnumerator SafetyTimeout()
    {
        yield return new WaitForSeconds(3f);
        
        // Force enable movement after 3 seconds as safety measure
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
    
    // Override Die method to stop movement and clean up
    protected override void Die()
    {
        // Stop all movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        // Stop any running coroutines
        StopAllCoroutines();
        
        // Call base Die method which handles animation and destruction
        base.Die();
    }
}

