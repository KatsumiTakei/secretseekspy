using naichilab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScene : SceneBase
{

    [SerializeField]
    ParticleSystem[] particles = null;

    ProgressInstruction progressInstruction = null;
    RankingSceneManager rankingSceneManager = null;

    IEnumerator CoPlayParticles()
    {
        foreach (var particle in particles)
        {
            yield return new WaitForSeconds(0.3f);
            particle.Play();
        }

    }

    void Awake()
    {
        progressInstruction = GetComponentInChildren<ProgressInstruction>();
    }

    private void OnEnable()
    {
        print("OnEnable ResultScene");
        EventManager.OnMultipleInput += OnMultipleInput;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
        StartCoroutine(CoPlayParticles());
    }

    private void OnDisable()
    {
        print("OnDisable ResultScene");
        EventManager.OnMultipleInput -= OnMultipleInput;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    void OnSceneUnLoaded(Scene scene)
    {
        if (scene.name.Contains("Ranking"))
        {
            progressInstruction.gameObject.SetActive(true);
        }
    }

    public void OnMultipleInput(eInputType inputType)
    {
        if (!progressInstruction.gameObject.activeSelf)
            return;

        if (inputType == eInputType.ShotAndDecideKeyDown)
        {
            ProgressManager.Instance.MoveScene(eSceneState.Title, null, null);

            foreach (var particle in particles)
            {
                particle.Clear(true);
            }
        }
    }

    public override void Open()
    {
        AudioManager.ChangeBGMVolume(0.5f);
        gameObject.SetActive(true);
        progressInstruction.gameObject.SetActive(false);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }
}