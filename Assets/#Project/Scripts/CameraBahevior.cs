using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [Header("Outside Limits")]
    [SerializeField] private Vector2 minLimits;
    [SerializeField] private Vector2 maxLimits;

    private Vector2 defaultMinLimits;
    private Vector2 defaultMaxLimits;

    private Camera cam;

    private void Awake()
    {
        defaultMinLimits = minLimits;
        defaultMaxLimits = maxLimits;

        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

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

    public Vector2 GetCameraExtents()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
            if (cam == null) return Vector2.zero;
        }

        if (cam.orthographic)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            return new Vector2(halfWidth, halfHeight);
        }
        else
        {
            float halfHeight = Mathf.Abs((transform.position.z - 0f)) * Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView * 0.5f);
            float halfWidth = halfHeight * cam.aspect;
            return new Vector2(halfWidth, halfHeight);
        }
    }
}