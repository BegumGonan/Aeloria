using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class HousesManager : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool isInsideDoor = false;

    [Header("Position")]
    [SerializeField] private Transform insidePosition;
    [SerializeField] private Transform outsidePosition;

    [Header("Fade Panel")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Camera")]
    [SerializeField] private CameraBehavior cameraBehavior;

    [Header("Inside Bounds")]
    [SerializeField] private BoxCollider2D insideBoundsCollider;

    [Header("Inside Camera Padding")]
    [SerializeField] private float paddingLeft = 0.5f;
    [SerializeField] private float paddingRight = 0.5f;
    [SerializeField] private float paddingTop = 0.5f;
    [SerializeField] private float paddingBottom = 0.5f;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private bool playerNear = false;

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteract;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerNear)
            StartCoroutine(ToggleHouse());
    }

    private IEnumerator ToggleHouse()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        GameObject player = GameObject.FindWithTag("Player");

        if (!isInsideDoor)
        {
            player.transform.position = insidePosition.position;

            if (insideBoundsCollider != null && cameraBehavior != null)
            {
                Bounds b = insideBoundsCollider.bounds;

                Vector2 extents = cameraBehavior.GetCameraExtents();
                float halfW = extents.x;
                float halfH = extents.y;

                float minX = b.min.x + paddingLeft + halfW;
                float maxX = b.max.x - paddingRight - halfW;

                float minY = b.min.y + paddingBottom + halfH;
                float maxY = b.max.y - paddingTop - halfH;

                if (minX > maxX)
                {
                    float centerX = (b.min.x + b.max.x) * 0.5f;
                    minX = maxX = centerX;
                }

                if (minY > maxY)
                {
                    float centerY = (b.min.y + b.max.y) * 0.5f;
                    minY = maxY = centerY;
                }

                Vector2 newMin = new Vector2(minX, minY);
                Vector2 newMax = new Vector2(maxX, maxY);

                cameraBehavior.SetLimits(newMin, newMax);
            }
        }
        else
        {
            player.transform.position = outsidePosition.position;

            if (cameraBehavior != null)
                cameraBehavior.ResetLimitsToDefault();
        }

        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, timer / fadeDuration);
            if (fadeImage != null)
                fadeImage.color = new Color(0f, 0f, 0f, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        if (fadeImage != null)
            fadeImage.color = new Color(0f, 0f, 0f, to);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}