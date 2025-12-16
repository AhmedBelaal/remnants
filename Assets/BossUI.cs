using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public Slider healthSlider;

    void Awake()
    {
        // Auto-assign if you forgot to drag it in
        if (healthSlider == null) 
            healthSlider = GetComponent<Slider>();
            
        // Hide bar immediately so it doesn't show before the boss wakes up
        gameObject.SetActive(false); 
    }

    public void ActivateBossUI(int maxHealth)
    {
        gameObject.SetActive(true); // Show the bar
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}