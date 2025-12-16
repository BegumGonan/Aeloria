using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CauldronUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject cauldronPanel;
    public List<PotionButtonUI> potionButtons;
    public TextMeshProUGUI feedbackText;

    [Header("Player References")]
    public InventoryManager playerInventory;
    public PlayerBehavior playerBehavior;
    
    private CauldronManager currentCauldron;

    private void Start()
    {
        cauldronPanel.SetActive(false);
    }

    public void Open(CauldronManager cauldron)
    {
        currentCauldron = cauldron;
        feedbackText.text = "";

        cauldronPanel.SetActive(true);
        playerBehavior.SetInventoryState(true);
    }

    public void Close()
    {
        cauldronPanel.SetActive(false);
        playerBehavior.SetInventoryState(false);
    }

    public void TryStartPotion(PotionData potion)
    {
        if (currentCauldron.isBrewing)
        {
            feedbackText.text = "The cauldron is already brewing.";
            return;
        }

        if (!currentCauldron.CanStartPotion(potion, playerInventory))
        {
            feedbackText.text = "You don't have all the ingredients.";
            return;
        }

        currentCauldron.StartPotion(potion, playerInventory);
        Close();
    }
}