using System.Collections.Generic;
using UnityEngine;

public class HumanHeadCreator : MonoBehaviour
{
    [SerializeField] GameObject eyeLid;
    [SerializeField] GameObject eyeWhite;
    [SerializeField] GameObject eyeIris;
    [SerializeField] GameObject eyePupil;

    GameObject leftEye;
    GameObject rightEye;

    GameObject hair;
    [SerializeField] GameObject hair1;
    [SerializeField] GameObject hair2;
    [SerializeField] GameObject hair3;
    [SerializeField] GameObject hair4;

    GameObject brows;
    [SerializeField] GameObject brows1;
    [SerializeField] GameObject brows2;
    [SerializeField] GameObject brows3;
    [SerializeField] GameObject brows4;

    GameObject sideBeard;
    [SerializeField] GameObject sideBeard1;
    [SerializeField] GameObject sideBeard2;
    [SerializeField] GameObject sideBeard3;
    [SerializeField] GameObject sideBeard4;

    GameObject stache;
    [SerializeField] GameObject stache1;
    [SerializeField] GameObject stache2;
    [SerializeField] GameObject stache3;
    [SerializeField] GameObject stache4;

    GameObject beard;
    [SerializeField] GameObject beard1;
    [SerializeField] GameObject beard2;
    [SerializeField] GameObject beard3;
    [SerializeField] GameObject beard4;

    HumanHeadSettings settings;

    PlasticMesh[] plasticMeshes;

    private void Awake()
    {
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

    public void CreateHead(HumanHeadSettings headSettings)
    {
        plasticMeshes = GetComponentsInChildren<PlasticMesh>();
        settings = headSettings;

        foreach(PlasticMesh plasticMesh in plasticMeshes)
        {
            plasticMesh.ResetPositions();

            plasticMesh.TransformVertexGroup("scalp", CalculateScalp);
            plasticMesh.TransformVertexGroup("browMiddle", CalculateBrow);
            plasticMesh.TransformVertexGroup("browInner", CalculateBrow);
            plasticMesh.TransformVertexGroup("browOuter", CalculateBrow);
            plasticMesh.TransformVertexGroup("temple", CalculateBrow);
            plasticMesh.TransformVertexGroup("eyeHole", CalculateEyes);
            plasticMesh.TransformVertexGroup("nose", CalculateNose);
            plasticMesh.TransformVertexGroup("noseTip", CalculateNose);
            plasticMesh.TransformVertexGroup("cheekBoneRear", CalculateCheekBones);
            plasticMesh.TransformVertexGroup("cheekBoneMiddle", CalculateCheekBones);
            plasticMesh.TransformVertexGroup("cheekBoneFront", CalculateCheekBones);
            plasticMesh.TransformVertexGroup("cheekUpper", CalculateCheek);
            plasticMesh.TransformVertexGroup("cheekLower", CalculateCheek);
            plasticMesh.TransformVertexGroup("earSquare", CalculateEars);
            plasticMesh.TransformVertexGroup("jawTop", CalculateJaw);
            plasticMesh.TransformVertexGroup("jawCorner", CalculateJaw);
            plasticMesh.TransformVertexGroup("mouth", CalculateMouth);
            plasticMesh.TransformVertexGroup("upperLip", CalculateLips);
            plasticMesh.TransformVertexGroup("lowerLip", CalculateLips);
            plasticMesh.TransformVertexGroup("chin", CalculateChin);

            plasticMesh.RecalculateMesh();
        }
    }

    Vector3 CalculateScalp(Vector3 vertex)
    {
        return vertex * settings.skullSize;
    }

    Vector3 CalculateBrow(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.browDistance,
                vertex.y,
                vertex.z + settings.browDepth
            );
    }

    Vector3 CalculateEyes(Vector3 vertex)
    {
         return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.eyeDistance,
                vertex.y + settings.eyeHeight,
                vertex.z + settings.eyeDepth
            );
    }

    Vector3 CalculateNose(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.noseWidth,
                vertex.y + settings.noseHeight,
                vertex.z + settings.noseDepth
            );
    }

    Vector3 CalculateCheekBones(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.cheekboneWidth,
                vertex.y + settings.cheekboneHeight,
                vertex.z
            );
    }

    Vector3 CalculateCheek(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.cheekSize,
                vertex.y,
                vertex.z + settings.cheekSize
            );
    }

    Vector3 CalculateEars(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x,
                vertex.y + settings.earHeight,
                vertex.z
            );
    }

    Vector3 CalculateJaw(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x)*settings.jawWidth,
                vertex.y + settings.jawHeight,
                vertex.z + settings.jawDepth
            );
    }

    Vector3 CalculateMouth(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.mouthWidth,
                vertex.y + settings.mouthHeight,
                vertex.z + settings.lipSize
            );
    }

    Vector3 CalculateLips(Vector3 vertex)
    {
            return new Vector3
            (
                vertex.x + settings.mouthWidth,
                vertex.y + settings.mouthHeight,
                vertex.z + settings.lipSize
            );
    }

    Vector3 CalculateChin(Vector3 vertex)
    {
        return new Vector3
            (
                vertex.x + Sign(vertex.x) * settings.chinWidth,
                vertex.y - settings.chinLength,
                vertex.z + settings.chinLength
            );
    }

    float Sign(float value)
    {
        return value == 0 ? 0 : Mathf.Sign(value);
    }
}
