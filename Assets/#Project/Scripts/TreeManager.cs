using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [Header("Tree Settings")]
    public int treeHealth = 5;
    public int stumpHealth = 3;
    public bool isStump = false;

    [Header("Sprites")]
    public Sprite fullTreeSprite;
    public Sprite stumpSprite;

    [Header("Wood Drop")]
    public GameObject woodPrefab;
    public int woodFromTree = 5;
    public int woodFromStump = 3;

    [Header("Regrowth Tracking")]
    public bool isCut = false;
    public int cutDay = -1;

    private SpriteRenderer sr;
    private PlayerBehavior player;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehavior pb = other.GetComponent<PlayerBehavior>();
        if (pb != null)
            player = pb;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerBehavior pb = other.GetComponent<PlayerBehavior>();
        if (pb != null && pb == player)
            player = null;
    }

    public void HitTree()
    {
        if (player == null) return;

        if (player.inventory.toolbar.selectedSlot == null ||
            player.inventory.toolbar.selectedSlot.itemName != "Axe")
            return;

        if (!isStump)
        {
            treeHealth--;

            if (treeHealth <= 0)
                CutTree();
        }
        else
        {
            stumpHealth--;

            if (stumpHealth <= 0)
                RemoveStump();
        }
    }

    private void CutTree()
    {
        sr.sprite = stumpSprite;
        isStump = true;

        DropWood(woodFromTree);

        isCut = true;
        cutDay = GameManager.instance.timeManager.timeStamp.day;
    }

    private void RemoveStump()
    {
        DropWood(woodFromStump);

        Destroy(gameObject);
    }

    private void DropWood(int amount)
    {
        if (woodPrefab == null) return;

        for (int i = 0; i < amount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Instantiate(woodPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }
    }

    public void Respawn()
    {
        isStump = false;
        isCut = false;

        treeHealth = 5;
        stumpHealth = 3;

        sr.sprite = fullTreeSprite;
    }
}