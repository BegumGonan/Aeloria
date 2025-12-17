using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    public void Collect(PlayerBehavior player)
    {
        if (gameObject.activeSelf && item != null && item.CanBeCollected)
        {
            player.inventory.Add("Backpack", item);
            gameObject.SetActive(false); 
            Destroy(gameObject, 0.1f);
        }
    }
}