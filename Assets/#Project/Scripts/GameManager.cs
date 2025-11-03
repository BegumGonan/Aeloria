using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TileManager tileManager;
    public ItemManager itemManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (tileManager == null) 
            tileManager = GetComponent<TileManager>();

        itemManager = GetComponent<ItemManager>();
    }
}
