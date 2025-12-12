using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap interactableMap;
    public TileBase hiddenInteractableMap;
    public Tilemap waterMap;
    [SerializeField] private Tilemap wateredMap;
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

    public bool IsSoilWatered(Vector3Int position)
    {
        if (wateredMap == null) return false;

        return wateredMap.GetTile(position) != null;
    }

    public void DryAllSoil()
    {
        foreach (var pos in wateredMap.cellBounds.allPositionsWithin)
        {
            if (wateredMap.GetTile(pos) != null)
            {
                wateredMap.SetTile(pos, null);
            }
        }
    }
}