using UnityEngine;


public class DRAGONCAHSE : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 0.4f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Transform target;
    private bool isChasing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isChasing || target == null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            if (anim != null) anim.SetFloat("Speed", 0f);
            return;
        }

        float dx = target.position.x - transform.position.x;

        if (Mathf.Abs(dx) <= stopDistance)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            if (anim != null) anim.SetFloat("Speed", 0f);
            return;
        }

        float dir = Mathf.Sign(dx);
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (sr != null) sr.flipX = dir > 0;
        if (anim != null) anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public void StartChase(Transform player)
    {
        target = player;
        isChasing = true;
    }
}
