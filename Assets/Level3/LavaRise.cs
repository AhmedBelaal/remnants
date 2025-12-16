using UnityEngine;

public class LavaRise : MonoBehaviour
{
    public float speed = 2f;
    public bool startRising = false;

    public Transform startPoint;
    public Transform stopPoint;

    private LevelManager lm;

    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        ResetLava();
    }

    void Update()
    {
        if (startRising)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);

            if (transform.position.y >= stopPoint.position.y)
            {
                startRising = false;
            }
        }
    }

    public void ResetLava()
    {
        transform.position = startPoint.position;
        startRising = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lm.RespawnPlayer();
        }
    }
}
