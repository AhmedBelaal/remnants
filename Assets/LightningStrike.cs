using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 0.8f; // Disappear quickly

    void Start()
    {
        // Destroy itself automatically after 0.8 seconds
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only hurt the player
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }
}