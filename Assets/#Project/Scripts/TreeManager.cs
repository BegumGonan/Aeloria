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

    [Header("Colliders")]
    [SerializeField] private Collider2D treeCollider;
    [SerializeField] private Collider2D stumpCollider;
    [SerializeField] private Collider2D interactionCollider;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (treeCollider != null) treeCollider.enabled = true;
        if (stumpCollider != null) stumpCollider.enabled = false;
        if (interactionCollider != null) interactionCollider.enabled = true;
    }

    public void HitTree(PlayerBehavior player)
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

        if (treeCollider != null) treeCollider.enabled = false;
        if (stumpCollider != null) stumpCollider.enabled = true;

        DropWood(woodFromTree);

        isCut = true;
        cutDay = GameManager.instance.timeManager.timeStamp.day;
    }

    private void RemoveStump()
    {
        DropWood(woodFromStump);

        if (sr != null)
        {
            sr.enabled = false;
        }

        if (treeCollider != null) treeCollider.enabled = false;
        if (stumpCollider != null) stumpCollider.enabled = false;
        if (interactionCollider != null) interactionCollider.enabled = false;
    }

    private void DropWood(int amount)
    {
        if (woodPrefab == null) return;

        Collider2D col = treeCollider;
        float minDistance = 0.5f;

        for (int i = 0; i < amount; i++)
        {
            Vector2 offset = Vector2.zero;

            if (col != null)
            {
                int tries = 0;
                bool valid = false;

                while (!valid && tries < 20)
                {
                    Vector2 randomDir = Random.insideUnitCircle.normalized;
                    float distance = col.bounds.size.x / 2f + Random.Range(minDistance, minDistance + 1f);
                    offset = randomDir * distance;

                    if (!col.OverlapPoint((Vector2)transform.position + offset))
                        valid = true;

                    tries++;
                }

                if (!valid)
                    offset = new Vector2(col.bounds.extents.x + minDistance, 0);
            }
            else
            {
                offset = Random.insideUnitCircle * 1f;
            }

            GameObject wood = Instantiate(woodPrefab, (Vector2)transform.position + offset, Quaternion.identity);

            Rigidbody2D rb = wood.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.AddForce(offset * 2f, ForceMode2D.Impulse);
        }
    }

    public void Respawn()
    {
        if (sr != null)
        {
            sr.enabled = true; 
        }

        isStump = false;
        isCut = false;

        treeHealth = 5;
        stumpHealth = 3;

        sr.sprite = fullTreeSprite;

        if (treeCollider != null) treeCollider.enabled = true;
        if (stumpCollider != null) stumpCollider.enabled = false;
        if (interactionCollider != null) interactionCollider.enabled = true;
    }

    public void CheckRespawn()
    {
        if (isCut && GameManager.instance.timeManager.timeStamp.day >= cutDay + 2)
        {
            Respawn();
        }
    }
}