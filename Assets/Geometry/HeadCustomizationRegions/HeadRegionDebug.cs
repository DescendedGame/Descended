using System.Linq;
using UnityEngine;

public class HeadRegionDebug : MonoBehaviour
{
    public HeadCustomizationRegions regions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter filter in filters)
        {
            Debug.Log("In a filter");
            Mesh newMesh = new Mesh();
            newMesh.vertices = filter.mesh.vertices;
            newMesh.normals = filter.mesh.normals;
            newMesh.triangles = filter.mesh.triangles;
            Color[] colors = new Color[newMesh.vertices.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                if (regions.scalp.Contains(newMesh.vertices[i]) || regions.scalp.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1,1,1))))
                    colors[i] = Color.red;
            }
            newMesh.colors = colors;
            filter.mesh = newMesh;
        }
    }
}
