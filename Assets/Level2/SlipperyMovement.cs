using UnityEngine;

public class SlipperyMovement : MonoBehaviour
{
    public float normalAcceleration = 20f;
    public float iceAcceleration = 5f;
    public float maxSpeed = 8f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool onIce = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        float acceleration = onIce ? iceAcceleration : normalAcceleration;

        rb.AddForce(new Vector2(moveInput * acceleration, 0));

        // Clamp max speed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            onIce = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            onIce = false;
        }
    }
}
