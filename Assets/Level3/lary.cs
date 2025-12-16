using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// skeletonwarrior inherits the core functionality from enemycontroller
public class lary : EnemyController
{
    [Header("Warrior Movement")]
    public float moveSpeed = 3f;
    public float chaseDistance = 8f;

    [Header("Melee Attack Settings")]
    public float attackRange = 1.5f;
    public float attackInterval = 1.5f;
    public int attackDamage = 10;
    
    [Header("Hitbox Offset (Facing Right)")]
    // The position of the hitbox, relative to the enemy's center, when facing RIGHT (unflipped)
    public Vector2 attackOffset = new Vector2(0.8f, 0f); 
    
    private float nextAttackTime;
    private bool isAttacking = false;

    // --- START (Overrides virtual Start from base class) ---
    protected  void Start()
    {
        // Must call the base Start to initialize health, animator, sprite renderer, and find player
        base.Start(); 
        nextAttackTime = Time.time;
    }

    // --- UPDATE ---
    void Update()
    {
        // Safety check for life status and player
        if (!isAlive || player == null) 
        {
            if (isAttacking) StopAllCoroutines();
            isAttacking = false;
            // CORRECTED: All lowercase "ismoving"
            if (anim != null) anim.SetBool("ismoving", false);
            return;
        }

        // Keep facing the player
        LookAtPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            // 1. Attack Check: If within range AND cooldown is ready
            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                // CORRECTED: All lowercase "ismoving"
                if (anim != null) anim.SetBool("ismoving", false);
                StartCoroutine(MeleeAttackSequence());
                nextAttackTime = Time.time + attackInterval;
            }
            // 2. Chase Check: If not in attack range
            else if (distance > attackRange && !isAttacking)
            {
                ChasePlayer();
            }
            // 3. Idle Check: Within range, but waiting for cooldown
            else
            {
                // CORRECTED: All lowercase "ismoving"
                if (anim != null) anim.SetBool("ismoving", false);
            }
        }
        // 4. Not Chasing: Out of range
        else
        {
            // CORRECTED: All lowercase "ismoving"
            if (anim != null) anim.SetBool("ismoving", false);
        }
    }

    // --- DIRECTIONAL FLIPPING ---
    public  void LookAtPlayer()
    {
        if (player == null || sr == null) return;
        
        // Correctly flips the sprite based on player position.
        if (transform.position.x > player.position.x)
        {
            sr.flipX = true; // Flips sprite to face left
        }
        else
        {
            sr.flipX = false; // Sprite faces right
        }
    }

    // --- MOVEMENT ---
    private void ChasePlayer()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // CORRECTED: All lowercase "ismoving"
        if (anim != null) anim.SetBool("ismoving", true);
    }

    // --- BODY COLLISION DAMAGE ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive && collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // 'damage' variable is inherited from EnemyController for contact damage
                playerStats.TakeDamage(damage); 
            }
        }
    }

    // --- MELEE ATTACK ---
    private IEnumerator MeleeAttackSequence()
    {
        isAttacking = true;
        
        // 1. Trigger Attack Animation
        // FINAL CORRECTION: Set to the generic lowercase "attack"
        if (anim != null) anim.SetTrigger("attack"); 

        // 2. Wait for the Windup portion of the animation to finish (e.g., 0.4 seconds)
        yield return new WaitForSeconds(0.4f);
        
        // Safety check
        if (player == null || !isAlive) 
        {
            isAttacking = false;
            yield break;
        }

        // 3. --- DAMAGE CALCULATION (Directional Hitbox Check) ---
        
        // Get Direction Multiplier: -1 if flipped (left), +1 if not flipped (right)
        float direction = sr.flipX ? -1f : 1f; 
        
        // Calculate the world position of the attack hitbox based on the offset and facing direction
        Vector2 hitboxCenter = (Vector2)transform.position + new Vector2(attackOffset.x * direction, attackOffset.y);

        // Perform the Physics2D OverlapCircle check
        Collider2D hit = Physics2D.OverlapCircle(hitboxCenter, attackRange, LayerMask.GetMask("Player"));
        
        if (hit != null && hit.CompareTag("Player"))
        {
            PlayerStats playerStats = hit.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
            }
        }

        // 4. Recovery: Wait for the rest of the attack animation to finish
        yield return new WaitForSeconds(0.1f); 

        isAttacking = false;
    }
    
    // --- DAMAGE OVERRIDE (Uses the 'hurt' Trigger) ---
    public  void TakeDamage(int damage)
    {
        if (!isAlive) return;
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (anim != null)
        {
            // CORRECTED: All lowercase "hurt"
            anim.SetTrigger("hurt"); 
        }
    }
    
    // --- DIE OVERRIDE (Uses the 'dead' Trigger) ---
    protected override void Die()
    {
        if (!isAlive) return;
        
        isAlive = false;
        StopAllCoroutines();
        
        if (anim != null) 
        {
            // CORRECTED: All lowercase "ismoving" and "dead"
            anim.SetBool("ismoving", false);
            anim.SetTrigger("dead"); 
        }
        
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;
        
        // Destroy the enemy after the death animation time
        Destroy(gameObject, 2f); 
    }
}