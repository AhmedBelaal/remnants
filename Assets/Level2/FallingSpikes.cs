using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public float fallDelay = 0.2f;
    public int damage = 1;

    private Rigidbody2D rb;
    private bool activated = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            Invoke(nameof(Fall), fallDelay);
        }
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }
}

