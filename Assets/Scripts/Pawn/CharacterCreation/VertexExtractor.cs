using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VertexExtractor : MonoBehaviour
{
    public HumanHeadRegions headRegions;
    public GameObject brow;
    public GameObject cheek;
    public GameObject cheekBone;
    public GameObject chinTip;
    public GameObject earSquare;
    public GameObject eyeHole;
    public GameObject jawLine;
    public GameObject lowerLip;
    public GameObject mouth;
    public GameObject nose;
    public GameObject noseTip;
    public GameObject scalp;
    public GameObject upperLip;

    void Awake()
    {
        headRegions.brow = ExtractVertices(brow);
        headRegions.cheek = ExtractVertices(cheek);
        headRegions.cheekBone = ExtractVertices(cheekBone);
        headRegions.chinTip = ExtractVertices(chinTip);
        headRegions.earSquare = ExtractVertices(earSquare);
        headRegions.eyeHole = ExtractVertices(eyeHole);
        headRegions.jawLine = ExtractVertices(jawLine);
        headRegions.lowerLip = ExtractVertices(lowerLip);
        headRegions.mouth = ExtractVertices(mouth);
        headRegions.nose = ExtractVertices(nose);
        headRegions.noseTip = ExtractVertices(noseTip);
        headRegions.scalp = ExtractVertices(scalp);
        headRegions.upperLip = ExtractVertices(upperLip);
    }

    Vector3[] ExtractVertices(GameObject targetObj)
    {
        string objPath = AssetDatabase.GetAssetPath(targetObj);
        string[] lines = File.ReadAllLines(objPath);
        List<Vector3> vertices = new List<Vector3>();

        foreach (string line in lines)
        {
            Debug.Log(line);
            if (line.StartsWith("v "))
            {
                string[] parts = line.Split(' ');
                Vector3 point = new Vector3( float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                if(!vertices.Contains(point))
                vertices.Add(point);
            }
        }

        return vertices.ToArray();
    }
}