using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Animator doorAnimator; // Drag the Door/Painting object here
    public string triggerName = "DoorOpen"; // The name of your parameter in the Animator

    private bool hasOpened = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasOpened)
        {
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger(triggerName);
                hasOpened = true; // Prevent re-triggering
            }
        }
    }
}