using System.Collections;
using UnityEngine;

public class AcidSpitter : EnemyControllerLevelOne
{
    [Header("Spitter Attack Settings")]
    public float detectionRange = 7f;     // Range to start tracking/firing
    public float attackRange = 5f;        // Range to initiate the fire sequence
    public float windupTime = 0.5f;       // Time before the projectile leaves the mouth
    public float fireRate = 2f;           // Delay between attacks

    [Header("Projectile Setup")]
    public GameObject acidProjectilePrefab; // Link your AcidProjectile Prefab here
    public Transform firePoint;            // Empty child object where the acid spawns

    private bool isAttacking = false;
    private float fireTimer;
    
    // PlayerStats reference removed
    
    // --- Override Start ---
    protected override void Start()
    {
        base.Start(); // Initialize health, sr, anim, and find player (from EnemyController)
        canMove = false; // Acid Spitter is stationary
        fireTimer = fireRate;
        
        // PlayerStats assignment removed
    }

    // --- Override Update ---
    void Update()
    {
        // 1. Safety Check: Stop ALL logic if player is null (assumed to mean player is dead/inactive)
        if (player == null) 
        {
            // Clean up attack state
            StopAllCoroutines(); 
            isAttacking = false; 
            return; 
        }

        // Keep facing the player
        LookAtPlayer();

        // 2. Cooldown Timer
        fireTimer -= Time.deltaTime;

        // 3. Attack Check: ONLY proceed if NOT currently attacking AND cooldown is finished
        if (!isAttacking && fireTimer <= 0) 
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                // Start the attack sequence
                StartCoroutine(AttackSequence());
                fireTimer = fireRate; // Reset the fire timer
            }
        }
    }

    // --- ATTACK SEQUENCE (Fire Projectile) ---
    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Windup Animation
        if (anim != null) anim.SetTrigger("Attack");

        // 2. Wait for the windup time before firing
        yield return new WaitForSeconds(windupTime);

        // 3. FINAL SAFETY CHECK: Stop attack if the player died during the windup
        if (player == null) // Now only checks if the player Transform is null
        {
            isAttacking = false;
            yield break; // Stop the coroutine immediately
        }
        
        // 4. Fire the projectile
        FireProjectile();

        // 5. Wait for the animation recovery/cooldown before allowing another attack attempt
        yield return new WaitForSeconds(fireRate - windupTime); 

        isAttacking = false;
    }

    // --- Projectile Firing Logic ---
    private void FireProjectile()
    {
        if (acidProjectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Acid Projectile Prefab or Fire Point not assigned on Acid Spitter!");
            return;
        }
        
        // 1. Calculate direction to the player (from the fire point)
        Vector2 directionToPlayer = (player.position - firePoint.position).normalized;
        
        // 2. Instantiate the prefab at the fire point's location
        GameObject projectileGO = Instantiate(acidProjectilePrefab, firePoint.position, Quaternion.identity);
        
        // 3. Get the projectile script and set its direction
        AcidProjectile projectileScript = projectileGO.GetComponent<AcidProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(directionToPlayer);
        }
    }
    
    // --- Override Die Method ---
    protected override void Die()
    {
        // Stop any running attack coroutines immediately upon death
        StopAllCoroutines();
        base.Die();
    }
}