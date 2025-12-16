using UnityEngine;
using UnityEngine.UI;

public class PotionButtonUI : MonoBehaviour
{
    [Header("Potion Data")]
    public PotionData assignedPotion;
    private CauldronUIManager cauldronUI;

    public void Setup(CauldronUIManager manager)
    {
        cauldronUI = manager;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            cauldronUI.TryStartPotion(assignedPotion);
        });
    }
}