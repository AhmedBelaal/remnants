using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushKnight : EnemyController
{
    [Header("Knight Settings")]
    public float agroRange = 10f;
    public float attackRange = 1.2f;
    public float attackCooldown = 2.5f;
    public float attackDelay = 0.4f;
    
    [Header("Stun Settings")]
    public float hurtStunDuration = 0.3f;

    private Rigidbody2D rb;
    private bool grounded;
    private float lastAttackTime = -10f; 
    private bool isAttacking = false;
    private bool isHurt = false; 

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

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
        // 1. Safety Checks
        if (!isAlive || player == null) 
        {
            if(rb != null) rb.velocity = Vector2.zero;
            return;
        }
        
        // 2. Stop logic if busy
        if (isHurt || isAttacking) 
        {
            rb.velocity = Vector2.zero;
            return; 
        }

        LookAtPlayer();

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        // 3. State Machine
        if (distToPlayer <= attackRange)
        {
            // ATTACK STATE
            rb.velocity = Vector2.zero; 

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else if (distToPlayer <= agroRange && grounded && canMove)
        {
            // CHASE STATE
            MoveTowardsPlayer(); 
        }
        else
        {
            // IDLE STATE (Stands still, but plays 'Run' anim because it's default)
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

    // --- OVERRIDES ---

    public override void TakeDamage(int damage)
    {
        if (!isAlive) return;
        base.TakeDamage(damage);

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

        LookAtPlayer(); 

        if (anim != null) anim.SetTrigger("Attack");
        
        yield return new WaitForSeconds(attackDelay);

        if (player != null && isAlive)
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

        // ADJUST IF NEEDED: Assuming Knight faces RIGHT by default
        if (transform.position.x > player.position.x) sr.flipX = true; 
        else sr.flipX = false; 
    }

    protected override void Die()
    {
        canMove = false;
        isAttacking = false;
        isHurt = false;
        
        if (rb != null) 
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false; 
        }

        StopAllCoroutines();
        base.Die();
    }
}