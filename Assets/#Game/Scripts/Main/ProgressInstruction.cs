using STLExtensiton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressInstruction : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer spaceRender = null;

    [SerializeField]
    Sprite[] spaceSprites = null;

    [SerializeField]
    TextMeshPro textMesh = null;

    int animCnt = 0;

    void OnEnable()
    {
        animCnt = 0;
    }

    void Update()
    {
        if (animCnt++ % 60 == 0)
        {
            spaceRender.sprite = (spaceRender.sprite == spaceSprites[1]) ? spaceSprites[0] : spaceSprites[1];
            textMesh.color = (textMesh.color == Color.white) ? Color.clear : Color.white;
        }
    }
}
