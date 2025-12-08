using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform hallwaySpawnPoint; // Drag the empty GameObject in the Hall of Whispers

    [Header("Trapdoor Repair")]
    public Trapdoor trapdoorScript; // Drag the 'Trap_Trigger' object from the Hallway

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Repair the trapdoor so the Hallway is safe
            if (trapdoorScript != null)
            {
                trapdoorScript.RepairFloor();
            }

            // 2. Teleport the player
            if (hallwaySpawnPoint != null)
            {
                other.transform.position = hallwaySpawnPoint.position;
                
                // Optional: Snap camera to new location instantly
                CameraFollow cam = FindObjectOfType<CameraFollow>();
                if(cam != null) 
                {
                    cam.transform.position = new Vector3(hallwaySpawnPoint.position.x, hallwaySpawnPoint.position.y, -10f);
                }
            }
        }
    }
}