using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenSkeleton : EnemyController
{
    [Header("AI Settings")]
    public float agroRange = 8f;       
    public float attackRange = 1.5f;   
    public float attackCooldown = 2f;  
    public float attackDelay = 0.5f;   

    [Header("Stun Settings")]
    public float hurtStunDuration = 0.5f; 

    private Rigidbody2D rb;
    private bool grounded;
    private float lastAttackTime = -10f; 
    private bool isAttacking = false;
    private bool isHurt = false; 

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
        
    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void Update()
    {
        // --- CRITICAL FIX: STOP EVERYTHING IF DEAD ---
        if (!isAlive || player == null) 
        {
            if(rb != null) rb.velocity = Vector2.zero;
            return;
        }
        
        // Stop logic if busy
        if (isHurt || isAttacking) 
        {
            rb.velocity = Vector2.zero; 
            return; 
        }

        LookAtPlayer();

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer <= attackRange)
        {
            rb.velocity = Vector2.zero; 
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else if (distToPlayer <= agroRange && grounded && canMove)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (groundCheck != null)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    private void MoveTowardsPlayer()
    {
        if (rb == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * maxSpeed, rb.velocity.y);
    }

    // --- CRITICAL FIX: 'override' connects this to the sword script ---
    public override void TakeDamage(int damage)
    {
        if (!isAlive) return;

        // 1. Run the health math from the parent
        base.TakeDamage(damage);

        // 2. Play the Animation
        if (isAlive)
        {
            StartCoroutine(HurtRoutine());
        }
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;
        rb.velocity = Vector2.zero; 
        if (anim != null) anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(hurtStunDuration);
        isHurt = false;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        LookAtPlayer(); // Force face player one last time

        if (anim != null) anim.SetTrigger("Attack");
        
        yield return new WaitForSeconds(attackDelay);

        if (player != null && isAlive) // Check isAlive again before dealing damage!
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= attackRange + 0.5f)
            {
                PlayerStats stats = player.GetComponent<PlayerStats>();
                if (stats != null) stats.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.5f); 
        isAttacking = false;
    }

    public override void LookAtPlayer()
    {
        if (player == null || sr == null) return;
        if (transform.position.x > player.position.x) sr.flipX = true; 
        else sr.flipX = false;
    }

    // --- CRITICAL FIX: STOP ATTACKS ON DEATH ---
    protected override void Die()
    {
        // 1. Set Flags
        canMove = false;
        isAttacking = false;
        isHurt = false;
        
        // 2. Kill Physics
        if (rb != null) 
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false; // Disable physics so he doesn't push the player
        }

        // 3. Kill Logic
        StopAllCoroutines(); // Force stop the Attack Routine if it's running
        
        // 4. Run standard death (Animation + Destroy)
        base.Die();
    }
}