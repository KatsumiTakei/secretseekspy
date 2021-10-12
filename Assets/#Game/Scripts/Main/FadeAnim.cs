using DG.Tweening;
using System;
using System.Collections;
using UnityDLL;
using UnityEngine;

public class FadeAnim : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer white = null;
    [SerializeField]
    SpriteRenderer black = null;

    [SerializeField]
    SpriteRenderer fadeL = null;
    [SerializeField]
    SpriteRenderer fadeR = null;

    TransformData initWhiteTrans = null;
    TransformData initBlackTrans = null;

    WaitForSeconds waitSeconds = new WaitForSeconds(1f);
    WaitForSeconds waitOnetenth = new WaitForSeconds(0.1f);

    float fadeInitPosX = 0f;

    Camera camera = null;

    void Awake()
    {
        initWhiteTrans = new TransformData(white.transform);
        initBlackTrans = new TransformData(black.transform);

        fadeInitPosX = fadeR.transform.localPosition.x;

        camera = GetComponentInParent<Camera>();
        //  /*!<  test*/    StartCoroutine(CoAnim(null));
    }


    IEnumerator CoFadeInOut(Action fadeInComplete, Action fadeOutComplete)
    {
        AudioManager.PlaySE("FadeIn");
        yield return CoFadeIn(fadeInComplete);

        yield return waitSeconds;

        AudioManager.PlaySE("FadeOut");
        yield return CoFadeOut(fadeOutCompleteImpl);

        void fadeOutCompleteImpl()
        {//  フェードに使用するリソースは演出後に初期化
            initWhiteTrans.ApplyTo(white.transform);
            initBlackTrans.ApplyTo(black.transform);

            fadeL.transform.SetLocalPositionX(-fadeInitPosX);
            fadeR.transform.SetLocalPositionX(fadeInitPosX);

            fadeOutComplete?.Invoke();
        }

    }

    public void FadeInOut(Action fadeInComplete, Action fadeOutComplete)
    {
        camera.depth = 1;
        StartCoroutine(CoFadeInOut(fadeInComplete, fadeOutComplete));
    }

    public IEnumerator CoFadeIn(Action animComplete)
    {//  太極図が合わさる

        float moveDuration = 0.25f;
        Vector3 endvalue = new Vector3(0f, 0f, 0.5f);
        float rotateDuration = 0.3f;

        Sequence whiteAnim = DOTween.Sequence();
        whiteAnim.Append(white.transform.DOLocalMove(endvalue, moveDuration));
        whiteAnim.Join(white.transform.DOLocalRotate(Vector3.zero, rotateDuration).SetEase(Ease.OutFlash));

        Sequence blackAnim = DOTween.Sequence();
        blackAnim.Append(black.transform.DOLocalMove(endvalue, moveDuration));
        blackAnim.Join(black.transform.DOLocalRotate(Vector3.zero, rotateDuration).SetEase(Ease.OutFlash));

        Sequence fadeLAnim = DOTween.Sequence();
        fadeLAnim.Append(fadeL.transform.DOLocalMove(new Vector3(-200f, 0f, 1f), moveDuration));

        Sequence fadeRAnim = DOTween.Sequence();
        fadeRAnim.Append(fadeR.transform.DOLocalMove(new Vector3(200f, 0f, 1f), moveDuration));

        yield return waitSeconds;

        //  アニメーション完了
        animComplete?.Invoke();
    }

    public IEnumerator CoFadeOut(Action animComplete)
    {//  太極図の収縮

        Vector3 endvalue = new Vector3(0f, 0f, -240f);
        float rotateDuration = 0.01f;

        Sequence whiteAnim = DOTween.Sequence();
        whiteAnim.Append(white.transform.DORotate(endvalue, rotateDuration).SetEase(Ease.InOutExpo).SetRelative().SetLoops(10));
        whiteAnim.Join(white.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBounce));

        Sequence blackAnim = DOTween.Sequence();
        blackAnim.Append(black.transform.DORotate(endvalue, rotateDuration).SetEase(Ease.InOutExpo).SetRelative().SetLoops(10));
        blackAnim.Join(black.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBounce));

        yield return waitOnetenth;

        //  アニメーション完了
        animComplete?.Invoke();
        camera.depth = -5;

    }


}
