using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushCreepLevelOne : EnemyControllerLevelOne
{
    private bool isTriggered = false;
    private Rigidbody2D rb;
    private bool grounded;
    
    [Header("Detection")]
    public float detectionRange = 5f; // NEW: The distance at which the enemy activates

    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    
    [Header("Animation")]
    public float appearAnimationDuration = 1f; // Duration of appear animation
    
    // Start is called before the first frame update
    protected override void Start()
    {
        // Initialize components (existing code)
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        // Initialize SpriteRenderer if not set
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        
        // Find player and initialize health (existing code)
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        
        // NEW: Check for player proximity to trigger activation
        if (!isTriggered)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= detectionRange)
            {
                // Activation Logic (Moved from OnTriggerEnter2D)
                anim.SetTrigger("Activate");
                isTriggered = true;
                canMove = false;
                
                // Wait for appear animation to complete
                StartCoroutine(WaitForAppearAnimation());
                
                // Safety timeout - force enable movement after 3 seconds
                StartCoroutine(SafetyTimeout());
            }
        }

        // Handle sprite flipping
        if (sr != null)
        {
            LookAtPlayer();
        }
        
        // Movement only occurs after being triggered, canMove is true, and grounded
        if (isTriggered && canMove && grounded)
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
        
        // Use the maxSpeed variable (inherited from EnemyController)
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * maxSpeed, rb.velocity.y);
    
        if (anim != null)
        {
            anim.SetTrigger("Walk");
        }
    }
    
    // --- OnTriggerEnter2D REMOVED ---
    // The activation is now handled by distance check in Update()

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