using TMPro;
using UnityEngine;

public class Time_UI : MonoBehaviour
{
    public TextMeshProUGUI daySeasonText;
    public TextMeshProUGUI timeText;

    public TimeManager timeManager;

    void Update()
    {
        if(timeManager == null) return;

        GameTimeStamp t = timeManager.GetTime();

        daySeasonText.text = $"Day {t.day} - {t.season}";
        timeText.text = $"{t.hour:D2}:{t.minute:D2}";
    }
}