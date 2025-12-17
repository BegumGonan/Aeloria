using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private Sprite[] energySprites = new Sprite[13];

    [Header("Sprites")]
    [SerializeField] private Image energyImage;

    private int currentInteractions = 0;
    private const int interactionsPerSpriteChange = 5;
    private int currentSpriteIndex = 0;
    private const int maxSpriteIndex = 12;

    public bool HasEnergy => currentSpriteIndex < maxSpriteIndex;

    private void Awake()
    {
        ResetEnergy();
    }

    public void ConsumeEnergy()
    {
        if (!HasEnergy)
        {
            Debug.Log("You don't have energy!");
            return;
        }

        currentInteractions++;

        if (currentInteractions >= interactionsPerSpriteChange)
        {
            ChangeSprite();
            currentInteractions = 0;
        }
    }

    public void ResetEnergy()
    {
        currentSpriteIndex = 0;
        currentInteractions = 0;
        UpdateSprite();
    }

    private void ChangeSprite()
    {
        currentSpriteIndex++;

        if (currentSpriteIndex > maxSpriteIndex)
            currentSpriteIndex = maxSpriteIndex;

        UpdateSprite();

        if (currentSpriteIndex == maxSpriteIndex - 1)
        {
            Debug.LogWarning("Your energy is running low! Be careful!");
        }

        if (currentSpriteIndex == maxSpriteIndex)
        {
            Debug.Log("You don't have energy!");
        }
    }

    public void DrinkEnergyPotion()
    {
        switch (currentSpriteIndex)
        {
            case 0: currentSpriteIndex = 0; break;
            case 1: currentSpriteIndex = 0; break;
            case 2: currentSpriteIndex = 0; break;
            case 3: currentSpriteIndex = 1; break;
            case 4: currentSpriteIndex = 2; break;
            case 5: currentSpriteIndex = 3; break;
            case 6: currentSpriteIndex = 4; break;
            case 7: currentSpriteIndex = 5; break;
            case 8: currentSpriteIndex = 6; break;
            case 9: currentSpriteIndex = 7; break;
            case 10: currentSpriteIndex = 8; break;
            case 11: currentSpriteIndex = 9; break;
            case 12: currentSpriteIndex = 10; break;
        }

        currentInteractions = 0;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (energyImage != null && energySprites.Length > currentSpriteIndex)
        {
            energyImage.sprite = energySprites[currentSpriteIndex];
        }
    }
}