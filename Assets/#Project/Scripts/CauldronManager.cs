using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CauldronManager : MonoBehaviour
{
    [Header("State")]
    public bool isBrewing = false;
    public bool potionReady = false;

    [Header("Current Brew")]
    public PotionData currentPotion;
    private float brewTimer;
    private float animationTimer;
    private int currentAnimationIndex;

    [Header("Tile References")]
    public Tilemap cauldronTilemap;
    public Tile idleTile;
    public List<Tile> brewingTiles;
    public Tile readyTile;

    [Header("Spawn Settings")]
    public Transform potionSpawnPoint;
    private GameObject currentVisual;

    private Vector3Int tilePos;
    private InventoryManager playerInventory;
    private TimeManager timeManager;
    public float animationSpeed = 0.25f;

    private void Start()
    {
        timeManager = GameManager.instance.timeManager;
        foreach (var pos in cauldronTilemap.cellBounds.allPositionsWithin)
        {
            if (cauldronTilemap.GetTile(pos) != null)
            {
                tilePos = pos;
                break;
            }
        }
        cauldronTilemap.SetTile(tilePos, idleTile);
    }

    private void Update()
    {
        if (!isBrewing || potionReady) return;

        brewTimer += Time.deltaTime * timeManager.timeSpeed;
        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentAnimationIndex = (currentAnimationIndex + 1) % brewingTiles.Count;
            cauldronTilemap.SetTile(tilePos, brewingTiles[currentAnimationIndex]);
        }

        if (brewTimer >= (currentPotion.brewTimeInGameHours * 60f))
        {
            FinishPotion();
        }
    }

    public bool CanStartPotion(PotionData potion, InventoryManager inventory)
    {
        foreach (var ingredient in potion.ingredients)
        {
            int count = inventory.backpack.GetItemCount(ingredient.item) +
                        inventory.toolbar.GetItemCount(ingredient.item);
            if (count < ingredient.amount) return false;
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
        animationTimer = 0f;
        isBrewing = true;
        potionReady = false;
    }

    private void ConsumeIngredients(PotionData potion, InventoryManager inventory)
    {
        foreach (var ingredient in potion.ingredients)
        {
            for (int i = 0; i < ingredient.amount; i++)
            {
                if (!inventory.toolbar.TryRemoveSingleItem(ingredient.item.data.itemName))
                    inventory.backpack.TryRemoveSingleItem(ingredient.item.data.itemName);
            }
        }
        GameManager.instance.uiManager.RefreshAll();
    }

    private void FinishPotion()
    {
        potionReady = true;
        isBrewing = false;
        cauldronTilemap.SetTile(tilePos, readyTile);

        if (currentPotion != null && potionSpawnPoint != null)
        {
            currentVisual = Instantiate(currentPotion.potionItem.gameObject, potionSpawnPoint.position, Quaternion.identity);
            currentVisual.name = "FinishedPotionVisual";
            if(currentVisual.GetComponent<Collider2D>()) currentVisual.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void CollectPotion()
    {
        if (!potionReady || currentPotion == null) return;

        for (int i = 0; i < currentPotion.outputCount; i++)
        {
            playerInventory.Add("Backpack", currentPotion.potionItem);
        }

        if (currentVisual != null) Destroy(currentVisual);
        ResetCauldron();
        GameManager.instance.uiManager.RefreshAll();
    }

    private void ResetCauldron()
    {
        currentPotion = null;
        isBrewing = false;
        potionReady = false;
        cauldronTilemap.SetTile(tilePos, idleTile);
    }
}