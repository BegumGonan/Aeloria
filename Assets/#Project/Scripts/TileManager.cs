using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap wateredMap;
    [SerializeField] private TileBase hiddenInteractableMap;
    [SerializeField] private TileBase plowedTile;
    [SerializeField] private TileBase wateredTile;

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

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public void SetWatered(Vector3Int position)
    {
        TileBase currentTile = interactableMap.GetTile(position);

        if (currentTile == plowedTile)
        {
            wateredMap.SetTile(position, wateredTile);
        }
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }
        return "";
    }
}