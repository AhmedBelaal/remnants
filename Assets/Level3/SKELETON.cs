using UnityEngine;

public class SKELETON : EnemyController
{
    public float moveSpeed = 2f;
    public float detectDistance = 6f;
    public float attackDistance = 1.2f;
    public float attackCooldown = 1f;

    private float nextAttackTime;

    void Update()
    {
        if (!isAlive || !canMove || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            anim.SetBool("IS WALKING", false);

            if (Time.time >= nextAttackTime)
            {
                anim.SetTrigger("ATTACK");
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (distance <= detectDistance)
        {
            FollowPlayer();
        }
        else
        {
            anim.SetBool("IS WALKING", false);
        }
    }

    void FollowPlayer()
    {
        anim.SetBool("IS WALKING", true);

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isAlive) return;

        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }
}
