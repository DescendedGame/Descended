using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VertexExtractor : MonoBehaviour
{
    public bool button;
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
        HeadCustomizationRegions dataHolder = new GameObject().AddComponent<HeadCustomizationRegions>();
        dataHolder.brow = ExtractVertices(brow);
        dataHolder.cheek = ExtractVertices(cheek);
        dataHolder.cheekBone = ExtractVertices(cheekBone);
        dataHolder.chinTip = ExtractVertices(chinTip);
        dataHolder.earSquare = ExtractVertices(earSquare);
        dataHolder.eyeHole = ExtractVertices(eyeHole);
        dataHolder.jawLine = ExtractVertices(jawLine);
        dataHolder.lowerLip = ExtractVertices(lowerLip);
        dataHolder.mouth = ExtractVertices(mouth);
        dataHolder.nose = ExtractVertices(nose);
        dataHolder.noseTip = ExtractVertices(noseTip);
        dataHolder.scalp = ExtractVertices(scalp);
        dataHolder.upperLip = ExtractVertices(upperLip);
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