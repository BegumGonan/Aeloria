using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory_UI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InputActionAsset inputActionsAsset;
    [SerializeField] private PlayerBehavior player;
    public List<Slot_UI> slots = new List<Slot_UI>();

    private InputAction toggleInventoryAction;

    private void Awake()
    {
        toggleInventoryAction = inputActionsAsset.FindActionMap("Player").FindAction("ToggleInventory");
    }

    private void OnEnable()
    {
        toggleInventoryAction.Enable();
        toggleInventoryAction.performed += ToggleInventory;
        Refresh();
    }

    private void OnDisable()
    {
        toggleInventoryAction.performed -= ToggleInventory;
        toggleInventoryAction.Disable();
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            Refresh();
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    void Refresh()
    {
        if (player == null || slots.Count != player.inventory.Slots.Count) return;

        for (int i = 0; i < slots.Count; i++)
        {
            var invSlot = player.inventory.Slots[i];
            if (invSlot.Type != CollectableType.NONE)
            {
                slots[i].SetItem(invSlot);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove(int slotID)
    {
        var slot = player.inventory.Slots[slotID];

        Collectable itemToDrop = GameManager.instance.itemManager.GetItemByType(slot.Type);

        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    }
}

