using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDLL;
using DG.Tweening;

public class UIGameoverMessage : MonoBehaviour
    ,IInitializable
{
    [SerializeField]
    FadeAnim fadeAnim = null;

    SpriteRenderer spriteRenderer = null;
    SpriteMask mask = null;

    public void Initialize()
    {
        spriteRenderer.enabled = false;
        mask.enabled = false;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();

        spriteRenderer.enabled = false;
        mask.enabled = false;

    }

    public void Anim(Action animComplete)
    {
        spriteRenderer.enabled = true;
        mask.enabled = true;

        fadeAnim.transform.parent.transform.position = Camera.main.transform.position;

        DOVirtual.DelayedCall(1.0f,() => fadeAnim.FadeInOut(complete, null));
        
        void complete()
        {
            foreach (var initializable in gameObject.FindObjectsOfInterface<IInitializable>())
            {
                initializable.Initialize();
            }

            animComplete?.Invoke();
        }
    }

}
