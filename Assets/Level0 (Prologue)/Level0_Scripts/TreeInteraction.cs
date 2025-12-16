using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeInteraction : MonoBehaviour
{
    [Header("Level Navigation")]
    public string nextLevelName = "Level2"; // Change this in Inspector!
    
    [Header("Memory Reward")]
    [Tooltip("1=Quarter, 2=Half, 3=3/4, 4=Full")]
    public int memoryStageReward = 2; // Default to Half for the Level 2 tree

    [Header("UI & Effects")]
    public GameObject interactionPrompt;
    public CanvasGroup fadeScreen;     
    public float fadeDuration = 2.0f;
    public KeyCode interactionKey = KeyCode.Return; // Enter Key
    
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
                StartCoroutine(EndLevelSequence());
            }
        }
    }

    IEnumerator EndLevelSequence()
    {
        isTransitioning = true;
        
        // 1. UPDATE HUD (Generic)
        if (HUDManager.instance != null)
        {
            HUDManager.instance.UpdateMemoryStage(memoryStageReward);
        }

        if(interactionPrompt != null) interactionPrompt.SetActive(false);

        // 2. Disable Player
        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.canMove = false;
            if(player.GetComponent<Rigidbody2D>()) player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            // Optional: Play the "Fainting" animation if you want the same effect
            if(player.GetComponent<Animator>()) player.GetComponent<Animator>().Play("Dead_King");
        }

        // 3. FADE TO BLACK
        if (fadeScreen != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeScreen.alpha = timer / fadeDuration; 
                yield return null; 
            }
            fadeScreen.alpha = 1f; 
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }

        // 4. Load Next Scene
        SceneManager.LoadScene(nextLevelName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if(interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            if(interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}

// using System.Collections;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class TreeInteraction : MonoBehaviour
// {
//     public string nextLevelName = "Level1"; 
//     public GameObject interactionPrompt;
//     public CanvasGroup fadeScreen;     
//     public float fadeDuration = 2.0f;
//     public KeyCode interactionKey = KeyCode.Return; 
//     private bool inRange = false;
//     private bool isTransitioning = false;

//     void Start()
//     {
//         if(interactionPrompt != null) interactionPrompt.SetActive(false);
//         if(fadeScreen != null) fadeScreen.alpha = 0f;
//     }

//     void Update()
//     {
//         if (inRange && !isTransitioning)
//         {
//             if (Input.GetKeyDown(interactionKey)) 
//             {
//                 StartCoroutine(EndPrologueSequence());
//             }
//         }
//     }

//     IEnumerator EndPrologueSequence()
//     {
//         isTransitioning = true;
        
//         // --- 1. UPDATE HUD VISUALLY (Zero -> Quarter) ---
//         if (HUDManager.instance != null)
//         {
//             HUDManager.instance.UpdateMemoryStage(1); // 1 = Quarter
//         }

//         if(interactionPrompt != null) interactionPrompt.SetActive(false);

//         // 2. Disable Player
//         PlayerControl player = FindObjectOfType<PlayerControl>();
//         if (player != null)
//         {
//             player.canMove = false;
//             if(player.GetComponent<Rigidbody2D>()) player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//             if(player.GetComponent<Animator>()) player.GetComponent<Animator>().Play("Dead_King"); // Or your specific cutscene anim
//         }

//         // 3. FADE TO BLACK
//         if (fadeScreen != null)
//         {
//             float timer = 0f;
//             while (timer < fadeDuration)
//             {
//                 timer += Time.deltaTime;
//                 fadeScreen.alpha = timer / fadeDuration; 
//                 yield return null; 
//             }
//             fadeScreen.alpha = 1f; 
//         }
//         else
//         {
//             yield return new WaitForSeconds(1.0f);
//         }

//         // 4. Load Next Scene
//         SceneManager.LoadScene(nextLevelName);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             inRange = true;
//             if(interactionPrompt != null) interactionPrompt.SetActive(true);
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             inRange = false;
//             if(interactionPrompt != null) interactionPrompt.SetActive(false);
//         }
//     }
// }