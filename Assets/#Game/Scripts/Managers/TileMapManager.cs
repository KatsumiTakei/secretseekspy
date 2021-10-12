using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityDLL;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class TileMapManager : SingletonMonoBehaviour<TileMapManager>
{
    [SerializeField]
    ItemBase[] itemTypes = null;

    [SerializeField]
    TreasureBoxModel[] treasureBoxModels = null;

    KeyValuePair<TreasureBoxModel, float>[] treasureBoxModelTable = null;
    public const int TileSize = 32;

    public TreasureBoxModel InjectTreasureBoxModel()
    {
        return RandomWithWeight.Lotto(treasureBoxModelTable);
    }

    public void GetItem(Player player, Vector3 position, KeyValuePair<ItemBase, float>[] itemTable)
    {
        var item = RandomWithWeight.Lotto(itemTable);
        item.gameObject.SetActive(true);
        item.transform.position = position;
        item.Give(player);
    }

    protected void Awake()
    {
        base.Awake();

        treasureBoxModels[0].itemTable = new KeyValuePair<ItemBase, float>[] {
            new KeyValuePair<ItemBase, float>(itemTypes[0], 70f),
            new KeyValuePair<ItemBase, float>(itemTypes[1], 20f),
            new KeyValuePair<ItemBase, float>(itemTypes[2], 10f)
                };

        treasureBoxModels[1].itemTable = new KeyValuePair<ItemBase, float>[] {
            new KeyValuePair<ItemBase, float>(itemTypes[1], 50f),
            new KeyValuePair<ItemBase, float>(itemTypes[2], 40f),
            new KeyValuePair<ItemBase, float>(itemTypes[3], 10f)
                };

        foreach (var item in itemTypes)
        {
            item?.gameObject.SetActive(false);
        }

        treasureBoxModelTable = new KeyValuePair<TreasureBoxModel, float>[] {
            new KeyValuePair<TreasureBoxModel, float>(treasureBoxModels[0], 60f),
            new KeyValuePair<TreasureBoxModel, float>(treasureBoxModels[1], 40f),
        };
    }

}
