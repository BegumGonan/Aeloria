using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 1.5f;

    [Header("Potion Use UI")]
    public PotionUseUI potionUseUI;

    private Vector2 moveInput;
    private Vector3 movement;
    private PlayerInput playerInput;
    private Collectable currentCollectable;
    public InventoryManager inventory;
    private TileManager tileManager;
    private bool isInventoryOpen = false;
    private PlayerEnergy playerEnergy;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var moveAction = playerInput.actions["Move"];
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        var interactAction = playerInput.actions["Interact"];
        interactAction.performed += OnInteract;

        var rightClickAction = playerInput.actions["RightClick"];
        rightClickAction.performed += OnRightClick;

        inventory = GetComponent<InventoryManager>();
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    private void OnDestroy()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
        playerInput.actions["RightClick"].performed -= OnRightClick;
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
        animator = GetComponentInChildren<Animator>();

        inventory.Add("Toolbar", GameManager.instance.itemManager.GetItemByName("Hoe"));
        inventory.Add("Toolbar", GameManager.instance.itemManager.GetItemByName("WateringCan"));
        inventory.Add("Toolbar", GameManager.instance.itemManager.GetItemByName("Axe"));

        Item valerianSeed = GameManager.instance.itemManager.GetItemByName("Valerian Seed");
        if (valerianSeed != null)
            for (int i = 0; i < 10; i++) inventory.Add("Backpack", valerianSeed);

        Item mushroomSeed = GameManager.instance.itemManager.GetItemByName("White Mushroom Seed");
        if (mushroomSeed != null)
            for (int i = 0; i < 5; i++) inventory.Add("Backpack", mushroomSeed);

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
        if (!playerEnergy.HasEnergy) return;

        if (currentCollectable != null)
        {
            currentCollectable.TryCollect();
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (var hit in hits)
        {
            CauldronInteractable cauldron = hit.GetComponent<CauldronInteractable>();
            if (cauldron != null)
            {
                cauldron.Interact(this);
                return;
            }
        }

        if (inventory.toolbar.selectedSlot == null) return;
        string selectedItem = inventory.toolbar.selectedSlot.itemName;

        if (selectedItem == "Axe")
        {
            foreach (var hit in hits)
            {
                TreeManager tree = hit.GetComponentInParent<TreeManager>();
                if (tree != null)
                {
                    tree.HitTree(this);
                    animator.SetTrigger("isChopping");
                    playerEnergy.ConsumeEnergy();
                    return;
                }
            }
        }

        Vector3Int position = new Vector3Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.y),
            0
        );

        if (tileManager == null) return;

        if (tileManager.IsCropReadyToHarvest(position))
        {
            TryHarvest(position);
            return;
        }

        string tileName = tileManager.GetTileName(position);
        if (string.IsNullOrWhiteSpace(tileName)) return;

        if (tileName == "soil" && tileManager.GetCropTile(position) == null)
        {
            Item itemObj = GameManager.instance.itemManager.GetItemByName(selectedItem);
            if (itemObj != null && itemObj.data != null && itemObj.data.cropData != null)
            {
                if (inventory.toolbar.TryRemoveSingleItem(selectedItem))
                {
                    tileManager.PlantCrop(position, itemObj.data.cropData);
                    GameManager.instance.uiManager.RefreshAll();
                    return;
                }
            }
        }

        if (tileName == "Interactable" && selectedItem == "Hoe")
        {
            tileManager.SetInteracted(position);
            animator.SetTrigger("isPlowing");
            playerEnergy.ConsumeEnergy();
        }
        else if (tileName == "soil" && selectedItem == "WateringCan")
        {
            if (tileManager.IsSoilWatered(position)) return;

            tileManager.SetWatered(position);
            animator.SetTrigger("isWatering");
            playerEnergy.ConsumeEnergy();
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

    private void TryHarvest(Vector3Int position)
    {
        CropTile cropTile = tileManager.GetCropTile(position);
        if (cropTile != null && cropTile.cropData != null && cropTile.cropData.grownItemPrefab != null)
        {
            inventory.Add("Backpack", cropTile.cropData.grownItemPrefab);
            tileManager.ClearCrop(position);
            GameManager.instance.uiManager.RefreshAll();
        }
    }

    public void SetCurrentCollectable(Collectable collectable)
    {
        currentCollectable = collectable;
    }

    public void DropItemFromInventory(string itemName)
    {
        Item itemPrefab = GameManager.instance.itemManager.GetItemByName(itemName);
        DropSingleItem(itemPrefab); 
    }

    private void DropSingleItem(Item itemPrefab)
    {
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f; 

        Item droppedItem = Instantiate(
            itemPrefab.gameObject,
            spawnLocation + spawnOffset,
            Quaternion.identity
        ).GetComponent<Item>();

        droppedItem.gameObject.SetActive(true);

        if (droppedItem.rb2d != null)
        {
            float dropForceMultiplier = 0.5f;
            droppedItem.rb2d.linearDamping = 5.0f;     
            droppedItem.rb2d.angularDamping = 1.0f; 

            droppedItem.rb2d.AddForce(spawnOffset * dropForceMultiplier, ForceMode2D.Impulse);
        }
    }
    
    public void DropItem(Item item)
    {
        DropSingleItem(item);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropSingleItem(item);
        }
    }

    public void SetInventoryState(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var selectedSlot = inventory.toolbar.selectedSlot;
        if (selectedSlot != null && selectedSlot.itemName == "Energy Potion")
        {
            potionUseUI.Open(playerEnergy, inventory, "Energy Potion");
        }
    }
}