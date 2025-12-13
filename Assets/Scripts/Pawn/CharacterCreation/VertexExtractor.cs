using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VertexExtractor : MonoBehaviour
{
    public bool trigger;

    public HumanHeadRegions headRegions;
    public GameObject scalp;
    public GameObject browMiddle;
    public GameObject browInner;
    public GameObject browOuter;
    public GameObject temple;
    public GameObject eyeHole;
    public GameObject nose;
    public GameObject noseTip;
    public GameObject cheekBoneRear;
    public GameObject cheekBoneMiddle;
    public GameObject cheekBoneFront;
    public GameObject cheekUpper;
    public GameObject cheekLower;
    public GameObject earSquare;
    public GameObject jawTop;
    public GameObject jawCorner;
    public GameObject mouth;
    public GameObject upperLip;
    public GameObject lowerLip;
    public GameObject chin;

    void Update()
    {
        if(trigger)
        {
            headRegions.scalp = ExtractVertices(scalp);
            headRegions.browMiddle = ExtractVertices(browMiddle);
            headRegions.browInner = ExtractVertices(browInner);
            headRegions.browOuter = ExtractVertices(browOuter);
            headRegions.temple = ExtractVertices(temple);
            headRegions.eyeHole = ExtractVertices(eyeHole);
            headRegions.nose = ExtractVertices(nose);
            headRegions.noseTip = ExtractVertices(noseTip);
            headRegions.cheekBoneRear = ExtractVertices(cheekBoneRear);
            headRegions.cheekBoneMiddle = ExtractVertices(cheekBoneMiddle);
            headRegions.cheekBoneFront = ExtractVertices(cheekBoneFront);
            headRegions.cheekUpper = ExtractVertices(cheekUpper);
            headRegions.cheekLower = ExtractVertices(cheekLower);
            headRegions.earSquare = ExtractVertices(earSquare);
            headRegions.jawTop = ExtractVertices(jawTop);
            headRegions.jawCorner = ExtractVertices(jawCorner);
            headRegions.mouth = ExtractVertices(mouth);
            headRegions.upperLip = ExtractVertices(upperLip);
            headRegions.lowerLip = ExtractVertices(lowerLip);
            headRegions.chin = ExtractVertices(chin);
            trigger = false;
        }
    }

    Vector3[] ExtractVertices(GameObject targetObj)
    {
        if (!targetObj) return new Vector3[0];
        string objPath = AssetDatabase.GetAssetPath(targetObj);
        string[] lines = File.ReadAllLines(objPath);
        List<Vector3> vertices = new List<Vector3>();

        foreach (string line in lines)
        {
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