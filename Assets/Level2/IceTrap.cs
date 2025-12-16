using UnityEngine;

public class IceTrap : MonoBehaviour
{
    public Animator animator;     // animator of the trap
    public int damage = 1;        // damage to player
    public float resetTime = 1f;  // time before trap opens again

    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player"))
        {
            canTrigger = false;

            // play close animation
            animator.SetTrigger("Close");

            // damage player
            other.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            // reopen trap after time
            Invoke(nameof(ResetTrap), resetTime);
        }
    }

    void ResetTrap()
    {
        canTrigger = true;
    }
}
