using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        [SerializeField] private int count;
        [SerializeField] private int maxAllowed = 99;
        [SerializeField] private CollectableType type;
        [SerializeField] private Sprite icon;

        public CollectableType Type => type;
        public int Count => count;
        public Sprite Icon => icon;

        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            maxAllowed = 99;
            icon = null;
        }

        public bool CanAddItem()
        {
            return count < maxAllowed;
        }

        public void AddItem(Collectable item)
        {
            this.type = item.type;
            this.icon = item.icon;
            count++;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;

                if (count == 0)
                {
                    icon = null;
                    type = CollectableType.NONE;
                }
            }
        }
    }

    [SerializeField] private List<Slot> slots = new List<Slot>();
    public IReadOnlyList<Slot> Slots => slots;


    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Collectable item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.Type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.Type == CollectableType.NONE)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public int GetItemCount(CollectableType type)
    {
        int total = 0;
        foreach (Slot slot in slots)
        {
            if (slot.Type == type)
            {
                total += slot.Count;
            }
        }
        return total;
    }
}
