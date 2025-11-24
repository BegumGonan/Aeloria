using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas canvas;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActionsAsset;

    [Header("Slots")]
    public List<Slot_UI> slots = new List<Slot_UI>();
    public string inventoryName;

    private InputAction toggleInventoryAction;
    public static bool dragSingleForThisDrag;
    private Inventory inventory;
    public RectTransform inventoryPanelRect;

    private void Awake()
    {
        toggleInventoryAction = inputActionsAsset.FindActionMap("Player")?.FindAction("ToggleInventory");

        if (canvas == null) 
            canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        toggleInventoryAction?.Enable();
        Refresh();
    }

    private void OnDisable()
    {
        toggleInventoryAction?.Disable();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    public void CloseInventory()
    {
        GameManager.instance.player.SetInventoryState(false);

        if (GameManager.instance.uiManager != null)
            GameManager.instance.uiManager.inventoryPanel.SetActive(false);
    }

    public void Refresh()
    {
        if (inventory == null || slots.Count == 0) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.Slots.Count)
            {
                var invSlot = inventory.Slots[i];
                if (invSlot.count > 0)
                    slots[i].SetItem(invSlot);
                else
                    slots[i].SetEmpty();
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove(int countToDrop)
    {
        if (UI_Manager.draggedSlot == null) return;

        Inventory sourceInventory = UI_Manager.draggedSlot.inventory;
        int sourceIndex = UI_Manager.draggedSlot.slotID;

        if (sourceInventory == null) return;
        if (sourceIndex < 0 || sourceIndex >= sourceInventory.Slots.Count) return;

        var slot = sourceInventory.Slots[sourceIndex];
        Item itemToDrop = slot.prefabItem;

        GameManager.instance.player.DropItem(itemToDrop, countToDrop);
        sourceInventory.Remove(sourceIndex, countToDrop);
        GameManager.instance.uiManager.RefreshAll();

        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;
        dragSingleForThisDrag = UI_Manager.dragSingle;

        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon.gameObject).GetComponent<Image>();
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform, false);
        UI_Manager.draggedIcon.raycastTarget = false;

        RectTransform rt = UI_Manager.draggedIcon.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(75, 75);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        if (UI_Manager.draggedIcon != null)
            MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        if (UI_Manager.draggedIcon != null)
            Destroy(UI_Manager.draggedIcon.gameObject);

        if (UI_Manager.draggedSlot != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            bool pointerOutside = !RectTransformUtility.RectangleContainsScreenPoint(
                inventoryPanelRect, 
                mousePos, 
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null
            );

            Debug.Log("PointerOutsideInventory: " + pointerOutside);

            if (pointerOutside)
            {
                int countToDrop = UI_Manager.dragSingle ? 1 : UI_Manager.draggedSlot.inventory.Slots[UI_Manager.draggedSlot.slotID].count;
                Remove(countToDrop);
            }
        }

        UI_Manager.draggedIcon = null;
        UI_Manager.draggedSlot = null;
    }

    public void SlotDrop(Slot_UI slot)
    {
        if (UI_Manager.draggedSlot == null || slot == null) return;
        if (UI_Manager.draggedSlot == slot) return;

        Inventory sourceInventory = UI_Manager.draggedSlot.inventory;
        int fromIndex = UI_Manager.draggedSlot.slotID;
        Inventory destInventory = slot.inventory;
        int toIndex = slot.slotID;

        if (sourceInventory == null)
        {
            Debug.LogWarning("SlotDrop: sourceInventory is null");
            return;
        }

        sourceInventory.MoveSlot(fromIndex, toIndex, destInventory);
        GameManager.instance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas == null) return;

        Vector2 position;
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePos,
            cam,
            out position
        );

        toMove.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private void SetupSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotID = i;
            slots[i].inventory = inventory;
        }
    }
}