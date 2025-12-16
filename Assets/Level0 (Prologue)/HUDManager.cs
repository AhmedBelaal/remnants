using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("Health Bar")]
    public Image[] heartSlots;          // Drag your 3 Heart Image objects here
    public Sprite heartFull;            // Drag 'heart_Full'
    public Sprite heartEmpty;           // Drag 'heart_Empty'

    [Header("Abilities")]
    public Image fireballIcon;          // Drag UI Image for Fireball
    public Sprite fireballReady;        // Drag 'fireball_Ready'
    public Sprite fireballCooldown;     // Drag 'fireball_Cooldown'

    public Image shieldIcon;            // Drag UI Image for Shield
    public Sprite shieldReady;          // Drag 'liquidShield_Ready'
    public Sprite shieldCooldown;       // Drag 'liquidShield_Cooldown'

    [Header("Memory Progress")]
    public Image memoryIcon;            // Drag the big Memory Fragment UI Image
    // Drag sprites in this order: Empty, Quarter, Half, 3over4, Full
    public Sprite[] memoryStages;       

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < heartSlots.Length; i++)
        {
            if (i < currentHealth) heartSlots[i].sprite = heartFull;
            else heartSlots[i].sprite = heartEmpty;

            // Handle cases where max health changes (optional safety)
            heartSlots[i].enabled = (i < maxHealth);
        }
    }

    public void SetFireballState(bool isReady)
    {
        if (fireballIcon != null)
            fireballIcon.sprite = isReady ? fireballReady : fireballCooldown;
    }

    public void SetShieldState(bool isReady)
    {
        if (shieldIcon != null)
            shieldIcon.sprite = isReady ? shieldReady : shieldCooldown;
    }

    public void UpdateMemoryStage(int stageIndex)
    {
        // 0 = Empty, 1 = Quarter, 2 = Half, 3 = 3/4, 4 = Full
        if (stageIndex >= 0 && stageIndex < memoryStages.Length)
        {
            memoryIcon.sprite = memoryStages[stageIndex];
        }
    }
}