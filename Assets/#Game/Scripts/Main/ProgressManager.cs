using System.Collections;
using UnityDLL;
using UnityEngine;
using TMPro;
using System;

public enum eSceneState
{
    Title,
    Play,
    Result,
}

public abstract class SceneBase : MonoBehaviour
{
    public SceneBase()
    {
    }

    public abstract void Open();
    public abstract void Close();
}


[DisallowMultipleComponent]
public class ProgressManager : SingletonMonoBehaviour<ProgressManager>
{
    [SerializeField]
    TextMeshPro textMesh = null;

    [SerializeField]
    FadeAnim fadeAnim = null;

    [SerializeField]
    TitleScene titleScene = null;

    [SerializeField]
    PlayScene playScene = null;

    [SerializeField]
    ResultScene resultScene = null;

    SceneBase currentScene = null;

    void MoveSceneImpl(eSceneState sceneState)
    {
        currentScene.Close();

        switch (sceneState)
        {
            case eSceneState.Title:
                currentScene = titleScene;
                break;
            case eSceneState.Play:
                currentScene = playScene;
                break;
            case eSceneState.Result:
                currentScene = resultScene;
                break;
        }

        currentScene.Open();
    }

    void Start()
    {
        currentScene = titleScene;
        titleScene.gameObject.SetActive(true);
        playScene.gameObject.SetActive(false);
        resultScene.gameObject.SetActive(false);
    }

    public void MoveScene(eSceneState sceneState, Action fadeInComplete, Action fadeOutComplete)
    {
        //Fade用カメラをSceneのカメラの座標に合わせる
        var refCamera = currentScene.GetComponentInChildren<Camera>();
        fadeAnim.transform.parent.position = refCamera.transform.position;
        fadeAnim.FadeInOut(fadeInCompleteImpl, fadeOutComplete);

        void fadeInCompleteImpl()
        {
            MoveSceneImpl(sceneState);
            fadeInComplete?.Invoke();
        }

    }
}
