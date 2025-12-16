using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion Data")]
public class PotionData : ScriptableObject
{
    [Header("Basic Info")]
    public string potionName;
    public Item potionItem;
    public Sprite potionIcon;

    [Header("Craft Settings")]
    public float brewTimeInGameHours;
    public int outputCount = 3;

    [Header("Ingredients")]
    public List<Ingredient> ingredients = new List<Ingredient>();

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int amount;
    }
}