using System.Collections;
using UnityEngine;

public class DRAGON : MonoBehaviour
{
    public float attackRange = 6f;
    public float fireRate = 2f;
    public float windupTime = 0.5f;
public int maxHealth = 10;
private int currentHealth;

    public GameObject fireProjectilePrefab;
    public Transform firePoint;

    private Animator anim;
    private Transform player;
    private bool isAttacking;
    private float timer;

    void Start()
    {
       anim = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player").transform;

    currentHealth = maxHealth;
    timer = fireRate;
    }

    void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAttacking && timer <= 0 && distance <= attackRange)
        {
            Debug.Log("DRAGON ATTACK TRIGGERED");
    StartCoroutine(DragonAttack());
    timer = fireRate;
        }
    }

    IEnumerator DragonAttack()
    {
        isAttacking = true;

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(windupTime);

        SpawnFire();

        isAttacking = false;
    }

    void SpawnFire()
    {
        if (fireProjectilePrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;

        GameObject fire = Instantiate(
            fireProjectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        fire.GetComponent<DragonFireProjectile>().SetDirection(dir);
    }
    public void TakeDamage(int damage)
{
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
        Die();
    }
}
void Die()
{
    anim.SetTrigger("dead");

    this.enabled = false;

    Collider2D col = GetComponent<Collider2D>();
    if (col != null) col.enabled = false;

    Destroy(gameObject, 2f);
}


}
