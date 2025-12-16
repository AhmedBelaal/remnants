using UnityEngine;

public class IceTrap1 : MonoBehaviour
{
    public Animator animator;
    public int damage = 1;
    public float closeTime = 0.5f;   // how long it stays closed

    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player"))
        {
            canTrigger = false;

            // Close trap
            animator.SetBool("isClosed", true);

            // Damage player
            other.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            // Re-open after delay
            Invoke(nameof(OpenTrap), closeTime);
        }
    }

    void OpenTrap()
    {
        animator.SetBool("isClosed", false);
        canTrigger = true;
    }
}

