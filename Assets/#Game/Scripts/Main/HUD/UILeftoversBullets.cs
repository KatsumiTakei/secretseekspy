using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeftoversBullets : MonoBehaviour
    , IInitializable
{
    readonly Color[] CartridgesColors = { Color.gray, Color.white, Color.yellow, Color.green };
    SpriteRenderer[] cartridges = null;

    public void Initialize()
    {
        for (int i = 0; i < cartridges.Length; i++)
            cartridges[i].color = CartridgesColors[0];
    }

    void Awake()
    {
        cartridges = GetComponentsInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        EventManager.OnShot += OnShot;
        EventManager.OnGetBullet += OnGetBullet;
    }

    void OnDisable()
    {
        EventManager.OnShot -= OnShot;
        EventManager.OnGetBullet -= OnGetBullet;
    }

    void OnShot(int bulletIndex)
    {
        cartridges[bulletIndex % 10].color = CartridgesColors[(bulletIndex) / 10];
    }

    void OnGetBullet(int bulletIndex)
    {
        cartridges[bulletIndex % 10].color = CartridgesColors[bulletIndex / 10 + 1];
    }
}
