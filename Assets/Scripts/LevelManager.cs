using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Helper for UI

public class LevelManager : MonoBehaviour
{
    public GameObject CurrentCheckpoint;
    public Animator anim;
    public PlayerControl playerController;

    
    public GameObject deathScreen;   // Your DeathPanel
    public float respawnDelay = 4.0f; // Total time from death to respawn
    public float fadeDuration = 1.5f; // How long the fade-in takes

    void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerControl>();
        
        if (playerController != null)
            anim = playerController.GetComponent<Animator>();
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
            playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        // 2. Play Death Animation
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        // 3. Wait for the animation to physically play out (falling down)
        yield return new WaitForSeconds(1.0f);

        // 4. START THE FADE IN
        if (deathScreen != null)
        {
            deathScreen.SetActive(true); // Turn it on

            // Get the CanvasGroup to control transparency
            CanvasGroup canvasGroup = deathScreen.GetComponent<CanvasGroup>();
            
            // Safety check: if you forgot to add the component, we just show it instantly
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f; // Start transparent
                float timer = 0f;

                // Loop to fade in
                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = timer / fadeDuration; // Slowly go from 0 to 1
                    yield return null; // Wait for next frame
                }
                canvasGroup.alpha = 1f; // Ensure it's fully visible at the end
            }
        }

        // 5. Wait for the remainder of the respawn delay
        // We subtract the fadeDuration so the total wait time is consistent
        float remainingTime = respawnDelay - fadeDuration;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 6. RESPAWN
        if (playerController != null && CurrentCheckpoint != null)
        {
            playerController.transform.position = CurrentCheckpoint.transform.position;
        }

        // 7. Reset Animation
        if (anim != null)
        {
            anim.Play("Idle_King");
        }

        // 8. Hide Screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // 9. Unlock Movement
        if (playerController != null)
        {
            playerController.canMove = true;
        }
    }
}