
using UnityDLL;
using UnityEngine;
using DG.Tweening;

public class Finder : MonoBehaviour
{
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0f, 1f);
    }

    private void OnEnable()
    {
        EventManager.OnFound += OnFound;
    }

    private void OnDisable()
    {
        EventManager.OnFound -= OnFound;
    }

    private void OnFound(GameObject watchingObject, GameObject foundObject)
    {
        AudioManager.PlaySE("Find");
        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() => {
            transform.position = foundObject.transform.position;
        });
        sequence.Append(
            transform
            .DOMoveY(TileMapManager.TileSize * 1.0f, 0.2f)
            .SetEase(Ease.OutBack)
            .SetRelative());
        sequence.Join(transform
            .DOScaleX(1f, 0.1f)
            .OnComplete(onComplete));
        
        EventManager.BroadcastGameFinish(false);

        void onComplete()
        {
            transform.DOScaleX(0f, 1f).SetDelay(2f);
        }
    }
}