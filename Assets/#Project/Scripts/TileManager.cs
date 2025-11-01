using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private TileBase hiddenInteractableMap;
    [SerializeField] private TileBase interactedTile;

    void Start()
{
    foreach (var position in interactableMap.cellBounds.allPositionsWithin)
    {
        var tile = interactableMap.GetTile(position);
        if (tile != null)
        {
            interactableMap.SetTile(position, hiddenInteractableMap);
        }
    }
}

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null && tile.name == "Interactable")
        {
            return true;
        }

        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }
}