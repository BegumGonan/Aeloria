using UnityEngine;

public class CauldronManager : MonoBehaviour
{
    [Header("State")]
    public bool isBrewing = false;
    public bool potionReady = false;

    [Header("Current Brew")]
    public PotionData currentPotion;
    private float brewTimer;

    [Header("References")]
    private InventoryManager playerInventory;
    private TimeManager timeManager;

    private void Start()
    {
        timeManager = GameManager.instance.timeManager;
    }

    private void Update()
    {
        if (!isBrewing || potionReady) return;

        brewTimer += Time.deltaTime * timeManager.timeSpeed;

        float targetMinutes = currentPotion.brewTimeInGameHours * 60f;

        if (brewTimer >= targetMinutes)
        {
            FinishPotion();
        }
    }

    public bool CanStartPotion(PotionData potion, InventoryManager inventory)
    {
        foreach (var ingredient in potion.ingredients)
        {
            int count =
                inventory.backpack.GetItemCount(ingredient.item) +
                inventory.toolbar.GetItemCount(ingredient.item);

            if (count < ingredient.amount)
                return false;
        }
        return true;
    }

    public void StartPotion(PotionData potion, InventoryManager inventory)
    {
        if (isBrewing) return;

        currentPotion = potion;
        playerInventory = inventory;

        ConsumeIngredients(potion, inventory);

        brewTimer = 0f;
        isBrewing = true;
        potionReady = false;
    }

    private void ConsumeIngredients(PotionData potion, InventoryManager inventory)
    {
        foreach (var ingredient in potion.ingredients)
        {
            int remaining = ingredient.amount;

            while (remaining > 0)
            {
                if (inventory.toolbar.TryRemoveSingleItem(ingredient.item.data.itemName))
                {
                    remaining--;
                    continue;
                }

                if (inventory.backpack.TryRemoveSingleItem(ingredient.item.data.itemName))
                {
                    remaining--;
                    continue;
                }

                break;
            }
        }

        GameManager.instance.uiManager.RefreshAll();
    }

    private void FinishPotion()
    {
        potionReady = true;
        isBrewing = false;
    }

    public void CollectPotion()
    {
        if (!potionReady || currentPotion == null) return;

        for (int i = 0; i < currentPotion.outputCount; i++)
        {
            playerInventory.Add("Backpack", currentPotion.potionItem);
        }

        ResetCauldron();
        GameManager.instance.uiManager.RefreshAll();
    }

    private void ResetCauldron()
    {
        currentPotion = null;
        brewTimer = 0f;
        potionReady = false;
    }
}