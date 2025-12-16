using UnityEngine;

// Boulder inherits core enemy properties (like maxSpeed, player reference) from EnemyController
public class BoulderScript : EnemyControllerLevelOne
{
    private bool isTriggered = false;
    private Rigidbody2D rb;

    // --- Override Start ---
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        canMove = false; // Start in the inactive state
        
    }

    // FixedUpdate is used for physics-based movement
    void FixedUpdate()
    {
        if (isTriggered && canMove && player != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (rb == null) return;
        
        // Calculate a 2D direction vector towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        
        // Move horizontally toward the player at the inherited 'maxSpeed'
        // Uses current Y velocity (gravity)
        rb.velocity = new Vector2(direction.x * maxSpeed, rb.velocity.y);
    }
    
    // --- Activation Logic (Fires when the player enters the trigger collider) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Only run if the boulder hasn't been triggered yet
            if (!isTriggered)
            {
                isTriggered = true;
                canMove = true;
                
                // Set the player reference from the trigger event
                player = collision.transform;
            }
        }
    }
    
    // --- Damage on Collision (Bumping the player) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Damage is hardcoded to 1 to prevent instant kill
                playerStats.TakeDamage(damage); 
            }
            
        }
        else if (collision.gameObject.CompareTag("BoulderKiller"))
        {
            // If the tag matches, destroy this GameObject (the one this script is on).
            Destroy(gameObject);
    }
    }
    
    // Override Die method
    protected override void Die()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        base.Die();
    }
    
}
