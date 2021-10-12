using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAlarm : ItemBase
{

    public override void Give(Player player)
    {
        //player.GetBullet();
        AnimGive("");
    }

    private void Start()
    {
        itemName = "毒針";
    }
}
