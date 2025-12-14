using System.Collections.Generic;
using UnityEngine;

public class PlasticMesh : MonoBehaviour
{
    [SerializeField] HumanHeadRegions regions;

    Dictionary<string, Vector3[]> regionByName = new Dictionary<string, Vector3[]>();
    Dictionary<Vector3, Vector3> updatedPositions = new Dictionary<Vector3, Vector3>();
    public delegate Vector3 VertexTransformation(Vector3 vertex);
    MeshFilter[] filters;
    Vector3[][] originalVertices;
    bool initialized = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized) return;
        filters = GetComponentsInChildren<MeshFilter>();
        originalVertices = new Vector3[filters.Length][];

        for (int i = 0; i < filters.Length; i++)
        {
            originalVertices[i] = new Vector3[filters[i].mesh.vertices.Length];
            for (int j = 0; j < originalVertices[i].Length; j++)
            {
                originalVertices[i][j] = filters[i].mesh.vertices[j];
            }
        }

        regionByName["scalp"] = regions.scalp;
        regionByName["browMiddle"] = regions.browMiddle;
        regionByName["browInner"] = regions.browInner;
        regionByName["browOuter"] = regions.browOuter;
        regionByName["temple"] = regions.temple;
        regionByName["eyeHole"] = regions.eyeHole;
        regionByName["nose"] = regions.nose;
        regionByName["noseTip"] = regions.noseTip;
        regionByName["cheekBoneRear"] = regions.cheekBoneRear;
        regionByName["cheekBoneMiddle"] = regions.cheekBoneMiddle;
        regionByName["cheekBoneFront"] = regions.cheekBoneFront;
        regionByName["cheekUpper"] = regions.cheekUpper;
        regionByName["cheekLower"] = regions.cheekLower;
        regionByName["earSquare"] = regions.earSquare;
        regionByName["jawTop"] = regions.jawTop;
        regionByName["jawCorner"] = regions.jawCorner;
        regionByName["mouth"] = regions.mouth;
        regionByName["upperLip"] = regions.upperLip;
        regionByName["lowerLip"] = regions.lowerLip;
        regionByName["chin"] = regions.chin;

        ResetPositions();
        initialized = true;
    }

    public void ResetPositions()
    {
        foreach (Vector3[] group in regionByName.Values)
        {
            foreach (Vector3 vertex in group)
            {
                updatedPositions[vertex] = vertex;
            }
        }
    }

    public void TransformVertexGroup(string groupName, VertexTransformation vertexTransformation)
    {
        if (!regionByName.ContainsKey(groupName)) return;
        foreach(Vector3 vertex in regionByName[groupName])
        {
            updatedPositions[vertex] = vertexTransformation(updatedPositions[vertex]);
        }
    }

    public void RecalculateMesh()
    {
        for(int i = 0; i < originalVertices.Length; i++)
        {
            Vector3[] vertices = new Vector3[originalVertices[i].Length];
            for(int j = 0; j < vertices.Length; j++)
            {
                if (updatedPositions.ContainsKey(originalVertices[i][j]))
                {
                    vertices[j] = updatedPositions[originalVertices[i][j]];
                }
                else
                {
                    vertices[j] = originalVertices[i][j];
                }
            }

            filters[i].mesh.vertices = vertices;
            filters[i].mesh.RecalculateNormals();
            filters[i].mesh.RecalculateBounds();
        }
    }

}
