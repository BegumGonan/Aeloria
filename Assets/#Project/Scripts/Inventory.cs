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
        public Item prefabItem;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
            icon = null;
            prefabItem = null;
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
            this.prefabItem = item;
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
                prefabItem = null;
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
            if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
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
    
    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory)
    {
        if (fromIndex < 0 || fromIndex >= slots.Count)
        {
            Debug.LogWarning($"MoveSlot: fromIndex out of range: {fromIndex} (slots.Count={slots.Count})");
            return;
        }
        if (toInventory == null)
        {
            Debug.LogWarning("MoveSlot: toInventory is null");
            return;
        }
        if (toIndex < 0 || toIndex >= toInventory.slots.Count)
        {
            Debug.LogWarning($"MoveSlot: toIndex out of range: {toIndex} (toInventory.slots.Count={toInventory.slots.Count})");
            return;
        }

        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if (fromSlot.IsEmpty) return;

        int toMove = Inventory_UI.dragSingleForThisDrag ? 1 : fromSlot.count;

        for (int i = 0; i < toMove; i++)
        {
            if (toSlot.IsEmpty)
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
                if (fromSlot.prefabItem != null)
                    toSlot.prefabItem = fromSlot.prefabItem;
            }
            else if (toSlot.CanAddItem(fromSlot.itemName))
            {
                if (fromSlot.prefabItem != null)
                    toSlot.AddItem(fromSlot.prefabItem);
                else
                    toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);

                if (toSlot.prefabItem == null && fromSlot.prefabItem != null)
                    toSlot.prefabItem = fromSlot.prefabItem;
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
}