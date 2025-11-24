using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlot = new List<Slot_UI>();
    [SerializeField] private InputActionAsset inputActionsAsset;

    private InputActionMap toolbarActionMap;
    private InputAction scrollAction;
    private Slot_UI selectedSlot;
    private int currentIndex = 0;

    private void Awake()
    {
        toolbarActionMap = inputActionsAsset.FindActionMap("UI");
        toolbarActionMap.FindAction("Select1").performed += SelectSlot1;
        toolbarActionMap.FindAction("Select2").performed += SelectSlot2;
        toolbarActionMap.FindAction("Select3").performed += SelectSlot3;
        toolbarActionMap.FindAction("Select4").performed += SelectSlot4;
        toolbarActionMap.FindAction("Select5").performed += SelectSlot5;
        toolbarActionMap.FindAction("Select6").performed += SelectSlot6;
        toolbarActionMap.FindAction("Select7").performed += SelectSlot7;
        toolbarActionMap.FindAction("Select8").performed += SelectSlot8;
        toolbarActionMap.FindAction("Select9").performed += SelectSlot9;

        scrollAction = toolbarActionMap.FindAction("ScrollWheel");
        scrollAction.performed += OnScrollPerformed;
    }

    private void OnEnable() => toolbarActionMap?.Enable();
    private void OnDisable() => toolbarActionMap?.Disable();
    private void Start() => SelectSlot(0);

    public void SelectSlot(int index)
    {
        if (toolbarSlot.Count == 0) return;

        if (selectedSlot != null)
        {
            selectedSlot.SetHiglight(false);
        }

        index = (index + toolbarSlot.Count) % toolbarSlot.Count;
        currentIndex = index;
        selectedSlot = toolbarSlot[index];
        selectedSlot.SetHiglight(true);

        GameManager.instance.player.inventory.toolbar.SelectedSlot(index);
    }

    public void SelectSlot1(InputAction.CallbackContext context) => SelectSlot(0);
    public void SelectSlot2(InputAction.CallbackContext context) => SelectSlot(1);
    public void SelectSlot3(InputAction.CallbackContext context) => SelectSlot(2);
    public void SelectSlot4(InputAction.CallbackContext context) => SelectSlot(3);
    public void SelectSlot5(InputAction.CallbackContext context) => SelectSlot(4);
    public void SelectSlot6(InputAction.CallbackContext context) => SelectSlot(5);
    public void SelectSlot7(InputAction.CallbackContext context) => SelectSlot(6);
    public void SelectSlot8(InputAction.CallbackContext context) => SelectSlot(7);
    public void SelectSlot9(InputAction.CallbackContext context) => SelectSlot(8);

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (scrollValue.y > 0) SelectSlot(currentIndex - 1);
        else if (scrollValue.y < 0) SelectSlot(currentIndex + 1);
    }
}