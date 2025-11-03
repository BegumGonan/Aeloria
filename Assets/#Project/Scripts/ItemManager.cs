using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;
    private Dictionary<CollectableType, Collectable> collectableItemsDic = new Dictionary<CollectableType, Collectable>();

    private void Awake()
    {
        foreach (Collectable item in collectableItems)
        {
            AddItem(item);
        }
    }

    private void AddItem(Collectable item)
    {
        if (!collectableItemsDic.ContainsKey(item.type))
        {
            collectableItemsDic.Add(item.type, item);
        }
    }
    
    public Collectable GetItemByType(CollectableType type)
    {
        if (collectableItemsDic.ContainsKey(type))
        {
            return collectableItemsDic[type];
        }
        return null;
    }
}
