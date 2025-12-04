using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public int maxHealth = 3;
    public int lives = 3;

    [Header("Abilities")]
    public bool hasLifeSiphon = false; // CHECK THIS to enable healing
    public bool hasFireball = false;   // Level 3 Ability
    
    [Header("Life Siphon Settings")]
    public float siphonCooldown = 3.0f; 
    private float lastSiphonTime = -10f; 

    // --- ACTIVE LIQUID SHIELD SETTINGS ---
    [Header("Liquid Shield (Active)")]
    public bool hasLiquidShield = false;
    public KeyCode shieldKey = KeyCode.Q;
    public float shieldDuration = 5.0f;
    public float shieldCooldown = 8.0f;
    
    public GameObject shieldVisual; 
    private Animator shieldAnim; 
    
    public bool isShieldActive = false;
    private bool isShieldOnCooldown = false;
    private float shieldTimer = 0f;
    private float cooldownTimer = 0f;

    [Header("Visuals")]
    private SpriteRenderer sr;
    private Animator playerAnim; 
    
    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();

        if (shieldVisual != null)
        {
            shieldAnim = shieldVisual.GetComponent<Animator>();
            shieldVisual.SetActive(false); 
        }
    }

    void Update()
    {
        // 1. Immunity Logic
        if(isImmune)
        {
            immunityTime += Time.deltaTime;
            if(immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.color = Color.white; 
            }
        }

        // 2. SHIELD INPUT
        if (hasLiquidShield && !isShieldActive && !isShieldOnCooldown)
        {
            if (Input.GetKeyDown(shieldKey))
            {
                ActivateShield();
            }
        }

        // 3. SHIELD DURATION
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                StartCoroutine(BreakShieldRoutine()); 
            }
        }

        // 4. COOLDOWN
        if (isShieldOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isShieldOnCooldown = false;
                Debug.Log("Liquid Shield Ready!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            StartCoroutine(BreakShieldRoutine());
            return; 
        }

        if(!isImmune)
        {
            health -= damage;

            if (health > 0)
            {
                if (playerAnim != null) playerAnim.SetTrigger("Hurt");
                StartCoroutine(FlashColorEffect(Color.red));
            }

            if(health <= 0) 
            {
                health = 0;
                if(lives > 0)
                {
                    FindObjectOfType<LevelManager>().RespawnPlayer();
                    health = 3;
                    lives--;
                    ResetShield();
                }
                else
                {
                    Debug.Log("Gameover");
                    Destroy(gameObject);
                }
            }

            isImmune = true;
            immunityTime = 0f;
        }
    }

    // --- LIFE SIPHON LOGIC (THIS IS NEW) ---
    public void TriggerLifeSiphon()
    {
        // 1. Check if we have the ability
        if (!hasLifeSiphon) return;

        // 2. Check Cooldown
        if (Time.time < lastSiphonTime + siphonCooldown) return;

        // 3. Check if we actually need health
        if (health >= maxHealth) return;

        // HEAL
        health++;
        lastSiphonTime = Time.time;
        Debug.Log("Life Siphon! Health Restored.");
        Debug.Log("Current Health: " + health);
        
        // Visual Feedback (Green Flash)
        StartCoroutine(FlashColorEffect(Color.green));
    }

    // ... Shield Functions ...

    void ActivateShield()
    {
        isShieldActive = true;
        shieldTimer = shieldDuration; 
        if (shieldVisual != null) { shieldVisual.SetActive(true); if (shieldAnim != null) shieldAnim.Play("LiquidShieldForm_King"); }
    }

    IEnumerator BreakShieldRoutine()
    {
        if (!isShieldActive) yield break;
        isShieldActive = false;
        isShieldOnCooldown = true;
        cooldownTimer = shieldCooldown;
        isImmune = true;
        immunityTime = 0f; 
        StartCoroutine(FlashColorEffect(Color.cyan)); 
        if (shieldAnim != null) { shieldAnim.Play("LiquidShieldPop_King"); yield return new WaitForSeconds(0.4f); }
        if (shieldVisual != null) { shieldVisual.SetActive(false); }
    }

    void ResetShield()
    {
        isShieldActive = false;
        isShieldOnCooldown = false; 
        if(shieldVisual != null) shieldVisual.SetActive(false);
    }

   IEnumerator FlashColorEffect(Color colorToFlash)
    {
        sr.color = colorToFlash; // Turn Red/Green/Cyan
        yield return new WaitForSeconds(0.2f);
        
        if (isImmune) 
        {
            // If we took damage, go to "Ghost Mode" (Transparent)
            sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else 
        {
            // If we just Healed (Life Siphon), go back to "Normal Mode" (White/Opaque)
            sr.color = Color.white; 
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerStats : MonoBehaviour
// {
//     [Header("Stats")]
//     public int health = 3;
//     public int maxHealth = 3;
//     public int lives = 3;

//     [Header("Abilities")]
//     public bool hasLifeSiphon = false;
//     public bool hasFireball = false; 
    
//     [Header("Liquid Shield (Active)")]
//     public bool hasLiquidShield = false;
//     public KeyCode shieldKey = KeyCode.Q;
//     public float shieldDuration = 5.0f;
//     public float shieldCooldown = 8.0f;
    
//     public GameObject shieldVisual; 
//     private Animator shieldAnim; 
    
//     public bool isShieldActive = false;
//     private bool isShieldOnCooldown = false;
//     private float shieldTimer = 0f;
//     private float cooldownTimer = 0f;

//     [Header("Visuals")]
//     private SpriteRenderer sr;
//     private Animator playerAnim; 
    
//     public bool isImmune = false;
//     private float immunityTime = 0f;
//     public float immunityDuration = 1.5f;

//     void Start()
//     {
//         sr = GetComponent<SpriteRenderer>();
//         playerAnim = GetComponent<Animator>();

//         if (shieldVisual != null)
//         {
//             shieldAnim = shieldVisual.GetComponent<Animator>();
//             shieldVisual.SetActive(false); 
//         }
//     }

//     void Update()
//     {
//         // 1. Immunity Logic
//         if(isImmune)
//         {
//             immunityTime += Time.deltaTime;
//             if(immunityTime >= immunityDuration)
//             {
//                 isImmune = false;
//                 sr.color = Color.white; 
//             }
//         }

//         // 2. SHIELD INPUT
//         if (hasLiquidShield && !isShieldActive && !isShieldOnCooldown)
//         {
//             if (Input.GetKeyDown(shieldKey))
//             {
//                 ActivateShield();
//             }
//         }

//         // 3. SHIELD DURATION
//         if (isShieldActive)
//         {
//             shieldTimer -= Time.deltaTime;
//             if (shieldTimer <= 0)
//             {
//                 StartCoroutine(BreakShieldRoutine()); 
//             }
//         }

//         // 4. COOLDOWN
//         if (isShieldOnCooldown)
//         {
//             cooldownTimer -= Time.deltaTime;
//             if (cooldownTimer <= 0)
//             {
//                 isShieldOnCooldown = false;
//                 Debug.Log("Liquid Shield Ready!");
//             }
//         }
//     }

//     public void TakeDamage(int damage)
//     {
//         // --- TANK MECHANIC ---
//         if (isShieldActive)
//         {
//             StartCoroutine(BreakShieldRoutine());
//             return; 
//         }

//         // --- NORMAL DAMAGE ---
//         if(!isImmune)
//         {
//             health -= damage;

//             if (health > 0)
//             {
//                 if (playerAnim != null) playerAnim.SetTrigger("Hurt");
//                 // HIT = FLASH RED
//                 StartCoroutine(FlashColorEffect(Color.red));
//             }

//             if(health <= 0) 
//             {
//                 health = 0;
//                 if(lives > 0)
//                 {
//                     FindObjectOfType<LevelManager>().RespawnPlayer();
//                     health = 3;
//                     lives--;
//                     ResetShield();
//                 }
//                 else
//                 {
//                     Debug.Log("Gameover");
//                     Destroy(gameObject);
//                 }
//             }

//             isImmune = true;
//             immunityTime = 0f;
//         }
//     }

//     void ActivateShield()
//     {
//         isShieldActive = true;
//         shieldTimer = shieldDuration; 
        
//         if (shieldVisual != null)
//         {
//             shieldVisual.SetActive(true);
//             if (shieldAnim != null) shieldAnim.Play("LiquidShieldForm_King");
//         }
//     }

//     IEnumerator BreakShieldRoutine()
//     {
//         if (!isShieldActive) yield break;

//         isShieldActive = false;
//         isShieldOnCooldown = true;
//         cooldownTimer = shieldCooldown;

//         // --- NEW VISUAL FEEDBACK ---
//         // FLASH CYAN (Blue) to indicate shield broke, NOT health lost
//         isImmune = true;
//         immunityTime = 0f; 
//         StartCoroutine(FlashColorEffect(Color.cyan)); 

//         if (shieldAnim != null)
//         {
//             shieldAnim.Play("LiquidShieldPop_King");
//             yield return new WaitForSeconds(0.4f); 
//         }

//         if (shieldVisual != null)
//         {
//             shieldVisual.SetActive(false);
//         }
        
//         Debug.Log("Shield Popped!");
//     }

//     void ResetShield()
//     {
//         isShieldActive = false;
//         isShieldOnCooldown = false; 
//         if(shieldVisual != null) shieldVisual.SetActive(false);
//     }

//     // --- GENERIC FLASH FUNCTION ---
//     // Now we can pass in any color we want!
//     IEnumerator FlashColorEffect(Color colorToFlash)
//     {
//         sr.color = colorToFlash;
//         yield return new WaitForSeconds(0.2f);
//         // After flash, go to Ghost Mode (transparent white)
//         if (isImmune) sr.color = new Color(1f, 1f, 1f, 0.5f);
//     }
// }

// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class PlayerStats : MonoBehaviour
// // {
// //     public int health = 3;
// //     public int lives = 3;

// //     private SpriteRenderer sr;
// //     private Animator anim; // Added Animator reference

// //     public bool isImmune = false;
// //     private float immunityTime = 0f;
// //     public float immunityDuration = 1.5f;

// //     void Start()
// //     {
// //         sr = GetComponent<SpriteRenderer>();
// //         // Try to find the animator on the player if not assigned
// //         anim = GetComponent<Animator>(); 
// //     }

// //     void Update()
// //     {
// //         if(isImmune == true)
// //         {
// //             immunityTime += Time.deltaTime;
            
// //             // Immunity expired
// //             if(immunityTime >= immunityDuration)
// //             {
// //                 isImmune = false;
// //                 sr.color = Color.white; // Reset to normal color and full alpha
// //             }
// //         }
// //     }

// // public void TakeDamage(int damage)
// //     {
// //         if(isImmune == false)
// //         {
// //             health = health - damage;
            
// //             // --- FIX START ---
// //             // Only play Hurt animation if we are NOT dead
// //             if (health > 0)
// //             {
// //                 if (anim != null)
// //                 {
// //                     anim.SetTrigger("Hurt");
// //                 }
                
// //                 // Visual Feedback (Red Flash) - Optional: You might want this even on death
// //                 StartCoroutine(FlashRedEffect());
// //             }
// //             // --- FIX END ---

// //             if(health < 0) health = 0;

// //             if(lives > 0 && health == 0)
// //             {
// //                 FindObjectOfType<LevelManager>().RespawnPlayer(); // This handles the Death animation
// //                 health = 3;
// //                 lives--;
// //             }
// //             else if (lives == 0 && health == 0)
// //             {
// //                 Debug.Log("Gameover");
// //                 Destroy(this.gameObject);
// //             }

// //             // Reset immunity timer
// //             isImmune = true;
// //             immunityTime = 0f;
// //         }
// //     }

// //     // Coroutine to handle the Red -> Transparent transition
// //     IEnumerator FlashRedEffect()
// //     {
// //         // Turn bright red immediately
// //         sr.color = Color.red;
        
// //         // Keep it red for a split second (0.2s is usually a good "impact" feel)
// //         yield return new WaitForSeconds(0.2f);

// //         // If we are still immune, switch to "Ghost Mode" (Semi-transparent)
// //         if (isImmune)
// //         {
// //             sr.color = new Color(1f, 1f, 1f, 0.5f); // 50% transparency
// //         }
// //     }
// // }