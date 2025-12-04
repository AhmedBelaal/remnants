using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFade : MonoBehaviour
{
    public float delayBeforeFading = 4.0f; // Wait time (Intro + Reading time)
    public float fadeDuration = 3.0f;      // How long it takes to disappear

    public CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        StartCoroutine(ShowAndFade());
    }

    IEnumerator ShowAndFade()
    {
        // 1. Wait for the Intro Animation + Player reading time
        yield return new WaitForSeconds(delayBeforeFading);

        // 2. Fade Out Loop
        float timer = 0f;
        float startAlpha = canvasGroup.alpha;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Lerp from 1 down to 0
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
            yield return null;
        }

        // 3. Ensure it's gone and disable the object to save performance
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}