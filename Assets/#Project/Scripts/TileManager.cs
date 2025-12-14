using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropTile
{
    public CropData cropData;
    public int growthDay;
    public bool isWatered;

    public CropTile(CropData data)
    {
        cropData = data;
        growthDay = 0;
        isWatered = false; 
    }
}


public class TileManager : MonoBehaviour
{
    public Tilemap interactableMap;
    public TileBase hiddenInteractableMap;
    public Tilemap waterMap;
    public Tilemap cropMap;
    [SerializeField] private Tilemap wateredMap;
    [SerializeField] private TileBase plowedTile;
    [SerializeField] private TileBase wateredTile;

    private Dictionary<Vector3Int, CropTile> cropTiles = new Dictionary<Vector3Int, CropTile>();

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

        if (currentTile == plowedTile || cropTiles.ContainsKey(position))
        {
            wateredMap.SetTile(position, wateredTile);
            
            if (cropTiles.ContainsKey(position))
            {
                cropTiles[position].isWatered = true;
            }
        }
    }
    
    public void PlantCrop(Vector3Int position, CropData cropData)
    {
        if ((interactableMap.GetTile(position) == plowedTile || interactableMap.GetTile(position) == hiddenInteractableMap)
            && !cropTiles.ContainsKey(position))
        {
            CropTile newCrop = new CropTile(cropData);
            cropTiles.Add(position, newCrop);

            if (cropData.growthStages.Length > 0)
            {
                cropMap.SetTile(position, cropData.growthStages[0]);
            }
        }
    }

    public void ClearCrop(Vector3Int position)
    {
        if (cropTiles.ContainsKey(position))
        {
            cropTiles.Remove(position);

            cropMap.SetTile(position, null);
            interactableMap.SetTile(position, plowedTile);

            if (wateredMap.GetTile(position) != null)
                wateredMap.SetTile(position, null);
        }
    }

    public void AdvanceCropsDay()
    {
        List<Vector3Int> positionsToUpdate = new List<Vector3Int>(cropTiles.Keys);

        foreach (Vector3Int position in positionsToUpdate)
        {
            if (cropTiles.TryGetValue(position, out CropTile cropTile))
            {
                if (cropTile.growthDay < cropTile.cropData.daysToGrow)
                {
                    if (cropTile.isWatered)
                    {
                        cropTile.growthDay++;
                    }

                    int stageIndex = Mathf.Min(cropTile.growthDay, cropTile.cropData.growthStages.Length - 1);
                    cropMap.SetTile(position, cropTile.cropData.growthStages[stageIndex]);
                }
                
                cropTile.isWatered = false;
            }
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
    
    public bool IsCropReadyToHarvest(Vector3Int position)
    {
        if (cropTiles.ContainsKey(position))
        {
            CropTile crop = cropTiles[position];
            return crop.growthDay >= crop.cropData.daysToGrow - 1;
        }
        return false;
    }

    public CropTile GetCropTile(Vector3Int position)
    {
        if (cropTiles.ContainsKey(position))
        {
            return cropTiles[position];
        }
        return null;
    }
}