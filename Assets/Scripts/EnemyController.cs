using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed;
    public int damage;
    protected SpriteRenderer sr; 
    public bool canMove = true; 
    public Animator anim;

    public int maxHealth = 3;
    protected int currentHealth; // Changed to protected so Skeleton sees it
    public bool isAlive = true;
    
    public Transform player;

    protected void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        if (!canMove) return;
        LookAtPlayer();
    }

    public virtual void LookAtPlayer()
    {
        if (player == null || sr == null) return;
        if (transform.position.x > player.position.x) sr.flipX = false;
        else sr.flipX = true;
    }

    // --- CRITICAL CHANGE: 'virtual' allows Skeleton to override this ---
    public virtual void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    // This is 'virtual' so Skeleton can add its own Death logic (stopping coroutines)
    protected virtual void Die()
    {
        isAlive = false;
        canMove = false; 

        PlayerStats pStats = FindObjectOfType<PlayerStats>();
        if (pStats != null) pStats.TriggerLifeSiphon();

        if (anim != null) anim.SetTrigger("Dead");

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return null;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}