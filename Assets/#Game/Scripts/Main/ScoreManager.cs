using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
    ,IInitializable
{
    int score = 0;

    ScoreCounterView view = null;
    public const int LimitScore = 999999999;

    public void Initialize()
    {
        score = 0;
        view.AnimChangeScore(0, score);
    }

    void Start()
    {
        view = new ScoreCounterView(GetComponent<TMPro.TextMeshPro>(), 0.5f, 10f, LimitScore, false);
    }

    private void OnEnable()
    {
        EventManager.OnSceneToResult += OnSceneToResult;
        EventManager.OnAddScore += OnAddScore;
        EventManager.OnNotAnimChangeScore += OnNotAnimChangeScore;
    }

    private void OnDisable()
    {
        EventManager.OnSceneToResult -= OnSceneToResult;
        EventManager.OnAddScore -= OnAddScore;
        EventManager.OnNotAnimChangeScore -= OnNotAnimChangeScore;
    }

    void OnSceneToResult()
    {
        int sendScore = score;
        foreach (var initializable in gameObject.FindObjectsOfInterface<IInitializable>())
        {
            initializable.Initialize();
        }

        ProgressManager.Instance.MoveScene(eSceneState.Result, null, fadeOutComplete);

        void fadeOutComplete()
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(sendScore);
            AudioManager.PlaySE("Result", 0.1f);
        }
    }

    void OnAddScore(int addScore)
    {
        view?.AnimChangeScore(addScore, score);
        score += addScore;
    }

    void OnNotAnimChangeScore(int score)
    {
        this.score = score;
        view?.AnimChangeScore(0, score);
    }

}