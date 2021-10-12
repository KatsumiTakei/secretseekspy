using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreasureBoxModel
{
    public Sprite[] sprites = null;
    public KeyValuePair<ItemBase, float>[] itemTable = null;

    public TreasureBoxModel(Sprite[] sprites, KeyValuePair<ItemBase, float>[] itemTable)
    {
        this.sprites = sprites;
        this.itemTable = itemTable;
    }

}

public class TreasureBox : MonoBehaviour
    , ISurveyTarget
    , IInitializable
{
    SpriteRenderer spriteRenderer = null;
    CircleCollider2D circleCollider = null;
    TreasureBoxModel model = null;

    public void Initialize()
    {
        circleCollider.enabled = true;
        model = TileMapManager.Instance.InjectTreasureBoxModel();
        spriteRenderer.sprite = model.sprites[0];
    }

    public void Surveyed(Player player)
    {
        spriteRenderer.sprite = model.sprites[1];
        TileMapManager.Instance.GetItem(player, transform.position, model.itemTable);
        circleCollider.enabled = false;
        //AudioManager.PlaySE("OpenTreasureBox");
        print(gameObject.name);
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = TileMapManager.Instance.InjectTreasureBoxModel();
        spriteRenderer.sprite = model.sprites[0];
    }

}
