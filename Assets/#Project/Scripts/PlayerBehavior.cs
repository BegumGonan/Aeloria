using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    private Vector2 moveInput;
    private Vector3 movement;
    private PlayerInput playerInput;
    private Collectable currentCollectable;
    public InventoryManager inventory;
    private TileManager tileManager;
    private bool isInventoryOpen = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var moveAction = playerInput.actions["Move"];
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        var interactAction = playerInput.actions["Interact"];
        interactAction.performed += OnInteract;

        inventory = GetComponent<InventoryManager>();
    }

    private void OnDestroy()
    {
        var moveAction = playerInput.actions["Move"];
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        var interactAction = playerInput.actions["Interact"];
        interactAction.performed -= OnInteract;
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
        animator = GetComponentInChildren<Animator>();

        Item hoeItem = GameManager.instance.itemManager.GetItemByName("Hoe");
        inventory.Add("Toolbar", hoeItem);

        Item wateringCanItem = GameManager.instance.itemManager.GetItemByName("WateringCan");
        inventory.Add("Toolbar", wateringCanItem);

        GameManager.instance.uiManager.RefreshAll();
    }

    private void Update()
    {
        if (isInventoryOpen)
        {
            movement = Vector3.zero;
            AnimateMovement(Vector3.zero);
            return;
        }
        movement = new Vector3(moveInput.x, moveInput.y, 0f);
        AnimateMovement(movement);
    }
   
    private void FixedUpdate()
    {
        if (isInventoryOpen) return;
        transform.position += movement * speed * Time.deltaTime;
    }
   
    private void OnMove(InputAction.CallbackContext context)
    {
        if (isInventoryOpen)
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = context.ReadValue<Vector2>();
    }
   
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isInventoryOpen) return;

        Vector3Int position = new Vector3Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.y),
            0
        );

        if (currentCollectable != null)
        {
            currentCollectable.TryCollect();
            return;
        }

        if (tileManager == null) return;

        string tileName = tileManager.GetTileName(position);

        if (string.IsNullOrWhiteSpace(tileName)) return;

        if (tileName == "Interactable" && inventory.toolbar.selectedSlot.itemName == "Hoe")
        {
            tileManager.SetInteracted(position);
            animator.SetTrigger("isPlowing");
        }
        else if (tileName == "soil" && inventory.toolbar.selectedSlot.itemName == "WateringCan")
        {
            if (tileManager.IsSoilWatered(position)) return;

            tileManager.SetWatered(position);
            animator.SetTrigger("isWatering");
        }
    }

    private void AnimateMovement(Vector3 direction)
    {
        if (animator == null) return;
        bool isMoving = direction.magnitude > 0;
        animator.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }
    }

    public void SetCurrentCollectable(Collectable collactable)
    {
        currentCollectable = collactable;
    }

    public void DropItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("DropItem: item is null!");
            return;
        }

        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;

        Item droppedItem = Instantiate(item.gameObject, spawnLocation + spawnOffset, Quaternion.identity).GetComponent<Item>();
        droppedItem.gameObject.SetActive(true); 
        if (droppedItem.rb2d != null)
            droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }

    public void SetInventoryState(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }
}