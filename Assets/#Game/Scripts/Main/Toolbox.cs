using UnityEngine;

[DisallowMultipleComponent]
[DefaultExecutionOrder(1)]
[RequireComponent(typeof(TileMapManager))]
[RequireComponent(typeof(ProgressManager))]
[RequireComponent(typeof(AudioManager))]
public class Toolbox : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        AudioManager.PlayBGM("BGM");
        AudioManager.ChangeBGMVolume(0.5f);

    }

    void Update()
    {
        InputManager.ManualUpdate();
    }


}
