using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    public float triggerDistance = 3f;
    public Transform player;
    private bool hasFallen = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;  
    }

    void Update()
    {
        if (hasFallen) return;

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= triggerDistance)
            {
                Fall();
            }
        }
    }

    void Fall()
    {
        hasFallen = true;
        rb.bodyType = RigidbodyType2D.Dynamic;   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats p = other.GetComponent<PlayerStats>();
            if (p != null)
            {
                p.TakeDamage(1);
            }

            Destroy(gameObject, 0.1f);
        }
    }
}
