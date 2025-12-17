using UnityEngine;

public class FinishedPotionPickup : MonoBehaviour
{
    public CauldronManager cauldron;

    public void Collect()
    {
        if (cauldron == null) return;

        cauldron.CollectPotion();
        Destroy(gameObject);
    }
}