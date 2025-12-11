using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [Header("Dış Dünya Limitleri")]
    [SerializeField] private Vector2 minLimits;
    [SerializeField] private Vector2 maxLimits;

    private Vector2 defaultMinLimits;
    private Vector2 defaultMaxLimits;

    private void Awake()
    {
        defaultMinLimits = minLimits;
        defaultMaxLimits = maxLimits;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float clampedX = Mathf.Clamp(target.position.x + offset.x, minLimits.x, maxLimits.x);
        float clampedY = Mathf.Clamp(target.position.y + offset.y, minLimits.y, maxLimits.y);

        transform.position = new Vector3(clampedX, clampedY, target.position.z + offset.z);
    }

    public void SetLimits(Vector2 newMin, Vector2 newMax)
    {
        minLimits = newMin;
        maxLimits = newMax;
    }

    public void ResetLimitsToDefault()
    {
        minLimits = defaultMinLimits;
        maxLimits = defaultMaxLimits;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}