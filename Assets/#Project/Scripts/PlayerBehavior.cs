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
    public Inventory inventory;
    private bool isInventoryOpen = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var moveAction = playerInput.actions["Move"];
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        var interactAction = playerInput.actions["Interact"];
        interactAction.performed += OnInteract;
        inventory = new Inventory(21);
    }
   
    private void OnDestroy()
    {
        var moveAction = playerInput.actions["Move"];
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        var interactAction = playerInput.actions["Interact"];
        interactAction.performed -= OnInteract;
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
        }
        else if (GameManager.instance != null && GameManager.instance.tileManager != null)
        {
            if (GameManager.instance.tileManager.IsInteractable(position))
            {
                GameManager.instance.tileManager.SetInteracted(position);
            }
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
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void SetInventoryState(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }
}
