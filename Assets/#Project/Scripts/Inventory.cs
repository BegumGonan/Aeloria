using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public int count;
        public int maxAllowed = 99;
        public Sprite icon;
        public string itemName;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
            icon = null;
        }

        public bool IsEmpty
        {
            get
            {
                if (itemName == "" && count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool CanAddItem(string itemName)
        {
            if (this.itemName == itemName && count < maxAllowed)
            {
                return true;
            }
            return false;
        }

        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            this.icon = item.data.icon;
            count++;
        }

        public void AddItem(string itemName, Sprite icon, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.maxAllowed = maxAllowed;
            count++;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;

                if (count <= 0)
                {
                    icon = null;
                    itemName = "";
                }
            }
        }
    }

    [SerializeField] private List<Slot> slots = new List<Slot>();
    public IReadOnlyList<Slot> Slots => slots;
    public Slot selectedSlot = null;
    
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
            if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item.data.itemName, item.data.icon, slot.maxAllowed);
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

    public void Remove(int index, int numToRemove)
    {
        Slot slot = Slots[index];
        for (int i = 0; i < numToRemove; i++)
        {
            if (slot.count > 0)
                slot.RemoveItem();
            else
                break;
        }
    }
    
    public bool TryRemoveSingleItem(string itemName)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == itemName && slot.count > 0)
            {
                slot.RemoveItem();
                return true;
            }
        }
        return false;
    }


    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if (fromSlot.IsEmpty) return;

        int actualToMove = Mathf.Min(numToMove, fromSlot.count); 

        for (int i = 0; i < actualToMove; i++) 
        {
            if (toSlot.IsEmpty)
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
            }
            else if (toSlot.CanAddItem(fromSlot.itemName))
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
            }
            else
            {
                break;
            }
            
            fromSlot.RemoveItem();
            if (fromSlot.IsEmpty) break;
        }
    }

    public int GetItemCount(Item item)
    {
        int total = 0;
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName)
            {
                total += slot.count;
            }
        }
        return total;
    }

    public void SelectedSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }
}