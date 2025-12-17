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

    private void Start() => cauldronPanel.SetActive(false);

    public void Open(CauldronManager cauldron, InventoryManager inventory)
    {
        currentCauldron = cauldron;
        playerInventory = inventory;
        playerBehavior = inventory.GetComponent<PlayerBehavior>();

        feedbackText.text = "";
        cauldronPanel.SetActive(true);
        playerBehavior.SetInventoryState(true);

        foreach (var button in potionButtons)
            button.Setup(this);
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
            feedbackText.text = "The cauldron is busy!";
            feedbackText.color = Color.yellow;
            return;
        }

        if (!currentCauldron.CanStartPotion(potion, playerInventory))
        {
            feedbackText.text = "Missing ingredients!";
            feedbackText.color = Color.red;
            return;
        }

        currentCauldron.StartPotion(potion, playerInventory);
        feedbackText.text = "Brewing started!";
        feedbackText.color = Color.green;

        Invoke(nameof(Close), 1.5f);
    }
}