using UnityEngine;
using UnityEngine.SceneManagement;

public class Fireball : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    public float lifeTime = 3f; // Destroy after 3 seconds if it hits nothing
    private Rigidbody2D rb;

    private Scene currentScene;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destroy automatically after a few seconds to clean up memory
        Destroy(gameObject, lifeTime);
        currentScene = SceneManager.GetActiveScene();
    }

    public void Setup(Vector2 direction)
    {
        // This is called by the PlayerControl script to set the direction
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        // Flip the sprite if shooting left
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(currentScene.name == "Level1")
        {
          // 1. Did we hit an enemy?
        EnemyControllerLevelOne enemy1 = hitInfo.GetComponent<EnemyControllerLevelOne>();
        if (enemy1 != null)
        {
            enemy1.TakeDamage(damage);
            Destroy(gameObject); // Poof!
            return;
        }

        // 2. Did we hit a wall/ground? (Assume "Ground" layer is used)
        // You might need to check layers here, or just destroy on any non-player collision
        if (!hitInfo.CompareTag("Player") && !hitInfo.GetComponent<SwordHitbox>())
        {
            Destroy(gameObject);
        }
        } 
        else
        {
            // 1. Did we hit an enemy?
        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Poof!
            return;
        }

        // 2. Did we hit a wall/ground? (Assume "Ground" layer is used)
        // You might need to check layers here, or just destroy on any non-player collision
        if (!hitInfo.CompareTag("Player") && !hitInfo.GetComponent<SwordHitbox>())
        {
            Destroy(gameObject);
        }
        }
    }
}