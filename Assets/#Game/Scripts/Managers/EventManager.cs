using System;
using UnityEngine;

public static class EventManager
{
    public static event Action<eInputType> OnMultipleInput;
    public static void BroadcastMultipleInput(eInputType argInput)
    {
        OnMultipleInput?.Invoke(argInput);
    }

    public static event Action OnApplicationQuit;
    public static void BroadcastApplicationQuit()
    {
        OnApplicationQuit?.Invoke();
        foreach (Action d in OnApplicationQuit.GetInvocationList())
            OnApplicationQuit -= d;
    }

    public static Action<float> OnChangeTime = null;
    public static void BroadcastChangeTime(float currentTime) => OnChangeTime?.Invoke(currentTime);


    public static Action OnSceneToResult = null;
    public static void BroadcastSceneToResult() => OnSceneToResult?.Invoke();
    
    public static Action<bool> OnGameFinish = null;
    public static void BroadcastGameFinish(bool isClear) => OnGameFinish?.Invoke(isClear);


    public static Action<int> OnAddScore = null;
    public static void BroadcastAddScore(int addScore) => OnAddScore?.Invoke(addScore);


    public static Action<int> OnNotAnimChangeScore = null;
    public static void BroadcastNotAnimChangeScore(int score) => OnNotAnimChangeScore?.Invoke(score);
    

    public static Action<int> OnShot = null;
    public static void BroadcastShot(int addedBulletIndex) => OnShot?.Invoke(addedBulletIndex);

    
    public static Action<int> OnGetBullet = null;
    public static void BroadcastGetBullet(int currentBulletIndex) => OnGetBullet?.Invoke(currentBulletIndex);
    

    public static Action<GameObject, GameObject> OnFound = null;
    public static void BroadcastFound(GameObject watchingObject, GameObject foundObject) => OnFound?.Invoke(watchingObject, foundObject);

    public static Action<bool> OnTargetingObj = null;
    public static void BroadcastTargetingObj(bool isTargeting) => OnTargetingObj?.Invoke(isTargeting);


}
