using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] item;
    private Dictionary<string, Item> nameToItemDic = new Dictionary<string, Item>();

    private void Awake()
    {
        foreach (Item item in item)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        if (!nameToItemDic.ContainsKey(item.data.itemName))
        {
            nameToItemDic.Add(item.data.itemName, item);
        }
    }
    
    public Item GetItemByName(string key)
    {
        if (nameToItemDic.ContainsKey(key))
        {
            return nameToItemDic[key];
        }
        return null;
    }
}
