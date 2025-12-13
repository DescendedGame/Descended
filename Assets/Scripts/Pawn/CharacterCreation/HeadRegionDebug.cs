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
                if (regions.scalp.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.browMiddle.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.browInner.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.browOuter.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.eyeHole.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.nose.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.noseTip.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.cheekBoneRear.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.cheekBoneMiddle.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.cheekBoneFront.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.cheekUpper.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.cheekLower.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.earSquare.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.jawTop.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.jawCorner.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.mouth.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.upperLip.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.lowerLip.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;
                if (regions.chin.Contains(newMesh.vertices[i]))
                    colors[i] = Color.red;

            }
            newMesh.colors = colors;
            filter.mesh = newMesh;
        }
    }
}
