using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Farming/Crop Data")]
public class CropData : ItemData
{
    [Header("Seed Information")]
    public string seedName;
    public Item grownItemPrefab;

    [Header("Growth Parameters")]
    public int daysToGrow;

    [Header("Visuals (Growth Stages)")]
    public TileBase[] growthStages;
}