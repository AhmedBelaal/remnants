using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("UI Assignments")]
    public GameObject choicePanel; // Drag 'EndingPanel' here

    [Header("Scene Names")]
    public string spareScene = "Ending_Spare";
    public string executeScene = "Ending_Execute";

    // Called automatically by KeeperBoss.cs when he dies
    public void ShowEndingChoice()
    {
        Debug.Log("Boss Defeated. Showing Choice.");
        if (choicePanel != null) 
        {
            choicePanel.SetActive(true); // Show the UI
            Time.timeScale = 0f; // PAUSE the game physics
        }
    }

    public void ChooseSpare()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(spareScene);
    }

    public void ChooseExecute()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(executeScene);
    }
}