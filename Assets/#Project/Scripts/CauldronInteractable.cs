using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CauldronInteractable : MonoBehaviour
{
    public CauldronManager cauldronManager;
    public CauldronUIManager uiManager;

    [Header("Interaction Settings")]
    public float interactRange = 1.5f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Interact(PlayerBehavior player)
    {
        if (Vector3.Distance(player.transform.position, transform.position) > interactRange)
            return;

        uiManager.Open(cauldronManager, player.inventory);
        player.SetInventoryState(true);
    }
}