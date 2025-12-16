using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float climSpeed= 3f;
    private Rigidbody2D Rb;
    private bool isClimb= false;
    private float verticalInput;


    // Start is called before the first frame update
    void Start()
    {
        Rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClimb)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        
    }
    void FixedUpdate()
    {
        if (isClimb)
        {
            Rb.gravityScale=0f;
            Rb.velocity = new Vector2(Rb.velocity.x,verticalInput*climSpeed);
        }
        else
        {
            Rb.gravityScale=3f;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimb = true;
        }
    }
     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimb = false;
        }
    }
}
