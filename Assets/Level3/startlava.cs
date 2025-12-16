using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartLava : MonoBehaviour
{
    public LavaRise lava;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lava.startRising = true;
        }
    }
}
