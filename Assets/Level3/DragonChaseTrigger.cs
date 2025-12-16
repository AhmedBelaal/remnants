using UnityEngine;

public class DragonChaseTrigger : MonoBehaviour
{
    public DRAGONCAHSE  dragon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dragon != null)
        {
            dragon.StartChase(other.transform);
        }
    }
}
