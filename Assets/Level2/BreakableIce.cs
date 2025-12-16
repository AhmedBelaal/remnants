using UnityEngine;

public class BreakableIce : MonoBehaviour
{
    public float breakDelay = 0.2f;
    private bool broken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (broken) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            broken = true;
            Invoke(nameof(Break), breakDelay);
        }
    }

    void Break()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 2f);
    }
}



