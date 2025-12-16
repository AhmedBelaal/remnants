using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }

    void DealDamage(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.TakeDamage(damage);
        }
    }
}