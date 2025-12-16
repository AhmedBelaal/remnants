using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public string nextLevelName = "Level0"; // The actual game scene (Prologue)
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Automatically call LoadNextLevel when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Skip if Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextLevel();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}