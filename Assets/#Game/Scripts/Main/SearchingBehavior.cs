//------------------------------------------------------------------------
//
// (C) Copyright 2017 Urahimono Project Inc.
//  @note   https://www.urablog.xyz/entry/2017/10/24/073654
//
//------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class SearchingBehavior : MonoBehaviour
{
    public enum eAxis
    {
        XY,
        XZ,
    }

    public enum eDimension
    {
        D2,
        D3,
    }


    public event System.Action<GameObject> onFound = (obj) => { };
    public event System.Action<GameObject> onLost = (obj) => { };

    [SerializeField, Range(0.0f, 360.0f)]
    float searchAngle = 0.0f;
    float searchCosTheta = 0.0f;

    SphereCollider sphereCollider = null;
    CircleCollider2D circleCollider = null;
    List<FoundData> foundList = new List<FoundData>();
    int layerMask = 0;

    [SerializeField]
    eAxis axis = eAxis.XY;

    [SerializeField]
    eDimension dimension = eDimension.D2;

    Vector3 axisAdjust = Vector3.zero;

    public eAxis GetAxis()
    {
        return axis;
    }

    public float SearchAngle { get { return searchAngle; } }

    public float SearchRadius
    {
        get
        {
            if (dimension == eDimension.D2)
            {
                if (circleCollider == null)
                {
                    circleCollider = GetComponent<CircleCollider2D>();
                }
                return circleCollider != null ? circleCollider.radius : 0.0f;
            }
            else
            {
                if (sphereCollider == null)
                {
                    sphereCollider = GetComponent<SphereCollider>();
                }
                return sphereCollider != null ? sphereCollider.radius : 0.0f;
            }
        }
    }


    private void Awake()
    {
        layerMask = LayerMask.GetMask(new string[] { "Player", "Collision" });
        sphereCollider = GetComponent<SphereCollider>();
        circleCollider = GetComponent<CircleCollider2D>();
        ApplySearchAngle();

        switch (axis)
        {
            case eAxis.XY:
                axisAdjust = new Vector3(1f, 1f, 0f);
                break;
            case eAxis.XZ:
                axisAdjust = new Vector3(1f, 0f, 1f);
                break;
        }
    }

    private void OnDestroy()
    {
        foundList.Clear();
    }

    // シリアライズされた値がインスペクター上で変更されたら呼ばれます。
    private void OnValidate()
    {
        ApplySearchAngle();
    }

    private void ApplySearchAngle()
    {
        float searchRad = searchAngle * 0.5f * Mathf.Deg2Rad;
        searchCosTheta = Mathf.Cos(searchRad);
    }

    private void Update()
    {
        UpdateFoundObject();
    }

    private void UpdateFoundObject()
    {
        foreach (var foundData in foundList)
        {
            GameObject targetObject = foundData.Obj;
            if (targetObject == null)
            {
                continue;
            }

            bool isFound = CheckFoundObject(targetObject);
            foundData.Update(isFound);

            if (foundData.IsFound())
            {
                //onFound(targetObject);
                EventManager.BroadcastFound(gameObject, targetObject);

            }
            else if (foundData.IsLost())
            {
                //onLost(targetObject);
            }
        }
    }

    private bool CheckFoundObject(GameObject i_target)
    {
        Vector3 targetPosition = i_target.transform.position;
        Vector3 myPosition = transform.position;

        Vector3 myPositionAdjust = Vector3.Scale(myPosition, axisAdjust);
        Vector3 targetPositionAdjust = Vector3.Scale(targetPosition, axisAdjust);

        Vector3 toTargetFlatDir = (targetPositionAdjust - myPositionAdjust).normalized;
        Vector3 myForward = (dimension == eDimension.D2) ? transform.up : transform.forward;
        if (!IsWithinRangeAngle(myForward, toTargetFlatDir, searchCosTheta))
        {
            return false;
        }

        Vector3 toTargetDir = (targetPosition - myPosition).normalized /** 10*/;

        if (!IsHitRay(myPosition, toTargetDir, i_target))
        {
            return false;
        }

        return true;
    }

    private bool IsWithinRangeAngle(Vector3 i_forwardDir, Vector3 i_toTargetDir, float i_cosTheta)
    {
        // 方向ベクトルが無い場合は、同位置にあるものだと判断する。
        if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        float dot = Vector3.Dot(i_forwardDir, i_toTargetDir);
        return dot >= i_cosTheta;
    }

    private bool IsHitRay(Vector3 i_fromPosition, Vector3 i_toTargetDir, GameObject i_target)
    {
        // 方向ベクトルが無い場合は、同位置にあるものだと判断する。
        if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        return Raycast(i_fromPosition, i_toTargetDir, i_target);
    }

    bool Raycast(Vector3 i_fromPosition, Vector3 i_toTargetDir, GameObject i_target)
    {
        bool res = false;
        if (dimension == eDimension.D2)
        {
            var onHitRay2D = Physics2D.RaycastAll(i_fromPosition, i_toTargetDir, SearchRadius, layerMask).FirstOrDefault();
            res = onHitRay2D.transform != null;

            if (!res)
                return false;

            if (onHitRay2D.transform.gameObject != i_target)
                return false;
        }
        else
        {
            RaycastHit onHitRay = default;
            res = Physics.Raycast(i_fromPosition, i_toTargetDir, out onHitRay, SearchRadius, layerMask);

            if (!res)
                return false;

            if (onHitRay.transform.gameObject != i_target)
                return false;
        }

        return res;
    }

    void TriggerEnter(GameObject enterObject)
    {
        // 念のため多重登録されないようにする。
        if (foundList.Find(value => value.Obj == enterObject) == null)
        {
            foundList.Add(new FoundData(enterObject));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        TriggerEnter(collider.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter(collision.gameObject);
    }
    //void TriggerExit(GameObject exitObject)
    //{
    //    var foundData = foundList.Find(value => value.Obj == exitObject);
    //    if (foundData == null)
    //    {
    //        return;
    //    }

    //    if (foundData.IsCurrentFound())
    //    {
    //        onLost(foundData.Obj);
    //    }

    //    foundList.Remove(foundData);
    //}
    //private void OnTriggerExit(Collider collider)
    //{
    //    TriggerExit(collider.gameObject);
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    TriggerExit(collision.gameObject);
    //}

    private class FoundData
    {
        public FoundData(GameObject i_object)
        {
            m_obj = i_object;
        }

        private GameObject m_obj = null;
        private bool m_isCurrentFound = false;
        private bool m_isPrevFound = false;

        public GameObject Obj
        {
            get { return m_obj; }
        }

        public Vector3 Position
        {
            get { return Obj != null ? Obj.transform.position : Vector3.zero; }
        }

        public void Update(bool i_isFound)
        {
            m_isPrevFound = m_isCurrentFound;
            m_isCurrentFound = i_isFound;
        }

        public bool IsFound()
        {
            return m_isCurrentFound && !m_isPrevFound;
        }

        public bool IsLost()
        {
            return !m_isCurrentFound && m_isPrevFound;
        }

        public bool IsCurrentFound()
        {
            return m_isCurrentFound;
        }
    }

}