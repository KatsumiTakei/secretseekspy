using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInfo
{
    Collision2D collision = null;

    public void CollisionEnter(Collision2D collision)
    {
        this.collision = collision;

        if(collision.gameObject.CompareTag("Damage"))
        {
            this.collision = null;
        }

    }

    public void CollisionExit()
    {
        collision = null;
    }

    public ContactPoint2D[] GetContactPoint2D()
    {
        return collision.contacts;
    }

}
