
using UnityEngine;
using DG.Tweening;

public class TargetingMessageView : MonoBehaviour
{

    SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        EventManager.OnTargetingObj += OnTargetingObj;
    }

    void OnDisable()
    {
        EventManager.OnTargetingObj -= OnTargetingObj;
    }

    void OnTargetingObj(bool isTargeting)
    {
        spriteRenderer.color = (isTargeting) ? Color.white : new Color(0.25f, 0.25f, 0.25f, 1f);
    }
}
