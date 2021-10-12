using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBullet : ItemBase
{
    public override void Give(Player player)
    {
        int random = Random.Range(1, 4);
        AnimGive(itemName + random.ToString() + "個");
        StartCoroutine(player.CoGetBullet(random));

        //for (int i = 0; i < random; i++)
        //{
        //    player.GetBullet();
        //}
    }

}
