using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for the parent class "EnemyController" so this works on 
        // BushCreeps, Skeletons, and Bosses automatically.
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            // We don't destroy the object here because one swing might hit 2 enemies!
        }
    }
}