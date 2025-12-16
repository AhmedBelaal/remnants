using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public int maxHealth = 10;
    public int damage = 2;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            Attack();
        }

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void Attack()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isAttacking", true);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.SetBool("isHurt", true);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;
    }

    // Called by animation event
    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void ResetHurt()
    {
        animator.SetBool("isHurt", false);
    }
}

