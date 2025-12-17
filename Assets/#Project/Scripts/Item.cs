using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    public ItemData data;
    [HideInInspector] public Rigidbody2D rb2d;

    public bool CanBeCollected { get; private set; } = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(EnableCollectionRoutine());
    }

    private IEnumerator EnableCollectionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        CanBeCollected = true;
    }
}