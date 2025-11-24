using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TileManager tileManager;
    public ItemManager itemManager;
    public UI_Manager uiManager;

    public PlayerBehavior player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (tileManager == null) tileManager = GetComponent<TileManager>();

        itemManager = GetComponent<ItemManager>();

        if (player == null) player = FindFirstObjectByType<PlayerBehavior>();

        uiManager = GetComponent<UI_Manager>();
    }
}