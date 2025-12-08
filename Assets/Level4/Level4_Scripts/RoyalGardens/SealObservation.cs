using UnityEngine;

public class SealObservation : MonoBehaviour
{
    public PuzzleStatue puzzleStatue; // Drag the Statue object here

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleStatue != null && !puzzleStatue.hasSeenSeal)
            {
                puzzleStatue.RevealSeal();
                Debug.Log("Seal Observed! Statue is now active.");
            }
        }
    }
}