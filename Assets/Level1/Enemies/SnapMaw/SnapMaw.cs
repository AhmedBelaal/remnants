using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snapmaw inherits the core enemy properties from EnemyController
public class SnapMaw : EnemyControllerLevelOne
{
    [Header("Snapmaw Attack Settings")]
    public float detectionRange = 5f;        // Range to enter the attack ready state
    public float attackRange = 1.5f;         // Range to initiate the bite attack
    public float windupTime = 0.5f;          // Time before the attack hitbox spawns
    public float attackDuration = 0.3f;      // Duration the hitbox stays active
    public float hitboxOffset = 1.5f;        // CRITICAL: Tweak this in Inspector for mouth position

    [Header("Attack Hitbox")]
    public GameObject biteHitboxPrefab;
    public Transform attackPoint;           // Transform for the Y position of the mouth

    private bool isTriggered = false;
    private bool isAttacking = false;
    
    // Reference to the PlayerStats component for checking player's life status


    // --- Override Start ---
    protected override void Start()
    {
        base.Start();
        canMove = false; // Snapmaw is assumed to be stationary
        
    }

    // --- Override Update ---
    void Update()
    {
        
        // Keep facing the player
        LookAtPlayer(); 
        
        if (!isAttacking)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // 1. Detection/Trigger Logic
            if (distanceToPlayer <= detectionRange && !isTriggered)
            {
                isTriggered = true;
            }

            // 2. Attack Logic
            if (isTriggered && distanceToPlayer <= attackRange)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    public override void LookAtPlayer()
{
    if (player == null || sr == null) return;

    
    if (transform.position.x > player.position.x)
    {
        
        sr.flipX = true;
    }
    else
    {
        // Enemy is LEFT of Player, so it faces RIGHT (sr.flipX = false)
        sr.flipX = false;
    }
}

    // --- ATTACK SEQUENCE (The Bite) ---
    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        if (anim != null) anim.SetTrigger("Attack");

        yield return new WaitForSeconds(windupTime);

        
        // Hitbox spawning logic (with positional offset fix for sprite flipping)
        if (biteHitboxPrefab != null && attackPoint != null)
        {
            // Determine the direction multiplier for position: 
            // sr.flipX=true (sprite facing left) means direction is -1.
            float directionMultiplier = sr.flipX ? -1f : 1f; 
            
            // Calculate the actual spawn position relative to the Snapmaw's center
            // X-position is calculated using the offset; Y-position comes from the attackPoint Transform.
            Vector3 spawnPosition = attackPoint.position; 
            spawnPosition.x = transform.position.x + (hitboxOffset * directionMultiplier); 
            
            // Instantiate the hitbox at the calculated position
            GameObject hitbox = Instantiate(biteHitboxPrefab, spawnPosition, Quaternion.identity);
            
            // Ensure the hitbox SCALE flips correctly
            Vector3 scale = hitbox.transform.localScale;
            if (sr.flipX) 
            { 
                scale.x = -Mathf.Abs(scale.x); // Flip scale to face left
            }
            else
            { 
                scale.x = Mathf.Abs(scale.x); // Standard scale to face right
            }
            hitbox.transform.localScale = scale;

            // Destroy the hitbox after its duration
            Destroy(hitbox, attackDuration);
        }

        // Recovery/Cooldown
        yield return new WaitForSeconds(0.5f); 

        isAttacking = false;
        isTriggered = false;
    }
    
    // Body collision logic (inherited from EnemyController, or added here if needed)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage); 
            }
        }
    }
    
    protected override void Die()
    {
        StopAllCoroutines();
        base.Die();
    }


}
