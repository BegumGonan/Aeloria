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
        [SerializeField] private Sprite icon;

        public string itemName;
        public int Count => count;
        public Sprite Icon => icon;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
            icon = null;
        }

        public bool CanAddItem()
        {
            return count < maxAllowed;
        }

        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
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
                    itemName = "";
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

    public void Add(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
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

    public int GetItemCount(Item item)
    {
        int total = 0;
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName)
            {
                total += slot.Count;
            }
        }
        return total;
    }
}
