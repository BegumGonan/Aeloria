using UnityEngine;

public class CauldronInteractable : MonoBehaviour
{
    public CauldronManager cauldronManager;
    public CauldronUIManager uiManager;

    private void OnMouseDown()
    {
        PlayerBehavior player = FindFirstObjectByType<PlayerBehavior>();
        if (player == null) return;

        uiManager.Open(cauldronManager);
    }
}