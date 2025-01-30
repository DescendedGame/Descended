using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VeinEnd", menuName = "Scriptable Objects/Vein Ends")]
public class VeinEnd : ScriptableObject
{

    [SerializeField] GameObject m_solid_sphere;
    [SerializeField] GameObject m_solid_wall;
    [SerializeField] GameObject m_hollow_wall;

    public GameObject GetEndPrefab(Vein.EndType type, bool is_tunnel)
    {
        switch (type)
        {
            case Vein.EndType.Sphere:
                if(is_tunnel)
                {
                    return m_solid_sphere;
                }
                else return m_solid_sphere;
            case Vein.EndType.None:
                if (is_tunnel)
                {
                    return m_hollow_wall;
                }
                else return m_solid_wall;
            default:
                return m_solid_sphere;
        }
    }


    //MeshFilter m_filter;

    //private void Awake()
    //{
    //}

    //public enum EndType
    //{
    //    Sphere,
    //    None,
    //}

    //public void CreateEnd(Vector3 position, Quaternion rotation, float radius, EndType type, bool is_tunnel)
    //{
    //    transform.localPosition = position;
    //    transform.localRotation = rotation;
    //    switch (type)
    //    {
    //        case EndType.Sphere:
    //            GenerateSphere(radius, is_tunnel);
    //            break;
    //        case EndType.None:
    //            GenerateEnd(radius, is_tunnel);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //void GenerateSphere(float radius, bool is_tunnel)
    //{

    //}

    //void GenerateEnd(float radius, bool is_tunnel)
    //{
    //    if (is_tunnel)
    //    {
    //        GenerateOpenEnd(radius);
    //    }
    //    else
    //    {
    //        GenerateFlatEnd(radius);
    //    }
    //}
    //void GenerateFlatEnd(float radius)
    //{
    //    Vector3[] vertices = new Vector3[13];
    //    Vector3[] normals = new Vector3[13];
    //    vertices[0] = Vector3.zero;
    //    normals[0] = Vector3.forward;
    //    float angle_step = Mathf.PI * 2 / 12;
    //    float angle = 0;

    //    for (int i = 1; i < vertices.Length; i++)
    //    {
    //        normals[i] = Vector3.forward;
    //        vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    //        angle += angle_step;
    //    }

    //    int[] triangles = new int[12 * 3];

    //    for (int i = 0; i < triangles.Length / 3; i++)
    //    {
    //        triangles[i * 3] = 0;
    //        triangles[i * 3 + 1] = i + 1;
    //        if (i != 11)
    //        {
    //            triangles[i * 3 + 2] = i + 2;
    //        }
    //        else
    //        {
    //            triangles[i * 3 + 2] = 1;
    //        }
    //    }
    //    Mesh mesh = GetComponent<MeshFilter>().mesh;
    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.normals = normals;
    //    mesh.triangles = triangles;
    //}

    //void GenerateOpenEnd(float radius)
    //{
    //    Vector3[] vertices = new Vector3[24];
    //    Vector3[] normals = new Vector3[24];
    //    float angle_step = Mathf.PI * 2 / 12;
    //    float angle = 0;

    //    for (int i = 0; i < vertices.Length / 2; i++)
    //    {
    //        normals[i] = Vector3.forward;
    //        vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    //        angle += angle_step;
    //    }
    //    angle = 0;
    //    for (int i = vertices.Length / 2; i < vertices.Length; i++)
    //    {
    //        normals[i] = Vector3.forward;
    //        vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius * 0.8f;
    //        angle += angle_step;
    //    }

    //    int[] triangles = new int[12 * 3 * 2];

    //    for (int i = 0; i < vertices.Length / 2; i++)
    //    {
    //        if (i != (vertices.Length / 2)-1)
    //        {
    //            triangles[i * 6] = i;
    //            triangles[i * 6 + 1] = i + 13;
    //            triangles[i * 6 + 2] = i + 12;

    //            triangles[i * 6 + 3] = i;
    //            triangles[i * 6 + 4] = i + 1;
    //            triangles[i * 6 + 5] = i + 13;
    //        }
    //        else
    //        {
    //            triangles[i * 6] = i;
    //            triangles[i * 6 + 1] = i + 1;
    //            triangles[i * 6 + 2] = i+12;

    //            triangles[i * 6 +3] = i;
    //            triangles[i * 6 + 4] = 0;
    //            triangles[i * 6 + 5] = i+1;
    //        }

    //    }
    //    Mesh mesh = GetComponent<MeshFilter>().mesh;
    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.normals = normals;
    //    mesh.triangles = triangles;
    //}
}
