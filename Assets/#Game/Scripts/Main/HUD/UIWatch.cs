using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIWatch : MonoBehaviour
{
    TextMeshPro text = null;


    void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }

    void OnEnable()
    {
        EventManager.OnChangeTime += OnChangeTime;
        EventManager.OnGameFinish += OnGameFinish;
    }

    void OnDisable()
    {
        EventManager.OnChangeTime -= OnChangeTime;
        EventManager.OnGameFinish -= OnGameFinish;
    }

    void Animate()
    {
        text.rectTransform.DOPunchScale(new Vector3(0.5f, 0.5f), 0.5f).OnComplete(() => text.rectTransform.localScale = Vector3.one);
    }

    public void Reset(float currentTime)
    {
        OnChangeTime(currentTime);
        text.color = Color.white;
    }

    void OnChangeTime(float currentTime)
    {
        text.text = ConvertSpecifiedFormat(currentTime);
        ChangeColorText(currentTime);
    }

    void OnGameFinish(bool isClear)
    {
        if (isClear)
            text.text = "000:00";
    }

    void ChangeColorText(float currentTime)
    {
        int second = Mathf.FloorToInt(currentTime);
        if (second < 30)
        {
            text.color = Color.yellow;
        }
        if (second < 10)
        {
            text.color = Color.red;
        }
    }

    string ConvertSpecifiedFormat(float currentTime)
    {
        float few = currentTime % 1f;
        int second = Mathf.FloorToInt(currentTime);

        return $"{second:000}:{few.ToString("f2").Replace("0.", "")}";
    }

}
