using UnityEngine;

public class TheBossTriggered : MonoBehaviour
{
    public KeeperBoss boss; // Drag the Boss object here
    public GameObject invisibleWall; // Optional: A wall that appears behind player

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss != null)
            {
                boss.WakeUp();
            }
            
            if (invisibleWall != null)
            {
                invisibleWall.SetActive(true);
            }

            // Destroy this trigger so it doesn't happen twice
            Destroy(gameObject);
        }
    }
}