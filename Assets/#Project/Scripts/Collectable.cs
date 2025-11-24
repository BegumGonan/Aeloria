using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private bool isInRange = false;
    private PlayerBehavior player;
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
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
        if (isInRange && player != null && item != null)
        {
            player.inventory.Add("Backpack", item);
            Destroy(gameObject);
        }
    }
}