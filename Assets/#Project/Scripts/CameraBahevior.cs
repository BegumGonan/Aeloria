using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 1f;
    [SerializeField] private Vector3 offset;


    [SerializeField] private Vector2 minLimits;
    [SerializeField] private Vector2 maxLimits;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minLimits.x, maxLimits.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minLimits.y, maxLimits.y);

        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
    }
}
