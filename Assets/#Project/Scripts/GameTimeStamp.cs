using UnityEngine;

[System.Serializable]
public class GameTimeStamp
{
    public int day = 1;
    public string season = "Spring";

    public int hour = 6;
    public int minute = 0;

    public void AddMinutes(int amount)
    {
        minute += amount;

        while (minute >= 60)
        {
            minute -= 60;
            hour++;
        }

        if (hour >= 24)
        {
            hour = 0;
        }
    }
}