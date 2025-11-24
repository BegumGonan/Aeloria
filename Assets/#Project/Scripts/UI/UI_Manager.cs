using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();
    [Header("Panels")]
    public GameObject inventoryPanel;
    public GameObject toolbarPanel;
    public List<Inventory_UI> inventoryUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActionsAsset;

    private InputAction toggleInventoryAction;
    private InputAction rightClickAction;

    private void Awake()
    {
        Initialize();

        var playerMap = inputActionsAsset.FindActionMap("Player");
        toggleInventoryAction = playerMap?.FindAction("ToggleInventory");
        rightClickAction = playerMap?.FindAction("DragSingle");
    }

    private void OnEnable()
    {
        if (toggleInventoryAction != null)
        {
            toggleInventoryAction.Enable();
            toggleInventoryAction.performed += ToggleInventoryUI;
        }

        if (rightClickAction != null)
        {
            rightClickAction.Enable();
            rightClickAction.started += OnRightClickStarted;
            rightClickAction.canceled += OnRightClickCanceled;
        }
    }

    private void OnDisable()
    {
        if (toggleInventoryAction != null)
        {
            toggleInventoryAction.performed -= ToggleInventoryUI;
            toggleInventoryAction.Disable();
        }

        if (rightClickAction != null)
        {
            rightClickAction.started -= OnRightClickStarted;
            rightClickAction.canceled -= OnRightClickCanceled;
            rightClickAction.Disable();
        }
    }

    private void OnRightClickStarted(InputAction.CallbackContext ctx) => dragSingle = true;
    private void OnRightClickCanceled(InputAction.CallbackContext ctx) => dragSingle = false;

    private void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
                inventoryUIByName.Add(ui.inventoryName, ui);
        }

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (toolbarPanel != null) toolbarPanel.SetActive(true);
    }

    private void ToggleInventoryUI(InputAction.CallbackContext context)
    {
        if (inventoryPanel == null) return;

        bool isNowOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isNowOpen);
        RefreshInventoryUI("Backpack");
        if (toolbarPanel != null) toolbarPanel.SetActive(!isNowOpen);

        GameManager.instance.player.SetInventoryState(isNowOpen);
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
            inventoryUIByName[inventoryName].Refresh();
    }

    public void RefreshAll()
    {
        foreach (var kvp in inventoryUIByName)
            kvp.Value.Refresh();
    }

    public Inventory_UI GetInventory_UI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
            return inventoryUIByName[inventoryName];

        return null;
    }
}