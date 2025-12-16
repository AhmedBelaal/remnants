using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public int damage = 2;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        sr.enabled = false;
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.enabled = true;
            col.enabled = true;

            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(damage);
            }
        }
    }
}
