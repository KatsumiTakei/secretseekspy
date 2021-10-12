using System;
using UnityEngine;
using UnityDLL;

public class ShortTimer : MonoBehaviour
{
    public const float InitTimeLimitSecond = 180f;
    TimeKeeper timeKeeper = new TimeKeeper(InitTimeLimitSecond);

    void OnEnable()
    {
        EventManager.OnGameFinish += OnGameFinish;
        Restart();
    }

    void OnDisable()
    {
        EventManager.OnGameFinish -= OnGameFinish;
        timeKeeper.Dispose();
    }

    void Update()
    {
        timeKeeper.ManualUpdate();
    }

    void OnGameFinish(bool isClear)
    {
        if (isClear)
        {
            timeKeeper.StopTimer();
        }
        else
        {
            Resume();
        }
    }

    void Restart()
    {
        timeKeeper.RestartTimer();
    }

    void Resume()
    {
        timeKeeper.ResumeTimer();
    }


}

public class TimeKeeper : IDisposable
{
    bool isRunning = false;
    float timeLimitSecond = 0f;

    public TimeKeeper(float initTimeLimitSecond)
    {
        isRunning = false;
        timeLimitSecond = initTimeLimitSecond;
    }

    public void Dispose()
    {
    }

    public void StopTimer()
    {
        ProcessTimer.Stop();
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
        ProcessTimer.Start();
    }

    public void RestartTimer()
    {
        isRunning = true;
        ProcessTimer.Restart();
    }

    public void ManualUpdate()
    {
        if (!isRunning)
            return;

        float currentTime = timeLimitSecond - ProcessTimer.TotalSeconds;
        EventManager.BroadcastChangeTime(currentTime);

        if (currentTime <= 0f)
        {
            EventManager.BroadcastGameFinish(true);
        }
    }

}