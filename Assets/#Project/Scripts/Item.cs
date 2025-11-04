using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    public ItemData data;
    [HideInInspector] public Rigidbody2D rb2d;
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
}
