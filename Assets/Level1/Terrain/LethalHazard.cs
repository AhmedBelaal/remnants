using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalHazard : MonoBehaviour
{
    // This function runs automatically when another Collider 2D enters this object's trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object that entered is tagged as "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // 2. Get the PlayerStats component from the Player
            PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
             
                playerStats.health = 0; 
                playerStats.TakeDamage(999);
            
        }
    }
}
