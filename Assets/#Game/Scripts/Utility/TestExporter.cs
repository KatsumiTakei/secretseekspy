using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必要コンポーネント
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TestExporter : MonoBehaviour
{

    private static readonly int TRIANGLE_COUNT = 12;
    private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);

    private void Awake()
    {
        SearchingBehavior i_object = GetComponent<SearchingBehavior>();

        if (i_object.SearchRadius <= 0.0f)
        {
            return;
        }

        Gizmos.color = MESH_COLOR;

        Transform transform = i_object.transform;
        Vector3 pos = transform.position + Vector3.up * 0.01f; // 0.01fは地面と高さだと見づらいので調整用。
        Quaternion rot = transform.rotation;
        Vector3 scale = Vector3.one * i_object.SearchRadius;

        Mesh mesh = null;

        if (i_object.SearchAngle > 0.0f)
        {
            mesh = CreateFanMesh(i_object.SearchAngle, TRIANGLE_COUNT, i_object.GetAxis());

            //Gizmos.DrawMesh(mesh, pos, rot, scale);

        }

        MeshFilter meshFillter = GetComponent<MeshFilter>();
        meshFillter.mesh = mesh;

        //頂点情報をもとに法線を計算
        mesh.RecalculateNormals();

        ObjExporter.MeshToFile(meshFillter, "Fan");

    }


    private static Mesh CreateFanMesh(float i_angle, int i_triangleCount, SearchingBehavior.eAxis axis)
    {
        var mesh = new Mesh();

        var vertices = CreateFanVertices(i_angle, i_triangleCount, axis);

        var triangleIndexes = new List<int>(i_triangleCount * 3);

        for (int i = 0; i < i_triangleCount; ++i)
        {
            triangleIndexes.Add(0);
            triangleIndexes.Add(i + 1);
            triangleIndexes.Add(i + 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangleIndexes.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    private static Vector3[] CreateFanVertices(float i_angle, int i_triangleCount, SearchingBehavior.eAxis axis)
    {
        if (i_angle <= 0.0f)
        {
            throw new System.ArgumentException(string.Format("角度がおかしい！ i_angle={0}", i_angle));
        }

        if (i_triangleCount <= 0)
        {
            throw new System.ArgumentException(string.Format("数がおかしい！ i_triangleCount={0}", i_triangleCount));
        }

        i_angle = Mathf.Min(i_angle, 360.0f);

        var vertices = new List<Vector3>(i_triangleCount + 2);

        // 始点
        vertices.Add(Vector3.zero);

        // Mathf.Sin()とMathf.Cos()で使用するのは角度ではなくラジアンなので変換しておく。
        float radian = i_angle * Mathf.Deg2Rad;
        float startRad = -radian / 2;
        float incRad = radian / i_triangleCount;

        for (int i = 0; i < i_triangleCount + 1; ++i)
        {
            float currentRad = startRad + (incRad * i);

            Vector3 vertex = (axis == SearchingBehavior.eAxis.XY) ?
                new Vector3(Mathf.Sin(currentRad), Mathf.Cos(currentRad), 0.0f)
                : new Vector3(Mathf.Sin(currentRad), 0.0f, Mathf.Cos(currentRad));
            vertices.Add(vertex);
        }

        return vertices.ToArray();
    }

}