using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameTimeStamp timeStamp = new GameTimeStamp();

    public float timeSpeed = 0.5f;
    private float timer;

    public bool dayEnded = false;

    void Update()
    {
        if (dayEnded) return;

        timer += Time.deltaTime * timeSpeed;

        if (timer >= 1f)
        {
            timer = 0f;
            timeStamp.AddMinutes(1);

            CheckEvents();
        }
    }

    void CheckEvents()
    {
        if (timeStamp.hour == 23 && timeStamp.minute == 0)
        {
            Debug.Log("It's getting late!");
        }

        if (timeStamp.hour == 0 && timeStamp.minute == 0)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        dayEnded = true;
        Debug.Log("Day finished. Player must sleep.");
    }

    public void StartNewDay()
    {
        dayEnded = false;
        timeStamp.day++;
        timeStamp.hour = 6;
        timeStamp.minute = 0;

        DailySpawner spawner = Object.FindFirstObjectByType<DailySpawner>();
        if (spawner != null)
            spawner.StartNewDay();

        TreeManager[] allTrees = Object.FindObjectsByType<TreeManager>(FindObjectsSortMode.None);
        foreach (TreeManager tree in allTrees)
        {
            tree.CheckRespawn();
        }
        
        if (GameManager.instance.tileManager != null)
        {
            GameManager.instance.tileManager.AdvanceCropsDay();
            GameManager.instance.tileManager.DryAllSoil();
        }
    }

    public GameTimeStamp GetTime()
    {
        return timeStamp;
    }
}