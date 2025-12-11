using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class HousesManager : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Transform insidePosition;
    [SerializeField] private Transform outsidePosition;

    [Header("Fade Panel")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Kamera")]
    [SerializeField] private CameraBehavior cameraBehavior;
    [SerializeField] private Vector2 insideMinLimits;
    [SerializeField] private Vector2 insideMaxLimits;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private bool playerNear = false;
    private bool inside = false;

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

        if (!inside)
        {
            player.transform.position = insidePosition.position;

            cameraBehavior.SetLimits(insideMinLimits, insideMaxLimits);
        }
        else
        {
            player.transform.position = outsidePosition.position;

            cameraBehavior.ResetLimitsToDefault();
        }

        inside = !inside;

        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, timer / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
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