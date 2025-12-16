using UnityEngine;

public class MagicPearEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int health = 3;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            anim.SetFloat("Speed", 0);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void MoveTowardsPlayer()
    {
        if (isAttacking) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Flip sprite
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        anim.SetTrigger("Attack");

    }

   

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", true);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }
}

