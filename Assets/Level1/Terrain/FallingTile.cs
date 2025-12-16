using System.Collections;
using UnityEngine;

public class FallingTile : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Time the tile waits after the boulder hits it before falling.")]
    public float fallDelay = 0.5f;

    private Rigidbody2D rb;
    private bool isTriggered = false; // Flag to ensure it only falls once

    void Start()
    {
        // Get the Rigidbody2D component.
        rb = GetComponent<Rigidbody2D>();

        // Ensure the tile starts in a static, non-falling state.
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
        }
    }

    // This function is called when another solid collider hits the tile's solid collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is a Boulder using its script component.
        BoulderScript boulder = collision.gameObject.GetComponent<BoulderScript>();

        // We only care if the Boulder hits the tile and the tile hasn't already been triggered.
        if (boulder != null && !isTriggered)
        {
            isTriggered = true;
            
            // Start the sequence that waits and then drops the tile.
            StartCoroutine(FallSequence());
        }
    }

    private IEnumerator FallSequence()
    {
        // 1. Wait Phase
        // Tile is triggered, but waits for the specified delay.
        yield return new WaitForSeconds(fallDelay);

        // 2. Fall Phase
        if (rb != null)
        {
            // Change BodyType to Dynamic to enable physics control (like gravity)
            rb.bodyType = RigidbodyType2D.Dynamic;
            
            // Set gravity scale to a value > 0 to make it fall.
            rb.gravityScale = 5f; 
        }

        // NOTE: The tile will now continue to fall indefinitely in the scene.
    }
}