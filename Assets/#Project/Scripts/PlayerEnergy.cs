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

    private void UpdateSprite()
    {
        if (energyImage != null && energySprites.Length > currentSpriteIndex)
        {
            energyImage.sprite = energySprites[currentSpriteIndex];
        }
    }
}