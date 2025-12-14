using UnityEngine;

public class HumanHeadCreator : MonoBehaviour
{
    [SerializeField] GameObject eyeLid;
    [SerializeField] GameObject eyeWhite;
    [SerializeField] GameObject eyeIris;
    [SerializeField] GameObject eyePupil;

    GameObject leftEye;
    GameObject rightEye;

    MeshRenderer selectedHair;
    [SerializeField] GameObject[] hairs;

    MeshRenderer selectedBrow;
    [SerializeField] GameObject[] brows;

    MeshRenderer selectedSideBeard;
    [SerializeField] GameObject[] sideBeards;

    MeshRenderer selectedStache;
    [SerializeField] GameObject[] staches;

    MeshRenderer selectedBeard;
    [SerializeField] GameObject[] beards;

    HumanHeadSettings settings;

    PlasticMesh[] plasticMeshes;

    MeshRenderer[] eyeLids = new MeshRenderer[2];
    MeshRenderer[] sclera = new MeshRenderer[2];
    MeshRenderer[] irises = new MeshRenderer[2];
    MeshRenderer[] pupils = new MeshRenderer[2];
    [SerializeField] MeshRenderer makeup;
    [SerializeField] MeshRenderer lips;
    [SerializeField] MeshRenderer[] head;

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

        eyeLids[0] = Instantiate(eyeLid, leftEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(eyeLids[0].gameObject, gameObject.layer);
        sclera[0] = Instantiate(eyeWhite, leftEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(sclera[0].gameObject, gameObject.layer);
        irises[0] = Instantiate(eyeIris, leftEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(irises[0].gameObject, gameObject.layer);
        pupils[0] = Instantiate(eyePupil, leftEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(pupils[0].gameObject, gameObject.layer);

        eyeLids[1] = Instantiate(eyeLid, rightEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(eyeLids[1].gameObject, gameObject.layer);
        sclera[1] = Instantiate(eyeWhite, rightEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(sclera[1].gameObject, gameObject.layer);
        irises[1] = Instantiate(eyeIris, rightEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(irises[1].gameObject, gameObject.layer);
        pupils[1] = Instantiate(eyePupil, rightEye.transform).GetComponentInChildren<MeshRenderer>();
        SetGameLayerRecursive(pupils[1].gameObject, gameObject.layer);
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

    public void CreateHead(HumanBodySettings bodySettings)
    {
        plasticMeshes = GetComponentsInChildren<PlasticMesh>(true);
        settings = bodySettings.headSettings;

        foreach(MeshRenderer lid in eyeLids)
        {
            lid.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.eyeLidColor));
        }

        foreach (MeshRenderer white in sclera)
        {
            white.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.scleraColor));
        }

        foreach (MeshRenderer iris in irises)
        {
            iris.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.irisColor));
        }

        foreach (MeshRenderer pupil in pupils)
        {
            pupil.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.pupilColor));
        }

        for(int i = 0; i < hairs.Length; i++)
        {
            hairs[i].GetComponent<PlasticMesh>().Initialize();
            if(settings.hairStyle-1 == i)
            {
                hairs[i].SetActive(true);
                hairs[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", bodySettings.hairColor);
                hairs[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_TransitionColor", bodySettings.hairColor);
            }
            else
            {
                hairs[i].SetActive(false);
            }
            if (settings.browStyle - 1 == i)
            {
                brows[i].SetActive(true);
                brows[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", bodySettings.hairColor);
                brows[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_TransitionColor", bodySettings.hairColor);
            }
            else
            {
                brows[i].SetActive(false);
            }
            if (settings.sideBeardStyle - 1 == i)
            {
                sideBeards[i].SetActive(true);
                sideBeards[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", bodySettings.hairColor);
                sideBeards[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_TransitionColor", bodySettings.hairColor);
            }
            else
            {
                sideBeards[i].SetActive(false);
            }
            if (settings.stacheStyle - 1 == i)
            {
                staches[i].SetActive(true);
                staches[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", bodySettings.hairColor);
                staches[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_TransitionColor", bodySettings.hairColor);
            }
            else
            {
                staches[i].SetActive(false);
            }
            if (settings.beardStyle - 1 == i)
            {
                beards[i].SetActive(true);
                beards[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_MainColor", bodySettings.hairColor);
                beards[i].GetComponentInChildren<MeshRenderer>().material.SetColor("_TransitionColor", bodySettings.hairColor);
            }
            else
            {
                beards[i].SetActive(false);
            }
        }

        lips.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.lipColor));
        makeup.material.SetColor("_MainColor", RecalculateAlpha(bodySettings.skinColor, settings.makeupColor));
        foreach (MeshRenderer headMesh in head)
        {
            headMesh.material.SetColor("_MainColor", bodySettings.skinColor);
        }

        foreach (PlasticMesh plasticMesh in plasticMeshes)
        {
            plasticMesh.Initialize();
            plasticMesh.ResetPositions();

            plasticMesh.TransformVertexGroup("scalp", CalculateScalp);
            plasticMesh.TransformVertexGroup("browMiddle", CalculateBrow);
            plasticMesh.TransformVertexGroup("browInner", CalculateBrow);
            plasticMesh.TransformVertexGroup("browOuter", CalculateBrow);
            plasticMesh.TransformVertexGroup("temple", NoCalculation);
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

    Vector3 NoCalculation(Vector3 vertex)
    {
        return vertex;
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

    Color RecalculateAlpha(Color skinColor, Color color)
    {
        return Color.Lerp(skinColor, new Color(color.r, color.g, color.b, 1), color.a);
    }
}
