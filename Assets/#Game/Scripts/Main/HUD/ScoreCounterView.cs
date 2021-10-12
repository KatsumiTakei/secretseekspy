using UnityEngine;
using DG.Tweening;

public class ScoreCounterView
{
    float tweenadjust = 1.0f;
    float tweenspeed = 0.5f;
    float ptsboader = 10f;

    TMPro.TextMeshPro scoretext = null;
    int drawValue = 0;

    bool isFillZero = false;
    string digit = "D";
    int limitScore = 0;

    public ScoreCounterView(TMPro.TextMeshPro scoretext, float tweenspeed, float ptsboader, int limitScore, bool isFillZero)
    {
        this.ptsboader = Mathf.Max(ptsboader, 1f);
        this.tweenspeed = Mathf.Clamp(tweenspeed, 0.1f, 5f);
        this.digit += limitScore.ToString().Length;
        this.scoretext = scoretext;
        this.scoretext.text = (isFillZero) ? 0.ToString(digit) : "0";
        this.isFillZero = isFillZero;
        this.limitScore = limitScore;
    }

    public void SimpleChangeScore(int score)
    {
        scoretext.text = (isFillZero) ? score.ToString(digit): score.ToString();
    }

    /// <summary>
    /// スコアを補間しながら加算する関数
    /// </summary>
    /// <param name="pts">変化するバリュー</param>
    /// <param name="score">現在のスコア</param>
    public void AnimChangeScore(int pts, int score)
    {
        if (pts < ptsboader)
        {//   補間補正判定 ptsの大きさに比例した変位をさせるが変位上限ptsboaderを超えたら違う挙動にする
            tweenadjust = Mathf.Max(pts / ptsboader, 0.1f);
        }
        else
        {
            tweenadjust = 1f;
        }

        DOTween
            .To(
            () => drawValue,
            (n) => drawValue = n,
            Mathf.Clamp(score + pts, 0, limitScore),
            tweenspeed * tweenadjust)
            .OnUpdate(() => { scoretext.text = (isFillZero) ? drawValue.ToString(digit) : drawValue.ToString(); });
        
    }
}