using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrice : ItemBase
{
    [SerializeField]
    int price = 0;

    public override void Give(Player player)
    {
        AnimGive(itemName);
        EventManager.BroadcastAddScore(price);
        //StartCoroutine();
    }

}
