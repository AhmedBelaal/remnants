using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyControllerLevelOne : MonoBehaviour
{
public float maxSpeed;
public int damage;
public SpriteRenderer sr;
public bool canMove = true; 
public Animator anim;
private EnemyControllerLevelOne controller;
    // Start is called before the first frame update
   protected virtual void Start() 
{
    currentHealth = maxHealth;
    anim = GetComponent<Animator>();
    if (sr == null)
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // New: Find player here since Snapper relies on it
    if (player == null)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        LookAtPlayer();
    }

public Transform player;

public virtual void LookAtPlayer()
{
    if (player == null || sr == null) return;

    
    if (transform.position.x > player.position.x)
    {
        
        sr.flipX = false;
    }
    else
    {
        // Enemy is LEFT of Player, so it faces RIGHT (sr.flipX = false)
        sr.flipX = true;
    }
}

public int maxHealth = 3;
public int currentHealth;
public bool isAlive = true;

public void TakeDamage(int damage)
{
    // 1. Safety check: Don't take damage if already dead
    if (!isAlive) return; 

    // Apply damage
    currentHealth -= damage;
    
    // Check for death
    if (currentHealth <= 0)
    {
        currentHealth = 0;
        Die(); // Call Die, which handles the death animation
    }
    else // ONLY trigger HURT if the enemy is still alive
    {
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }
    }
}
protected virtual void Die()
    {
        isAlive = false;
        canMove = false; 

        PlayerStats pStats = FindObjectOfType<PlayerStats>();
        if (pStats != null)
        {
            //pStats.TriggerLifeSiphon();
        }

        // Set death animation trigger BEFORE starting coroutine
        if (anim != null)
        {
            anim.SetTrigger("Dead");
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // Wait a frame to ensure trigger is processed
        yield return null;
        
        // Wait for death animation to play (adjust time as needed)
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}



