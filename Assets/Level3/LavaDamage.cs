using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LavaDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats p = other.GetComponent<PlayerStats>();
            if (p != null)
            {
                p.TakeDamage(damage);
            }
        }
    }
}
