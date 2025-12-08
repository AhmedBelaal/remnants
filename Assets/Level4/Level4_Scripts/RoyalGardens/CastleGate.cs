using UnityEngine;

public class CastleGate : MonoBehaviour
{
    public Sprite openSprite;       // The "Open Door" image
    public GameObject magicSeal;    // The Magic Seal object on the door
    public Transform hallwayPoint;  // Where the player spawns in the Hall of Whispers

    private bool isOpen = false;
    private SpriteRenderer sr;
    private PolygonCollider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
    }

    public void Unlock()
    {
        if (isOpen) return;
        isOpen = true;

        // 1. Swap Sprite
        if (sr != null && openSprite != null) sr.sprite = openSprite;

        // 2. Destroy the Magic Seal
        if (magicSeal != null) Destroy(magicSeal);

        // 3. Make the Gate a Trigger so the player can walk "into" it
        if (col != null) col.isTrigger = true;
        
        Debug.Log("Gate Unlocked!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only teleport if the gate is open and the player hits it
        if (isOpen && other.CompareTag("Player"))
        {
            if (hallwayPoint != null)
            {
                other.transform.position = hallwayPoint.position;
                
                // Optional: Snap camera instantly to avoid "sliding" effect
                CameraFollow cam = FindObjectOfType<CameraFollow>();
                if(cam != null) cam.transform.position = new Vector3(hallwayPoint.position.x, hallwayPoint.position.y, -10f);
            }
        }
    }
}