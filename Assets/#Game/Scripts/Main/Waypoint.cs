using STLExtensiton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    Vector3[] waypoints = null;
    Vector3 destination = Vector3.zero;

    Loop currentWaypointIndex = new Loop(0, 0);

    List<Transform> waypointsTrans = null;

    void Awake()
    {
        waypointsTrans = GetComponentsInChildren<Transform>().ToList();
        waypointsTrans.RemoveAt(0);
        waypoints = new Vector3[waypointsTrans.Count];
        waypointsTrans.Indexed().ForEach(_ => waypoints[_.Index] = _.Element.position);

        currentWaypointIndex.Reset(0, waypoints.Length - 1);
        GetComponentsInChildren<SpriteRenderer>().ForEach(_ => _.enabled = false);
    }

    void OnEnable()
    {
        StartCoroutine(CoInitializeWaypoints());
    }

    /// <summary>
    /// Waypointsは部屋の移動で座標が変わるためコルーチンで移動後に初期化
    /// </summary>
    /// <returns></returns>
    IEnumerator CoInitializeWaypoints()
    {
        yield return null;
        waypointsTrans.Indexed().ForEach(_ => waypoints[_.Index] = _.Element.position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourcePoint"></param>
    /// <returns>進行方向のIndex -1 は待機</returns>
    public int CalcStepDirection(Vector3 sourcePoint)
    {
        var distance = destination - sourcePoint;
        if (Math.Abs(distance.x) > TileMapManager.TileSize * 0.5f)
        {
            return (distance.x > 0) ? MoveDirection.Right : MoveDirection.Left;
        }
        if (Math.Abs(distance.y) > TileMapManager.TileSize * 0.5f)
        {
            return (distance.y > 0) ? MoveDirection.Up : MoveDirection.Down;
        }

        return -1;
    }

    public void SearchNearPoint(Vector3 sourcePoint)
    {
        currentWaypointIndex.Reset(0, waypoints.Length - 1);
        float distance = 99999f;

        waypoints.ForEach(_ => searchDestination(_));

        void searchDestination(Vector3 waypoint)
        {
            float calcDistance = Vector3.Distance(sourcePoint, waypoint);
            if (calcDistance < distance)
            {
                distance = calcDistance;
                destination = waypoint;
            }
        }
    }


    public void SetNextDestination()
    {
        Debug.Log("wait pre");
        currentWaypointIndex += 1;
        Debug.Log("wait afe");
        destination = waypoints[currentWaypointIndex];
        Debug.Log("wait end");
    }
}
