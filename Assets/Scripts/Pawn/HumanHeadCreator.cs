using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanHeadCreator : MonoBehaviour
{
    [SerializeField] HumanHeadRegions headRegions;

    Vector3[][] originalVertices;
    MeshFilter[] allFilters;

    Dictionary<Vector3, Vector3> updatedPositions = new Dictionary<Vector3, Vector3>();

    private void Awake()
    {
        allFilters = GetComponentsInChildren<MeshFilter>();
        originalVertices = new Vector3[allFilters.Length][];
        for(int i = 0; i < allFilters.Length; i++)
        {
            originalVertices[i] = allFilters[i].mesh.vertices;
        }
    }

    public void CreateHead(HumanHeadSettings settings)
    {
        updatedPositions.Clear();

        foreach(Vector3[] vertices in originalVertices)
        {
            foreach(Vector3 vertex in vertices)
            {
                updatedPositions[vertex] = vertex;
            }
        }
        foreach(Vector3 vertex in headRegions.scalp)
        {
            updatedPositions[vertex] = updatedPositions[vertex] * settings.skullSize;
            Vector3 mirroredVertex = new Vector3(-vertex.x, vertex.y, vertex.z);
            updatedPositions[mirroredVertex] = updatedPositions[mirroredVertex] * settings.skullSize;
        }
        foreach (Vector3 vertex in headRegions.brow)
        {
            updatedPositions[vertex] = updatedPositions[vertex] * settings.browDepth;
            Vector3 mirroredVertex = new Vector3(-vertex.x, vertex.y, vertex.z);
            updatedPositions[mirroredVertex] = updatedPositions[mirroredVertex] * settings.browDepth;
        }

        for (int i = 0; i < allFilters.Length; i++)
        {
            Vector3[] vertices = new Vector3[originalVertices[i].Length];
            for(int j = 0; j < vertices.Length; j++)
            {
                vertices[j] = originalVertices[i][j];
            }

            for (int j = 0; j < vertices.Length; j++)
            {
                if (updatedPositions.ContainsKey(vertices[j]))
                {
                    vertices[j] = updatedPositions[vertices[j]];
                }
            }
            allFilters[i].mesh.vertices = vertices;
            allFilters[i].mesh.RecalculateBounds();
            allFilters[i].mesh.RecalculateNormals();
        }
    }

    void CalculateScalp()
    {
        foreach(Vector3 position in headRegions.scalp)
        {
            updatedPositions[position] = position * 1;
        }
    }
}
