using Doozy.Engine.UI.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim
{
    SpriteRenderer spriteRenderer = null;
    Sprite[] anims = null;
    int animFreamCnt = 0;
    int animCnt = 0;
    int dirIndex = 0;
    const int AnimChangeDelimitationIndex = 4;


    public PlayerAnim(SpriteRenderer spriteRenderer, Sprite[] anims)
    {
        this.spriteRenderer = spriteRenderer;
        this.anims = anims;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveDir"> 0:down 1:up 2:left 3:right </param>
    public void MoveAnim(int moveDir)
    {
        dirIndex = AnimChangeDelimitationIndex * moveDir;
        spriteRenderer.sprite = anims[dirIndex + (animCnt % 2 + 1)];

        if(animFreamCnt++ % 3 == 1)
        {
            animCnt++;
        }
    }

    public void StopAnim()
    {
        spriteRenderer.sprite = anims[dirIndex];
    }   
    public void UseWeaponAnim()
    {
        spriteRenderer.sprite = anims[dirIndex + 3];
    }


}
