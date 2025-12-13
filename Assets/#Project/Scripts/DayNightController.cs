using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour
{
    public TimeManager timeManager;
    public Light2D globalLight;

    void Update()
    {
        GameTimeStamp t = timeManager.GetTime();
        float hour = t.hour + t.minute / 60f;

        globalLight.intensity = GetIntensity(hour);
    }

    float GetIntensity(float hour)
    {
        if (hour >= 6f && hour < 10f)
            return Mathf.Lerp(0.4f, 1f, Mathf.InverseLerp(6f, 10f, hour));

        if (hour >= 10f && hour < 18f)
            return 1f;

        if (hour >= 18f && hour < 24f)
            return Mathf.Lerp(1f, 0.2f, Mathf.InverseLerp(18f, 24f, hour));

        return Mathf.Lerp(0.2f, 0.4f, Mathf.InverseLerp(0f, 6f, hour));
    }
}