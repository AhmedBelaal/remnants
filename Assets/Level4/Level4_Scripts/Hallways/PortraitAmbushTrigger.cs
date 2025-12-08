using UnityEngine;

public class PortraitAmbushTrigger : MonoBehaviour
{
    [Header("Fake Knights (To Hide)")]
    public GameObject fakeKnightL;
    public GameObject fakeKnightR;

    [Header("Real Knights (To Spawn)")]
    public GameObject realKnightL;
    public GameObject realKnightR;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player and the ambush hasn't happened yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            SpringAmbush();
        }
    }

    private void SpringAmbush()
    {
        hasTriggered = true;

        // 1. Hide the fake static images
        if (fakeKnightL != null) fakeKnightL.SetActive(false);
        if (fakeKnightR != null) fakeKnightR.SetActive(false);

        // 2. Activate the real enemy knights
        if (realKnightL != null) realKnightL.SetActive(true);
        if (realKnightR != null) realKnightR.SetActive(true);

        // Optional: Disable this trigger so it can't be activated again
        GetComponent<Collider2D>().enabled = false;
        
        Debug.Log("Ambush Sprung!");
    }
}