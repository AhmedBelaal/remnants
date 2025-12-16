using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snapper inherits the core functionality (health, damage, death, etc.) from EnemyController
public class Snapper : EnemyControllerLevelOne
{
    [Header("Snapper Settings")]
    public float detectionRange = 5f;        // Range at which the snapper triggers
    public float attackRange = 1.5f;         // Range for the actual bite/attack
    public float windupTime = 0.5f;          // Time before the attack hitbox starts
    public float attackDuration = 0.3f;      // Duration of the attack hitbox being active

    [Header("Attack Hitbox")]
    public GameObject biteHitboxPrefab;
    public Transform attackPoint;

    private bool isTriggered = false;
    private bool isAttacking = false;

    private PlayerStats playerStats;
    
    // --- START (Overrides virtual Start from base class) ---
    protected override void Start()
    {
        // Must call the base Start to initialize health, animator, and find player
        base.Start();
        canMove = false; // Snapper is stationary
    }

    // --- UPDATE ---
    void Update()
    {
        // Keep facing the player, regardless of movement status
        LookAtPlayer(); 

        if (player != null && !isAttacking)
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
        
        sr.flipX = false;
    }
}
    // --- BODY COLLISION DAMAGE ---
    // Deals damage when the player physically runs into the Snapper's body
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // 'damage' variable is inherited from EnemyController
                playerStats.TakeDamage(damage); 
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Trigger Attack Animation
        if (anim != null) anim.SetTrigger("Attack");

        // 2. Wait for the Windup portion of the animation to finish
        yield return new WaitForSeconds(windupTime);
        if (player == null) 
        {
            isAttacking = false;
            isTriggered = false;
            yield break; // Stop the coroutine immediately
        }
        // 3. Attack Hitbox Active (The actual damage frame)
        if (biteHitboxPrefab != null && attackPoint != null)
        {
            // --- FIX: POSITIONAL OFFSET AND FLIP ---
            
            // Define the offset distance (TWEAK THIS VALUE IN THE INSPECTOR IF POSSIBLE!)
            // This is the approximate distance from the Snapper's center to its mouth.
            float horizontalOffset = 1.5f; 
            
            // Determine the direction multiplier: 
            // If sr.flipX is true (facing left), direction is -1.
            // If sr.flipX is false (facing right), direction is +1.
            float directionMultiplier = sr.flipX ? -1f : 1f; 
            
            // Calculate the actual spawn position relative to the Snapper's center
            // We use attackPoint.position for the Y-level, but anchor the X position to the center.
            Vector3 spawnPosition = attackPoint.position; 
            spawnPosition.x = transform.position.x + (horizontalOffset * directionMultiplier); 
            
            // 4. Instantiate the hitbox at the calculated position
            GameObject hitbox = Instantiate(biteHitboxPrefab, spawnPosition, Quaternion.identity);
            
            // 5. Ensure the hitbox SCALE flips correctly to face the player
            Vector3 scale = hitbox.transform.localScale;
            // The logic below assumes sr.flipX=false means the sprite faces RIGHT (positive scale)
            if (sr.flipX) 
            { 
                // When flipped (facing left), scale must be negative to face left
                scale.x = -Mathf.Abs(scale.x); 
            }
            else
            { 
                // When not flipped (facing right), scale must be positive
                scale.x = Mathf.Abs(scale.x); 
            }
            hitbox.transform.localScale = scale;

            // 6. Destroy the hitbox after its duration
            Destroy(hitbox, attackDuration);
        }

        // 7. Recovery/Cooldown: Wait for the rest of the attack animation to finish
        yield return new WaitForSeconds(0.5f); 

        isAttacking = false;
        isTriggered = false;
    }
    
    // --- DIE OVERRIDE ---
    protected override void Die()
    {
        // Stop the attack to prevent hitbox spawning after death
        StopAllCoroutines();
        // Call the base Die method which handles animation and destruction
        base.Die();
    }
}