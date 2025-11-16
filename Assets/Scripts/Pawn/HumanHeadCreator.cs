using System.Collections.Generic;
using UnityEngine;

public class HumanHeadCreator : MonoBehaviour
{
    [SerializeField] GameObject originalHead;
    [SerializeField] HumanHeadRegions headRegions;

    MeshFilter[] allFilters;

    Dictionary<Vector3, Vector3> updatedPositions;

    private void Awake()
    {
        allFilters = GetComponentsInChildren<MeshFilter>();
    }

    public void CreateHead(HumanHeadSettings settings)
    {


        Debug.Log(settings);
    }

    void CalculateScalp()
    {
        foreach(Vector3 position in headRegions.scalp)
        {
            updatedPositions[position] = position * 1;
        }
    }
}
