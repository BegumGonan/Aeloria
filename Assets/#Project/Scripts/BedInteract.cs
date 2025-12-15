using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class BedInteract : MonoBehaviour
{
    [Header("Fade Panel")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private bool playerNear = false;
    private PlayerEnergy playerEnergy; 

    private void Start() 
    {
        PlayerBehavior player = Object.FindFirstObjectByType<PlayerBehavior>();
        if (player != null)
        {
            playerEnergy = player.GetComponent<PlayerEnergy>();
        }
    }

    private void OnEnable()
    {
        interactAction.action.performed += OnInteract;
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerNear)
            StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        playerEnergy?.ResetEnergy(); 
        
        GameManager.instance.timeManager.StartNewDay();

        GameManager.instance.tileManager.DryAllSoil();

        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float a = Mathf.Lerp(from, to, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            timer += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, to);
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