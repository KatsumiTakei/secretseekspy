using STLExtensiton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityDLL;
using UnityEngine;

[DisallowMultipleComponent]
public class TitleScene : SceneBase
{
    [SerializeField]
    ParticleSystem[] particles = null;

    [SerializeField]
    Transform animTrans = null;

    Vector3 initPos = Vector3.zero;
    Waypoint waypoint = null;

    IEnumerator CoLateStart()
    {
        yield return null;
        waypoint.SearchNearPoint(animTrans.position);

    }

    void OnEnable()
    {
        print("OnEnable TitleScene");
        EventManager.OnMultipleInput += OnMultipleInput;
    }

    void OnDisable()
    {
        print("OnDisable TitleScene");
        EventManager.OnMultipleInput -= OnMultipleInput;
    }

    void Awake()
    {
        waypoint = GetComponentInChildren<Waypoint>();
        initPos = animTrans.position;

        StartCoroutine(CoLateStart());

        foreach (var particle in particles)
        {
            particle.Pause(true);
        }


    }
    public void OnMultipleInput(eInputType inputType)
    {
        if (inputType == eInputType.ShotAndDecideKeyDown)
        {
            ProgressManager.Instance.MoveScene(eSceneState.Play, null, null);
        }
    }

    public override void Open()
    {
        AudioManager.PlayBGM("BGM");
        gameObject.SetActive(true);
        animTrans.position = initPos;
        waypoint.SearchNearPoint(animTrans.position);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
