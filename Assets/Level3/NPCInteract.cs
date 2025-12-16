using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject interactionUI;
    private bool playerInRange;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (interactionUI != null)
            interactionUI.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }
}
