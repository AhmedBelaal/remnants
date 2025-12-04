using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeInteraction : MonoBehaviour
{
    
    public string nextLevelName = "Level1"; 


    public GameObject interactionPrompt;
    public CanvasGroup fadeScreen;     
    public float fadeDuration = 2.0f;
    public KeyCode interactionKey;
    private bool inRange = false;
    private bool isTransitioning = false;

    void Start()
    {
        if(interactionPrompt != null) interactionPrompt.SetActive(false);
        
        if(fadeScreen != null) fadeScreen.alpha = 0f;
    }

    void Update()
    {
        if (inRange && !isTransitioning)
        {
            if (Input.GetKeyDown(interactionKey)) 
            {
                StartCoroutine(EndPrologueSequence());
            }
        }
    }

    IEnumerator EndPrologueSequence()
    {
        isTransitioning = true;
        
        if(interactionPrompt != null) interactionPrompt.SetActive(false);

        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.canMove = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             if(player.GetComponent<Animator>()) player.GetComponent<Animator>().Play("Dead_King");
        }

        // 3. FADE TO BLACK LOGIC
        if (fadeScreen != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                // Lerp alpha from 0 to 1
                fadeScreen.alpha = timer / fadeDuration; 
                yield return null; // Wait for next frame
            }
            fadeScreen.alpha = 1f; // Force full black at the end
        }
        else
        {
            // Safety wait if you forgot to assign the fade screen
            yield return new WaitForSeconds(1.0f);
        }

        // 4. Load the next scene
        SceneManager.LoadScene(nextLevelName);
    }

    // --- DETECT PLAYER ENTERING ZONE ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            // SHOW THE PROMPT
            if(interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    // --- DETECT PLAYER LEAVING ZONE ---
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            // HIDE THE PROMPT
            if(interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}