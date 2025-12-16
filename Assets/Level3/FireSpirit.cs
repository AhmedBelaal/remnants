using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpirit : EnemyController
{
    private bool isTriggered = false;
    private Rigidbody2D rb;
    private bool grounded;
    
    [Header("Detection")]
    public float detectionRange = 5f; // The distance at which the enemy activates

    // Note: Ground check fields are kept, but movement logic assumes flying/free movement
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    
    [Header("Animation")]
    public float appearAnimationDuration = 1f; // Duration of appear animation
    
    // Start is called before the first frame
    protected virtual void Start()
    {
        // Must call base.Start() first to initialize EnemyController variables if necessary
        base.Start(); 

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        
        // Player finding logic (kept from original code)
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        currentHealth = maxHealth;
        
        // OPTIONAL: Ensure the creep is inactive/hidden at start if your game requires it
        // gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        
        // 1. Activation Logic
        if (!isTriggered)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= detectionRange)
            {
                anim.SetTrigger("Activate");
                isTriggered = true;
                canMove = true; // Prevent movement during the appearance animation
                
                // Wait for appear animation to complete
                
            }
        }

        // 2. Sprite Flipping (Must be added for LookAtPlayer to exist)
        if (sr != null && isTriggered)
        {
            LookAtPlayer();
        }
        
        // 3. Movement Logic (The Fix: Removed the 'grounded' check)
        if (isTriggered && canMove) // <-- FIX IS HERE
        {
            MoveTowardsPlayer();
        } 
        else if (rb != null)
        {
            // If triggered but waiting for 'canMove' or simply not moving, stop horizontal velocity
            rb.velocity = new Vector2(0f, rb.velocity.y); 
        }
    }

    void FixedUpdate()
    {
        // Keep grounded check for physics accuracy, even if not used for movement control
        if (groundCheck != null)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    // NEW: Method required by the Update() loop
    private void LookAtPlayer()
    {
        // Determine the direction to the player
        float directionX = player.position.x - transform.position.x;

        // Flip the sprite based on the player's position
        if (directionX > 0) // Player is to the right
        {
            sr.flipX = false;
        }
        else if (directionX < 0) // Player is to the left
        {
            sr.flipX = true;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (rb == null) return;
        
        Vector2 direction = (player.position - transform.position).normalized;
        
        // Ensure maxSpeed is > 0 in the Inspector!
        rb.velocity = new Vector2(direction.x * maxSpeed, rb.velocity.y);
    
        if (anim != null)
        {
            anim.SetTrigger("Walk");
        }
    }
    
    
    
    // ... (OnCollisionEnter2D and Die methods are unchanged)
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Assuming 'damage' is inherited
                playerStats.TakeDamage(damage);
            }
        }
    }
    
    protected override void Die()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        StopAllCoroutines();
        
        // Call base Die method which handles animation and destruction
        base.Die();
    }
}