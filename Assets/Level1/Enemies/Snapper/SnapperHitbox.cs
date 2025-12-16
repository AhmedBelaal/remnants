using UnityEngine;

// This script should be attached to the prefab used for the biteHitboxPrefab
public class SnapperHitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    public int damageToDeal = 1; // Damage value for the Snapper's attack

    // This function is called when another object's collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object that entered is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // 2. Get the PlayerStats component
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // 3. Deal damage
                playerStats.TakeDamage(damageToDeal);
                
                // 4. Destroy the hitbox immediately to prevent multiple damage ticks
                Destroy(gameObject);
            }
        }
    }
}