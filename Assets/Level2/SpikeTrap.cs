using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError("SpikeTrap: PlayerStats NOT found on Player!");
            return;
        }

        playerStats.TakeDamage(damage);
    }
}
