using System.Collections;
using UnityEngine;

public class KeeperBoss : EnemyController
{
    [Header("Boss Config")]
    public BossUI bossUI; 
    public float phase2Threshold = 0.5f; 
    public GameObject lightningPrefab;   // You will create this in Step 3
    public Transform[] lightningSpots;   // Empty objects in the room

    [Header("Combat Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float lightningCooldown = 3f;

    private bool isSleeping = true; // Boss starts frozen
    private bool isPhase2 = false;
    private bool isActing = false;  // True if currently attacking/casting
    private float lastAttackTime = -10f;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        
        if (bossUI != null) bossUI.Hide();
    }

    // --- CALLED BY THE TRIGGER ---
    public void WakeUp()
    {
        if (!isSleeping) return;
        
        isSleeping = false;
        
        if (bossUI != null) bossUI.ActivateBossUI(maxHealth);
        
        // Play the Roar/Anger animation
        if (anim != null) anim.SetTrigger("WakeUp");
        
        // Give him 2 seconds to finish roaring before he chases
        StartCoroutine(WakeUpDelay());
    }

    IEnumerator WakeUpDelay()
    {
        isActing = true;
        yield return new WaitForSeconds(2.0f); // Adjust to match your Anger animation length
        isActing = false;
    }

    void Update()
    {
        // 1. STOP if sleeping, dead, or busy acting
        if (isSleeping || !isAlive || player == null || isActing)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            if (anim != null) anim.SetBool("IsRunning", false);
            return;
        }

        LookAtPlayer();
        float dist = Vector2.Distance(transform.position, player.position);

        // --- PHASE 2 (LIGHTNING) ---
        if (isPhase2)
        {
            if (Time.time > lastAttackTime + lightningCooldown)
            {
                StartCoroutine(CastLightning());
            }
            else
            {
                // In Phase 2, he can still chase/melee while waiting for lightning cooldown
                HandleChaseAndMelee(dist);
            }
        }
        // --- PHASE 1 (MELEE ONLY) ---
        else
        {
            HandleChaseAndMelee(dist);
        }
    }

    void HandleChaseAndMelee(float dist)
    {
        if (dist <= attackRange)
        {
            // Stop and Attack
            rb.velocity = Vector2.zero;
            if (anim != null) anim.SetBool("IsRunning", false);

            if (Time.time > lastAttackTime + attackCooldown)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        else
        {
            // Run towards player
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
            if (anim != null) anim.SetBool("IsRunning", true);
        }
    }

    // --- COMBAT ACTIONS ---

    IEnumerator MeleeAttack()
    {
        isActing = true;
        lastAttackTime = Time.time;
        if (anim != null) anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); // Wait for sword swing frame

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange + 1f)
        {
            player.GetComponent<PlayerStats>().TakeDamage(damage);
        }

        yield return new WaitForSeconds(0.5f); // Recovery
        isActing = false;
    }

    IEnumerator CastLightning()
    {
        isActing = true;
        lastAttackTime = Time.time;
        
        // Stop moving
        if (anim != null) 
        {
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("Cast");
        }

        yield return new WaitForSeconds(0.5f); // Wait for staff raise

        // Spawn Lightning (Logic for Step 3)
        if (lightningPrefab != null && lightningSpots.Length > 0)
        {
            // Spawn 3 bolts
            for(int i=0; i<3; i++)
            {
                Transform spot = lightningSpots[Random.Range(0, lightningSpots.Length)];
                Instantiate(lightningPrefab, spot.position, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(1.0f); // Recovery
        isActing = false;
    }

    // --- HEALTH & DEATH ---

    public override void TakeDamage(int damage)
    {
        if (isSleeping || !isAlive) return;

        base.TakeDamage(damage);
        if (bossUI != null) bossUI.UpdateHealth(currentHealth);

        // Check Phase 2
        if (!isPhase2 && (float)currentHealth / maxHealth <= phase2Threshold)
        {
            isPhase2 = true;
            Debug.Log("PHASE 2 STARTED");
            // Optional: Play anger anim again?
        }
        else if (!isActing)
        {
            if (anim != null) anim.SetTrigger("Hurt");
        }
    }

    protected override void Die()
    {
        isAlive = false;
        StopAllCoroutines();
        if (rb != null) rb.velocity = Vector2.zero;
        
        if (anim != null)
        {
        anim.SetTrigger("Dead");
        anim.Play("Death_Keeper");
        }
        
        if (bossUI != null) bossUI.Hide();

        // Show the Choice UI
        FindObjectOfType<EndingManager>().ShowEndingChoice();
    }

     public override void LookAtPlayer()
    {
        if (player == null || sr == null) return;
        if (transform.position.x > player.position.x) sr.flipX = true;
        else sr.flipX = false;
    }
}