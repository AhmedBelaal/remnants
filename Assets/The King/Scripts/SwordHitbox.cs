using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 1;
    private Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentScene.name == "Level1")
        {
            // Check for the parent class "EnemyControllerLevelOne" so this works on 
            // Snapper and other Level 1 enemies automatically.
            EnemyControllerLevelOne enemy1 = other.GetComponent<EnemyControllerLevelOne>();

            if (enemy1 != null)
            {
                enemy1.TakeDamage(damage);
                // We don't destroy the object here because one swing might hit 2 enemies!
                return;
            }
        }
        else
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
}