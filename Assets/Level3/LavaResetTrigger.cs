using UnityEngine;

public class LavaResetTrigger : MonoBehaviour
{
    public LavaRise lava;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lava.ResetLava();
        }
    }
}
