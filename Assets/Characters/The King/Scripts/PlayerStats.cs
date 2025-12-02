using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public int lives = 3;

    private SpriteRenderer sr;
    private Animator anim; // Added Animator reference

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Try to find the animator on the player if not assigned
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        if(isImmune == true)
        {
            immunityTime += Time.deltaTime;
            
            // Immunity expired
            if(immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.color = Color.white; // Reset to normal color and full alpha
            }
        }
    }

public void TakeDamage(int damage)
    {
        if(isImmune == false)
        {
            health = health - damage;
            
            // --- FIX START ---
            // Only play Hurt animation if we are NOT dead
            if (health > 0)
            {
                if (anim != null)
                {
                    anim.SetTrigger("Hurt");
                }
                
                // Visual Feedback (Red Flash) - Optional: You might want this even on death
                StartCoroutine(FlashRedEffect());
            }
            // --- FIX END ---

            if(health < 0) health = 0;

            if(lives > 0 && health == 0)
            {
                FindObjectOfType<LevelManager>().RespawnPlayer(); // This handles the Death animation
                health = 3;
                lives--;
            }
            else if (lives == 0 && health == 0)
            {
                Debug.Log("Gameover");
                Destroy(this.gameObject);
            }

            // Reset immunity timer
            isImmune = true;
            immunityTime = 0f;
        }
    }

    // Coroutine to handle the Red -> Transparent transition
    IEnumerator FlashRedEffect()
    {
        // Turn bright red immediately
        sr.color = Color.red;
        
        // Keep it red for a split second (0.2s is usually a good "impact" feel)
        yield return new WaitForSeconds(0.2f);

        // If we are still immune, switch to "Ghost Mode" (Semi-transparent)
        if (isImmune)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f); // 50% transparency
        }
    }
}