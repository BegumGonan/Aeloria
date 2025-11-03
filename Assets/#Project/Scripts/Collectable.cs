using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool isInRange = false;
    private PlayerBehavior player;
    public CollectableType type;
    public Sprite icon;
    public Rigidbody2D rb2d;
    private bool canBeCollected = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke(nameof(EnableCollection), 0.5f);
    }

    private void EnableCollection()
    {
        canBeCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior playerBehavior = collision.GetComponent<PlayerBehavior>();
        if (playerBehavior != null)
        {
            player = playerBehavior;
            isInRange = true;
            player.SetCurrentCollectable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerBehavior playerBehavior = collision.GetComponent<PlayerBehavior>();
        if (playerBehavior != null && playerBehavior == player)
        {
            isInRange = false;
            player.SetCurrentCollectable(null);
            player = null;
        }
    }

    public void TryCollect()
    {
        if (isInRange && player != null)
        {
            player.inventory.Add(this);
            Destroy(gameObject);
        }
    }
}


