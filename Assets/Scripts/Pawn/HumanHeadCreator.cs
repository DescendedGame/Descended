using System.Collections.Generic;
using UnityEngine;

public class HumanHeadCreator : MonoBehaviour
{
    [SerializeField] HumanHeadRegions headRegions;

    Vector3[][] originalVertices;
    MeshFilter[] allFilters;

    Dictionary<Vector3, Vector3> updatedPositions = new Dictionary<Vector3, Vector3>();

    [SerializeField] GameObject eyeLid;
    [SerializeField] GameObject eyeWhite;
    [SerializeField] GameObject eyeIris;
    [SerializeField] GameObject eyePupil;

    GameObject leftEye;
    GameObject rightEye;

    private void Awake()
    {
        allFilters = GetComponentsInChildren<MeshFilter>();
        originalVertices = new Vector3[allFilters.Length][];
        for (int i = 0; i < allFilters.Length; i++)
        {
            originalVertices[i] = allFilters[i].mesh.vertices;
        }

        leftEye = new GameObject("LeftEye");
        leftEye.layer = gameObject.layer;
        leftEye.transform.SetParent(transform, false);
        leftEye.transform.localPosition = new Vector3(-0.0389840007f, 0.0870779976f, 0.129999995f);

        rightEye = new GameObject("RightEye");
        rightEye.layer = gameObject.layer;
        rightEye.transform.SetParent(transform, false);
        rightEye.transform.localPosition = new Vector3(0.0389840007f, 0.0870779976f, 0.129999995f);

        SetGameLayerRecursive(Instantiate(eyeLid, leftEye.transform),gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyeWhite, leftEye.transform), gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyeIris, leftEye.transform), gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyePupil, leftEye.transform), gameObject.layer);

        SetGameLayerRecursive(Instantiate(eyeLid, rightEye.transform), gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyeWhite, rightEye.transform), gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyeIris, rightEye.transform), gameObject.layer);
        SetGameLayerRecursive(Instantiate(eyePupil, rightEye.transform), gameObject.layer);
    }

    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, layer);
        }
    }

    public void CreateHead(HumanHeadSettings settings)
    {
        updatedPositions.Clear();

        foreach (Vector3[] vertices in originalVertices)
        {
            foreach (Vector3 vertex in vertices)
            {
                updatedPositions[vertex] = vertex;
            }
        }

        CalculateScalp(settings);
        CalculateJaw(settings);
        CalculateCheekBones(settings);
        CalculateCheek(settings);
        CalculateChin(settings);
        CalculateEyes(settings);
        CalculateBrow(settings);
        CalculateMouth(settings);
        CalculateLips(settings);
        CalculateNose(settings);
        CalculateEars(settings);

        for (int i = 0; i < allFilters.Length; i++)
        {
            Vector3[] vertices = new Vector3[originalVertices[i].Length];
            for (int j = 0; j < vertices.Length; j++)
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

    void CalculateScalp(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.scalp)
        {
            updatedPositions[vertex] = updatedPositions[vertex] * settings.skullSize;
        }
    }

    void CalculateJaw(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.jawTop)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x)*settings.jawWidth,
                updatedPositions[vertex].y + settings.jawHeight,
                updatedPositions[vertex].z + settings.jawDepth
            );
        }
        foreach (Vector3 vertex in headRegions.jawCorner)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.jawWidth,
                updatedPositions[vertex].y + settings.jawHeight,
                updatedPositions[vertex].z + settings.jawDepth
            );
        }
    }

    void CalculateCheekBones(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.cheekBoneRear)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.cheekboneWidth,
                updatedPositions[vertex].y + settings.cheekboneHeight,
                updatedPositions[vertex].z
            );
        }
        foreach (Vector3 vertex in headRegions.cheekBoneMiddle)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.cheekboneWidth,
                updatedPositions[vertex].y + settings.cheekboneHeight,
                updatedPositions[vertex].z
            );
        }
        foreach (Vector3 vertex in headRegions.cheekBoneFront)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.cheekboneWidth,
                updatedPositions[vertex].y + settings.cheekboneHeight,
                updatedPositions[vertex].z
            );
        }
    }

    void CalculateCheek(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.cheekUpper)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.cheekSize,
                updatedPositions[vertex].y,
                updatedPositions[vertex].z + settings.cheekSize
            );
        }
        foreach (Vector3 vertex in headRegions.cheekLower)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.cheekSize,
                updatedPositions[vertex].y,
                updatedPositions[vertex].z + settings.cheekSize
            );
        }
    }

    void CalculateChin(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.chin)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.chinWidth,
                updatedPositions[vertex].y - settings.chinLength,
                updatedPositions[vertex].z + settings.chinLength
            );
        }
    }

    void CalculateEyes(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.eyeHole)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.eyeDistance,
                updatedPositions[vertex].y + settings.eyeHeight,
                updatedPositions[vertex].z + settings.eyeDepth
            );
        }
    }

    void CalculateBrow(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.browMiddle)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.browDistance,
                updatedPositions[vertex].y,
                updatedPositions[vertex].z + settings.browDepth
            );
        }
        foreach (Vector3 vertex in headRegions.browInner)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.browDistance,
                updatedPositions[vertex].y,
                updatedPositions[vertex].z + settings.browDepth
            );
        }
        foreach (Vector3 vertex in headRegions.browOuter)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.browDistance,
                updatedPositions[vertex].y,
                updatedPositions[vertex].z + settings.browDepth
            );
        }
    }

    void CalculateMouth(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.mouth)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x)*settings.mouthWidth,
                updatedPositions[vertex].y + settings.mouthHeight,
                updatedPositions[vertex].z + settings.lipSize
            );
        }
    }

    void CalculateLips(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.upperLip)
        {
            //updatedPositions[vertex] = new Vector3
            //(
            //    updatedPositions[vertex].x + settings.mouthWidth,
            //    updatedPositions[vertex].y + settings.mouthHeight,
            //    updatedPositions[vertex].z + settings.lipSize);

            //if (vertex.x != 0)
            //{
            //    Vector3 mirroredVertex = new Vector3(-vertex.x, vertex.y, vertex.z);
            //    updatedPositions[mirroredVertex] = new Vector3
            //    (
            //        updatedPositions[mirroredVertex].x - settings.mouthWidth,
            //        updatedPositions[mirroredVertex].y + settings.mouthHeight,
            //        updatedPositions[mirroredVertex].z + settings.lipSize
            //    );
            //}
        }
        foreach (Vector3 vertex in headRegions.lowerLip)
        {
            //updatedPositions[vertex] = new Vector3
            //(
            //    updatedPositions[vertex].x + settings.mouthWidth,
            //    updatedPositions[vertex].y + settings.mouthHeight,
            //    updatedPositions[vertex].z + settings.lipSize);

            //if (vertex.x != 0)
            //{
            //    Vector3 mirroredVertex = new Vector3(-vertex.x, vertex.y, vertex.z);
            //    updatedPositions[mirroredVertex] = new Vector3
            //    (
            //        updatedPositions[mirroredVertex].x - settings.mouthWidth,
            //        updatedPositions[mirroredVertex].y + settings.mouthHeight,
            //        updatedPositions[mirroredVertex].z + settings.lipSize
            //    );
            //}
        }
    }

    void CalculateNose(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.nose)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.noseWidth,
                updatedPositions[vertex].y + settings.noseHeight,
                updatedPositions[vertex].z + settings.noseDepth
            );
        }
        foreach (Vector3 vertex in headRegions.noseTip)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x + Sign(vertex.x) * settings.noseWidth,
                updatedPositions[vertex].y + settings.noseHeight,
                updatedPositions[vertex].z + settings.noseDepth
            );
        }
    }

    void CalculateEars(HumanHeadSettings settings)
    {
        foreach (Vector3 vertex in headRegions.earSquare)
        {
            updatedPositions[vertex] = new Vector3
            (
                updatedPositions[vertex].x,
                updatedPositions[vertex].y + settings.earHeight,
                updatedPositions[vertex].z
            );
        }
    }

    float Sign(float value)
    {
        return value == 0 ? 0 : Mathf.Sign(value);
    }
}
