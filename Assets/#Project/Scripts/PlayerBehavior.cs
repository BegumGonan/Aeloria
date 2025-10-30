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

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var moveAction = playerInput.actions["Move"];
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }

    private void OnDestroy()
    {
        var moveAction = playerInput.actions["Move"];
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
    }

    private void Update()
    {
        movement = new Vector3(moveInput.x, moveInput.y, 0f);
        AnimateMovement(movement);
    }

    private void FixedUpdate()
    {
        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
}


