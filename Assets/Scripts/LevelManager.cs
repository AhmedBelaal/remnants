using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject CurrentCheckpoint;
    public Animator anim;
    public PlayerControl playerController;
    
    public GameObject deathScreen;   // Your DeathPanel
    public float respawnDelay = 4.0f; 
    public float fadeDuration = 1.5f; 

    void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerControl>();
        
        if (playerController != null)
            anim = playerController.GetComponent<Animator>();

        // Set HUD Memory Fragment based on Level
        if (HUDManager.instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            
            if (sceneName.Contains("Level0") || sceneName.Contains("Prologue")) 
                HUDManager.instance.UpdateMemoryStage(0); 
            else if (sceneName.Contains("Level1")) 
                HUDManager.instance.UpdateMemoryStage(1); 
            else if (sceneName.Contains("Level2")) 
                HUDManager.instance.UpdateMemoryStage(2); 
            else if (sceneName.Contains("Level3")) 
                HUDManager.instance.UpdateMemoryStage(3); 
            else if (sceneName.Contains("Level4") || sceneName.Contains("Boss")) 
                HUDManager.instance.UpdateMemoryStage(4); 
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        // 1. Lock Movement
        if (playerController != null)
        {
            playerController.canMove = false;
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if(rb != null) rb.velocity = Vector2.zero;
        }

        // 2. Play Death Animation
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        // 3. Wait for animation
        yield return new WaitForSeconds(1.0f);

        // 4. FADE IN DEATH SCREEN
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            CanvasGroup canvasGroup = deathScreen.GetComponent<CanvasGroup>();
            
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f; 
                float timer = 0f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = timer / fadeDuration; 
                    yield return null; 
                }
                canvasGroup.alpha = 1f; 
            }
        }

        // 5. Wait remainder of delay
        float remainingTime = respawnDelay - fadeDuration;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 6. TELEPORT PLAYER
        if (playerController != null && CurrentCheckpoint != null)
        {
            playerController.transform.position = CurrentCheckpoint.transform.position;
        }

        // 7. RESET ANIMATION
        if (anim != null)
        {
            anim.Play("Idle_King");
            anim.ResetTrigger("Death");
        }

        // 8. HEAL PLAYER & UPDATE HUD
        if (playerController != null)
        {
            PlayerStats playerStats = playerController.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // --- THIS IS THE FIX ---
                playerStats.health = playerStats.maxHealth; // Restore health to full
                
                if (HUDManager.instance != null)
                {
                    HUDManager.instance.UpdateHealth(playerStats.health, playerStats.maxHealth);
                }
            }
        }

        // 9. HIDE DEATH SCREEN
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // 10. UNLOCK MOVEMENT
        if (playerController != null)
        {
            playerController.canMove = true;
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI; // Helper for UI
// using UnityEngine.SceneManagement;

// public class LevelManager : MonoBehaviour
// {
//     public GameObject CurrentCheckpoint;
//     public Animator anim;
//     public PlayerControl playerController;

    
//     public GameObject deathScreen;   // Your DeathPanel
//     public float respawnDelay = 4.0f; // Total time from death to respawn
//     public float fadeDuration = 1.5f; // How long the fade-in takes

//     void Start()
//     {
//         if (playerController == null)
//             playerController = FindObjectOfType<PlayerControl>();
        
//         if (playerController != null)
//             anim = playerController.GetComponent<Animator>();

//             string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    
//     if (sceneName.Contains("Level0")) 
//         HUDManager.instance.UpdateMemoryStage(0); // Empty
//     else if (sceneName.Contains("Level1")) 
//         HUDManager.instance.UpdateMemoryStage(1); // Quarter
//     else if (sceneName.Contains("Level2")) 
//         HUDManager.instance.UpdateMemoryStage(2); // Half
//     else if (sceneName.Contains("Level3")) 
//         HUDManager.instance.UpdateMemoryStage(3); // 3/4
//     else if (sceneName.Contains("Level4")) 
//         HUDManager.instance.UpdateMemoryStage(4); // Full
//     }

//     public void RespawnPlayer()
//     {
//         StartCoroutine(RespawnCoroutine());
//     }

//     public IEnumerator RespawnCoroutine()
//     {
//         // 1. Lock Movement
//         if (playerController != null)
//         {
//             playerController.canMove = false;
//             playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//         }

//         // 2. Play Death Animation
//         if (anim != null)
//         {
//             anim.SetTrigger("Death");
//         }

//         // 3. Wait for the animation to physically play out (falling down)
//         yield return new WaitForSeconds(1.0f);

//         // 4. START THE FADE IN
//         if (deathScreen != null)
//         {
//             deathScreen.SetActive(true); // Turn it on

//             // Get the CanvasGroup to control transparency
//             CanvasGroup canvasGroup = deathScreen.GetComponent<CanvasGroup>();
            
//             // Safety check: if you forgot to add the component, we just show it instantly
//             if (canvasGroup != null)
//             {
//                 canvasGroup.alpha = 0f; // Start transparent
//                 float timer = 0f;

//                 // Loop to fade in
//                 while (timer < fadeDuration)
//                 {
//                     timer += Time.deltaTime;
//                     canvasGroup.alpha = timer / fadeDuration; // Slowly go from 0 to 1
//                     yield return null; // Wait for next frame
//                 }
//                 canvasGroup.alpha = 1f; // Ensure it's fully visible at the end
//             }
//         }

//         // 5. Wait for the remainder of the respawn delay
//         // We subtract the fadeDuration so the total wait time is consistent
//         float remainingTime = respawnDelay - fadeDuration;
//         if (remainingTime > 0)
//         {
//             yield return new WaitForSeconds(remainingTime);
//         }

//         // 6. RESPAWN
//         if (playerController != null && CurrentCheckpoint != null)
//         {
//             playerController.transform.position = CurrentCheckpoint.transform.position;
//         }
//         PlayerStats playerStats = playerController.GetComponent<PlayerStats>();
//         if (playerStats != null)
//         {
//            if(HUDManager.instance != null) 
//             HUDManager.instance.UpdateHealth(playerStats.health, PlayerStats.maxHealth);

//         }
//         // 7. Reset Animation
//         if (anim != null)
//         {
//             anim.Play("Idle_King");
//         }

//         // 8. Hide Screen
//         if (deathScreen != null)
//         {
//             deathScreen.SetActive(false);
//         }

//         // 9. Unlock Movement
//         if (playerController != null)
//         {
//             playerController.canMove = true;
//         }
//     }
// }