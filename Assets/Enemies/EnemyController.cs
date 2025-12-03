using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
public float maxSpeed;
public int damage;
public SpriteRenderer sr;
public bool canMove = true; 
public Animator anim;
private EnemyController controller;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        LookAtPlayer();
    }

public Transform player;

public void LookAtPlayer()
{
    if (player == null || sr == null) return;

    // Flip sprite based on player position (inverted)
    if (transform.position.x > player.position.x)
    {
        sr.flipX = false;
    }
    else
    {
        sr.flipX = true;
    }
}

public int maxHealth = 3;
private int currentHealth;
public bool isAlive = true;

public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
protected virtual void Die()
    {
        isAlive = false;
        canMove = false; 

        PlayerStats pStats = FindObjectOfType<PlayerStats>();
        if (pStats != null)
        {
            pStats.TriggerLifeSiphon();
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



