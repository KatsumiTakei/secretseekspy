using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDLL;
using DG.Tweening;
using System;
using System.Linq;
using STLExtensiton;

public interface IInitializable
{
    void Initialize();
}

[DisallowMultipleComponent]
public class PlayScene : SceneBase
    , IInitializable
{
    [SerializeField]
    SpriteRenderer darkness = null;

    [SerializeField]
    UIClearMessage clearMessage = null;
    [SerializeField]
    UIGameoverMessage gameoverMessage = null;

    Pausable pausable = null;
    Goal[] goals = null;

    int preTime = (int)ShortTimer.InitTimeLimitSecond;

    public void Initialize()
    {
        preTime = (int)ShortTimer.InitTimeLimitSecond;
        EventManager.BroadcastNotAnimChangeScore(0);
        RoomManager.Instance.Initialize();
        darkness.SetAlpha(1f);
        darkness.DOColor(new Color(0, 0, 0, 0.85f), ShortTimer.InitTimeLimitSecond);

        int activeGoal = UnityEngine.Random.Range(0, 2);
        goals.ToList().Indexed().ForEach(goal =>
        {
            goal.Element.gameObject.SetActive(activeGoal % 2 == goal.Index % 2);
        });

    }

    private void OnEnable()
    {
        print("OnEnable PlayScene");
        EventManager.OnGameFinish += OnGameFinish;
        EventManager.OnChangeTime += OnChangeTime;

    }

    private void OnDisable()
    {
        print("OnDisable PlayScene");
        EventManager.OnGameFinish -= OnGameFinish;
        EventManager.OnChangeTime -= OnChangeTime;
    }

    void Start()
    {
        darkness.gameObject.SetActive(true);
        if (goals == null)
            goals = GetComponentsInChildren<Goal>();
        Initialize();


        pausable = new Pausable(this, StartCoroutine, StopCoroutine);
    }

    public override void Open()
    {
        AudioManager.FadeOutBGM();
        gameObject.SetActive(true);
        if (goals == null)
            goals = GetComponentsInChildren<Goal>();

        Initialize();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    void OnGameFinish(bool isClear)
    {
        if (isClear)
        {
            pausable.Pause();
            clearMessage.Anim(animComplete);

            void animComplete()
            {
                pausable.Resume();
                EventManager.BroadcastSceneToResult();
            }

        }
        else
        {
            pausable.Pause();
            darkness.DOKill();
            darkness.color = Color.black;
            gameoverMessage.Anim(pausable.Resume);
        }
    }

    void OnChangeTime(float currentTime)
    {
        int ceilTime = Mathf.CeilToInt(currentTime);
        if (ceilTime != preTime)
        {
            AudioManager.PlaySE("Tick");
            preTime = ceilTime;
        }
    }

}

