using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionUseUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelRoot;
    public TextMeshProUGUI messageText;
    public Button yesButton;
    public Button noButton;

    private PlayerEnergy playerEnergy;
    private InventoryManager inventoryManager;
    private Inventory inventory;
    private string potionName;

    private void Awake()
    {
        panelRoot.SetActive(false);
    }

    public void Open(PlayerEnergy energy, InventoryManager inv, string potionName)
    {
        this.playerEnergy = energy;
        this.inventoryManager = inv;
        this.inventory = inv.backpack;
        this.potionName = potionName;

        messageText.text = $"Do you want to drink {potionName}?";
        panelRoot.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesClicked);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(Close);
    }

    private void OnYesClicked()
    {
        if (potionName != "Energy Potion") return;

        playerEnergy.DrinkEnergyPotion();

        if (inventoryManager.toolbar != null && inventoryManager.toolbar.selectedSlot != null)
        {
            var toolbarSlot = inventoryManager.toolbar.selectedSlot;
            if (toolbarSlot.itemName == potionName)
            {
                toolbarSlot.RemoveItem();

                if (toolbarSlot.IsEmpty)
                {
                    inventoryManager.toolbar.selectedSlot = null;
                }

                FinalizeConsumption();
                return;
            }
        }

        inventoryManager.backpack.TryRemoveSingleItem(potionName);
        FinalizeConsumption();
    }

    private void FinalizeConsumption()
    {
        GameManager.instance.uiManager.RefreshAll();
        Close();
    }

    public void Close()
    {
        panelRoot.SetActive(false);
    }
}