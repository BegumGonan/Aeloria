using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DailySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject prefab;
        public int amount = 5;
    }

    public List<SpawnableItem> itemsToSpawn;
    public TileManager tileManager;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("Spawn Validation")]
    public float paddingRadius = 1.0f; 
    public LayerMask obstacleLayer; 

    private List<GameObject> spawnedItems = new List<GameObject>();

    public void StartNewDay()
    {
        foreach (GameObject obj in spawnedItems)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedItems.Clear();

        foreach (SpawnableItem item in itemsToSpawn)
        {
            for (int i = 0; i < item.amount; i++)
            {
                Vector3 spawnPos = GetValidSpawnPosition();
                if (spawnPos != Vector3.zero)
                {
                    GameObject newObj = Instantiate(item.prefab, spawnPos, Quaternion.identity);
                    spawnedItems.Add(newObj);
                }
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        if (tileManager == null) return Vector3.zero;

        int attempts = 0;
        while (attempts < 100)
        {
            attempts++;

            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 worldPos = new Vector3(x, y, 0);

            Vector3Int cellPos = tileManager.interactableMap.WorldToCell(worldPos);

            TileBase interactableTile = tileManager.interactableMap.GetTile(cellPos);
            TileBase waterTile = tileManager.waterMap.GetTile(cellPos);

            if (interactableTile == null && waterTile == null)
            {
                Vector3 cellCenterWorld = tileManager.interactableMap.GetCellCenterWorld(cellPos);

                Collider2D obstacleHit = Physics2D.OverlapCircle(cellCenterWorld, paddingRadius, obstacleLayer);

                if (obstacleHit == null)
                {
                    return cellCenterWorld;
                }
            }
        }
        return Vector3.zero;
    }
}