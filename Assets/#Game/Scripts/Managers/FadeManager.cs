using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityDLL;

[DisallowMultipleComponent]
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    #region DEFINE_VARIABLE

    public const float ALPHA_MAX = 1.0f;
    public const float ALPHA_MIN = 0.0f;

    // For scene
    public List<Texture2D> texs;

    private float _fadeAlpha = 0;
    private bool _isFading = false;

    public bool isUsingTexture = false;
    public Color fadeColor;
    public const float defInterval = 0.5f;
    private Text loadingText;

    // For GameObject
    private GameObject target;
    private Color color;
    private Material mat;
    // For GameObjects
    private List<GameObject> targets = new List<GameObject>();
    private List<Material> mats = new List<Material>();
    #endregion

    public void Start()
    {
    }

    #region FOR_SCENE_FADE
    public void OnGUI()
    {
        if (_isFading)
        {

            if (isUsingTexture)
            {
                GUI.color = fadeColor;
                fadeColor.a = _fadeAlpha;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texs[0]);
            }
            else
            {
                GUI.color = fadeColor;
                fadeColor.a = _fadeAlpha;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            }
        }
    }

    public void LoadLevel(string scene, float interval = defInterval, bool useLoading = false)
    {
        if (useLoading)
            loadingText = GameObject.Find("LoadingText").GetComponent<Text>();

        StartCoroutine(TransScene(scene, interval, useLoading));
    }

    public void LoadLevel(string scene, Texture2D tex, float interval = defInterval, bool useLoading = false)
    {
        StartCoroutine(TransSceneWithTexture(scene, tex, interval, useLoading));
    }

    public void Wink(float interval)
    {
        StartCoroutine(FadeInOut(interval));
    }

    private IEnumerator TransScene(string scene, float interval, bool useLoading)
    {
        //AudioManager.Instance.FadeOutBGM(0.1f);
        // It's getting dark.
        #region [ FADEOUT ]
        this._isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this._fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
        #endregion

        if (useLoading)
        {
            yield return AsyncLoading(scene);

        }
        else
            SceneManager.LoadScene(scene);

        // It's getting lighter.
        #region [ FADEIN ]
        time = 0;
        while (time <= interval)
        {
            this._fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this._isFading = false;
        #endregion
    }
    private IEnumerator TransSceneWithTexture(string scene, Texture2D texture, float interval, bool useLoading)
    {
        #region [ FADEOUT ]
        _isFading = true;
        float time = 0;
        while (time <= interval)
        {
            _fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
        #endregion

        if (useLoading)
        {
            yield return AsyncLoading(scene);
        }
        else
            SceneManager.LoadScene(scene);

        #region [ FADEIN ]
        time = 0;
        while (time <= interval)
        {
            _fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        _isFading = false;
        #endregion
    }

    private IEnumerator FadeInOut(float interval)
    {
        #region [ FADEOUT ]
        _isFading = true;
        float time = 0;
        while (time <= interval)
        {
            _fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
        #endregion

        #region [ FADEIN ]
        time = 0;
        while (time <= interval)
        {
            this._fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this._isFading = false;
        #endregion
    }

    private IEnumerator AsyncLoading(string scene)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            //print(async.progress * 100 + "%");
            if (loadingText)
                loadingText.text = (async.progress * 100).ToString("F0") + "%";

            yield return new WaitForEndOfFrame();
        }

        if (loadingText)
            loadingText.text = "100%";

        yield return new WaitForSeconds(1);

        async.allowSceneActivation = true;
    }
    #endregion

    #region FOR_GAMEOBJECT_FADE
    private void SetTarget(GameObject go)
    {
        target = go;
        mat = go.GetComponent<Renderer>().material;
        color = mat.GetColor("_Color");
    }
#if iTween
    public void FadeOut(GameObject go)
    {
        SetTarget(go);

        iTween.ValueTo(target, iTween.Hash(
            "from", ALPHA_MAX,
            "to", ALPHA_MIN,
            "onupdatetarget", target,
            "onupdate", "updateFromValue",
            "time", 0.8f,
            "delay", 0.0f,
            "easeType", "easeOutQuad",
            "oncomplete", "FadeOutComplete"));
    }
    public void FadeIn(GameObject go)
    {
        SetTarget(go);

        iTween.ValueTo(target, iTween.Hash(
            "from", ALPHA_MAX,
            "to", ALPHA_MIN,
            "onupdatetarget", target,
            "onupdate", "updateFromValue",
            "time", 0.8f,
            "delay", 0.0f,
            "easeType", "easeOutQuad",
            "oncomplete", "FadeInComplete"
            ));
    }
#endif
    private void updateFromValue(float newValue)
    {
        color.a = newValue;
        mat.SetColor("_Color", color);
    }
    private void updateFromValues(float newValue)
    {
        color.a = newValue;
        foreach (Material mat in mats)
        {
            mat.SetColor("_Color", color);
        }
    }
    private void FadeOutComplete()
    {
        Debug.Log("Fade Out Complete");
    }

    private void FadeInComplete()
    {
        Debug.Log("Fade In Complete");
    }

    public void AddFadeObjects(GameObject go)
    {
        targets.Add(go);
        mats.Add(go.GetComponent<Renderer>().material);
    }

    public void RemoveFadeObjects(GameObject go)
    {
        targets.Remove(go);
        mats.Remove(go.GetComponent<Renderer>().material);
    }

    public void StartFadeIn()
    {

    }
    public void StartFadeOut()
    {

    }

#endregion
}