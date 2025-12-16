using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    public float moveSpeed = 8f;
    public int damageAmount = 1;
    public float lifetime = 3f; // Max time before the projectile is destroyed

    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Ensure the projectile is destroyed after a set time to prevent clutter
        Destroy(gameObject, lifetime); 
    }

    void FixedUpdate()
    {
        // Move the projectile using the Rigidbody2D's velocity
        // This relies on the Rigidbody2D being set to Kinematic
        rb.velocity = direction * moveSpeed;
    }

    // This function is called by the AcidSpitter when the projectile is spawned
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        
        // Optional: Rotate the sprite to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player (or any object tagged "Player")
        if (other.CompareTag("Player"))
        {
            // IMPORTANT: If your PlayerStats script is NOT attached directly to the root Player GameObject, 
            // you might need to use other.GetComponentInParent<PlayerStats>()
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }
            
            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
        // Destroy upon hitting environment (like ground or walls)
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}