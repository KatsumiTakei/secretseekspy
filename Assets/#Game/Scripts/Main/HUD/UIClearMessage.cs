using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIClearMessage : MonoBehaviour
{
    TextMeshPro text = null;

    public void Anim(Action complete)
    {
        text.rectTransform.DOScaleY(20f, 1f)
            .OnComplete(() => text.rectTransform.DOScaleY(0f, 1f).OnComplete(() => complete?.Invoke()));
        //AudioManager.PlaySE("");
    }

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        text.rectTransform.localScale = new Vector3(20f, 0f);
    }

}
