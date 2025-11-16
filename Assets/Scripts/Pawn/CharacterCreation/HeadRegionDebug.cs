using System.Linq;
using UnityEngine;

public class HeadRegionDebug : MonoBehaviour
{
    public HumanHeadRegions regions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter filter in filters)
        {
            Debug.Log(filter.gameObject.name);
            Mesh newMesh = new Mesh();
            newMesh.vertices = filter.mesh.vertices;
            newMesh.normals = filter.mesh.normals;
            newMesh.triangles = filter.mesh.triangles;
            Color[] colors = new Color[newMesh.vertices.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                if (regions.brow.Contains(newMesh.vertices[i]) || regions.brow.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1,1,1))))
                    colors[i] = Color.red;
                if (regions.cheek.Contains(newMesh.vertices[i]) || regions.cheek.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.cheekBone.Contains(newMesh.vertices[i]) || regions.cheekBone.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.chinTip.Contains(newMesh.vertices[i]) || regions.chinTip.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.earSquare.Contains(newMesh.vertices[i]) || regions.earSquare.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.eyeHole.Contains(newMesh.vertices[i]) || regions.eyeHole.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.jawLine.Contains(newMesh.vertices[i]) || regions.jawLine.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.lowerLip.Contains(newMesh.vertices[i]) || regions.lowerLip.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.mouth.Contains(newMesh.vertices[i]) || regions.mouth.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.nose.Contains(newMesh.vertices[i]) || regions.nose.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.noseTip.Contains(newMesh.vertices[i]) || regions.noseTip.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.scalp.Contains(newMesh.vertices[i]) || regions.scalp.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;
                if (regions.upperLip.Contains(newMesh.vertices[i]) || regions.upperLip.Contains(Vector3.Scale(newMesh.vertices[i], new Vector3(-1, 1, 1))))
                    colors[i] = Color.red;

            }
            newMesh.colors = colors;
            filter.mesh = newMesh;
        }
    }
}
