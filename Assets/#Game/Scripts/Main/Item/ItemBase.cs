using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected string itemName = string.Empty;


    public abstract void Give(Player player);

    protected void AnimGive(string message)
    {
        LogMessage.OutputLog(message + "Get!");
        AudioManager.PlaySE("GetItem");

        transform
            .DOLocalMoveY(25f, 0.4f)
            .SetRelative()
            .OnComplete(onLocalMoveComplete);

        void onLocalMoveComplete()
        {
            transform
                .DOPunchPosition(Vector3.up * 5f, 0.6f)
                .OnComplete(onComplete);
        }

        void onComplete()
        {
            gameObject.SetActive(false);
        }
    }
}
