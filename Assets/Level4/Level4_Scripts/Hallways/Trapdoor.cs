using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    [Header("Trap Components")]
    public GameObject fakeFloor; // The floor that disappears
    public Collider2D trapTrigger; // The trigger box itself (Assign this!)

    [Header("Settings")]
    public float delay = 0.0f;

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            if (delay > 0) Invoke("OpenTrap", delay);
            else OpenTrap();
        }
    }

    void OpenTrap()
    {
        if (fakeFloor != null)
        {
            // Disable visuals and physics so player falls
            fakeFloor.SetActive(false); 
        }
    }

    public void RepairFloor()
    {
        // 1. Bring the floor back (Visuals + Collider)
        if (fakeFloor != null)
        {
            fakeFloor.SetActive(true);
        }

        // 2. Disable the TRAP TRIGGER so it doesn't open again
        if (trapTrigger != null)
        {
            trapTrigger.enabled = false;
        }
        else
        {
            // Fallback: Disable this script and its collider
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }

        Debug.Log("Trapdoor repaired! Safe to walk.");
    }
}