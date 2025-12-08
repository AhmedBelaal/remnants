using UnityEngine;

public class PuzzleStatue : MonoBehaviour
{
    public CastleGate linkedGate;   
    public GameObject hintUI;       

    // Starts false, becomes true when player visits the gate
    public bool hasSeenSeal = false; 

    private bool inRange = false;
    private bool isSolved = false;

    void Start()
    {
        if (hintUI != null) hintUI.SetActive(false);
    }

    void Update()
    {
        // Only allow solving if they have seen the seal!
        if (inRange && hasSeenSeal && !isSolved && Input.GetKeyDown(KeyCode.Return))
        {
            Solve();
        }
    }

    void Solve()
    {
        isSolved = true;
        if (hintUI != null) hintUI.SetActive(false);
        if (linkedGate != null) linkedGate.Unlock();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSolved)
        {
            inRange = true;
            // Only show the prompt if they know about the seal
            if (hasSeenSeal && hintUI != null) 
            {
                hintUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            if (hintUI != null) hintUI.SetActive(false);
        }
    }

    // Called by the Gate Trigger
    public void RevealSeal()
    {
        hasSeenSeal = true;
    }
}