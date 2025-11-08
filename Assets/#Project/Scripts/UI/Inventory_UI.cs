using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InputActionAsset inputActionsAsset;
    [SerializeField] private PlayerBehavior player;
    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;

    public List<Slot_UI> slots = new List<Slot_UI>();
    private InputAction toggleInventoryAction;
    private InputAction rightClickAction;
    private bool dragSingle;

    private void Awake()
    {
        toggleInventoryAction = inputActionsAsset.FindActionMap("Player").FindAction("ToggleInventory");
        rightClickAction = inputActionsAsset.FindActionMap("Player").FindAction("DragSingle");

        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        toggleInventoryAction.Enable();
        toggleInventoryAction.performed += ToggleInventory;

        rightClickAction.Enable();
        rightClickAction.performed += ctx => dragSingle = true;
        rightClickAction.canceled += ctx => dragSingle = false;

        Refresh();
    }

    private void OnDisable()
    {
        toggleInventoryAction.performed -= ToggleInventory;
        toggleInventoryAction.Disable();

        rightClickAction.performed -= ctx => dragSingle = true;
        rightClickAction.canceled -= ctx => dragSingle = false;
        rightClickAction.Disable();
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        bool isNowOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isNowOpen);
        player.SetInventoryState(isNowOpen);
        if (isNowOpen)
        {
            Refresh();
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        player.SetInventoryState(false);
    }

    void Refresh()
    {
        if (player == null || slots.Count != player.inventory.Slots.Count) return;

        for (int i = 0; i < slots.Count; i++)
        {
            var invSlot = player.inventory.Slots[i];
            if (!string.IsNullOrEmpty(invSlot.itemName))
            {
                slots[i].SetItem(invSlot);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove()
    {
        if (draggedSlot == null) return;

        var slot = player.inventory.Slots[draggedSlot.slotID];
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(slot.itemName);

        if (itemToDrop != null)
        {
            int countToDrop = dragSingle ? 1 : slot.Count;

            player.DropItem(itemToDrop, countToDrop);
            player.inventory.Remove(draggedSlot.slotID, countToDrop);

            Refresh();
        }

        draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon.gameObject).GetComponent<Image>();
        draggedIcon.transform.SetParent(canvas.transform, false);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        if (draggedIcon != null)
            MoveToMousePosition(draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        if (draggedIcon != null)
            Destroy(draggedIcon.gameObject);

        draggedIcon = null;
        draggedSlot = null;

        Debug.Log("Done Dragging");
    }

    public void SlotDrop(Slot_UI slot)
    {
        if (draggedSlot == null || slot == null) return;

        Debug.Log($"Dropped: {draggedSlot.name} on {slot.name}");
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
}