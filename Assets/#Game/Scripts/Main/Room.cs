using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
    ,IInitializable
{
    Entrance entrance = null;
    bool isSetup = false;

    public void Initialize()
    {
        isSetup = false;
    }

    Entrance GetEntrance()
    {
        return entrance = entrance ?? GetComponentInChildren<Entrance>();
    }

    public void Admission(Vector3 pos, bool isMainMap)
    {
        gameObject.SetActive(true);
        if (isMainMap || isSetup)
            return;

        var entrance = GetEntrance();
        entrance.transform.position = pos;
        transform.position = entrance.transform.localPosition - entrance.OriginLocalPos;
        entrance.transform.localPosition = entrance.OriginLocalPos;
        isSetup = true;
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }

}
