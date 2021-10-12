using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pausable : 
    IDisposable
    //, ITogglePauser
    //, ICoroutineRegister
    //, ITweenRegister
{
    #region		Variables
    MonoBehaviour rootObject = null;
    MonoBehaviour[] pausingMonoBehaviours = null;
    List<Transform> tweenObjs = new List<Transform>();
    List<IEnumerator> pausingCoroutines = new List<IEnumerator>();
    Func<IEnumerator, Coroutine> startCoroutine = null;
    Action<IEnumerator> stopCoroutine = null;

    List<GameObject> ignoreGameObjects = new List<GameObject>();

    public static bool IsPause { get; private set; } = false;

    #endregion  Variables 

    public Pausable(MonoBehaviour rootObject, Func<IEnumerator, Coroutine> startCoroutine, Action<IEnumerator> stopCoroutine)
    {
        this.rootObject = rootObject;
        this.startCoroutine = startCoroutine;
        this.stopCoroutine = stopCoroutine;
        IsPause = false;
        ignoreGameObjects.Add(rootObject.gameObject);
        //EventManager.OnTogglePause += OnTogglePause;
        //EventManager.OnRegistCoroutine += OnRegistCoroutine;
        //EventManager.OnRegistTween += OnRegistTween;

    }

    public void Dispose()
    {
        //EventManager.OnTogglePause -= OnTogglePause;
        //EventManager.OnRegistCoroutine -= OnRegistCoroutine;
        //EventManager.OnRegistTween -= OnRegistTween;
    }

    //public void OnRegistTween(Transform tweenObj)
    //{
    //    tweenObjs.Add(tweenObj);
    //}

    //public void DeleteCoroutines()
    //{
    //    //for (int i = 0, max = pausingCoroutines.Count; i < max; i++)
    //    //{
    //    //    pausingCoroutines[i] = null;
    //    //}

    //    pausingCoroutines.Clear();
    //}


    //public void OnRegistCoroutine(IEnumerator coroutine)
    //{
    //    pausingCoroutines.Add(coroutine);
    //}

    ///// <summary>
    ///// トグル
    ///// </summary>
    //public void OnTogglePause()
    //{
    //    IsPause = !IsPause;

    //    if (IsPause)
    //    {
    //        //AudioManager.Instance.PlaySE(ResourcesPath.Audio.SE._TogglePause);
    //        Pause();
    //    }
    //    else
    //    {
    //        Resume();
    //    }

    //}

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        pausingMonoBehaviours = //rootObject.GetComponentsInChildren<MonoBehaviour>();
        Array.FindAll(rootObject.GetComponentsInChildren<MonoBehaviour>(), MonoBehaviourPredicate);

        for (int Index = 0, LengthMax = pausingMonoBehaviours.Length; Index < LengthMax; Index++)
        {
            pausingMonoBehaviours[Index].enabled = false;
        }

        pausingCoroutines.ForEach(p => stopCoroutine(p));
        tweenObjs.ForEach(p => p.DOPause());

    }
    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        for (int Index = 0, LengthMax = pausingMonoBehaviours.Length; Index < LengthMax; Index++)
        {
            pausingMonoBehaviours[Index].enabled = true;
        }

        pausingCoroutines.ForEach(p => startCoroutine(p));
        tweenObjs.ForEach(p => p.DOPlay());
    }

    bool MonoBehaviourPredicate(MonoBehaviour monoBehaviour)
    {
        return (monoBehaviour.enabled &&
            (Array.FindIndex(ignoreGameObjects.ToArray(), gameObject => gameObject == monoBehaviour.gameObject) < 0));
    }

}

