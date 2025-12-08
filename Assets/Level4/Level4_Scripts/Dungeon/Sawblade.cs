using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1; // 1 Damage per hit
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            
            if (stats != null)
            {
                stats.TakeDamage(damage);
                // Optional: Push player back? (Knockback logic would go here)
            }
        }
    }
}
