using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBreak : MonoBehaviour
{
    public float breakDelay = 0.5f;
    private bool playerOnBridge = false;
    private float timer = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (playerOnBridge)
        {
            timer += Time.deltaTime;

            if (timer >= breakDelay)
            {
                BreakBridge();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBridge = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBridge = false;
            timer = 0f;  
        }
    }

    void BreakBridge()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; 
    }
}

