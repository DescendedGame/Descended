using UnityEngine;

[System.Serializable]
public struct HumanHeadSettings
{
    //head
    public float skullSize;

    public float jawWidth;
    public float jawHeight;
    public float jawDepth;

    public float cheekboneWidth;
    public float cheekboneHeight;
    public float cheekSize;

    public float chinLength;
    public float chinWidth;

    //eyes
    public float eyeHeight;
    public float eyeDistance;
    public float eyeDepth;
    public float eyeSize;

    //brows
    public float outerBrow;
    public float innerBrow;
    public float browDistance;
    public float browDepth;

    //mouth
    public float mouthWidth;
    public float mouthHeight;
    public float lipSize;

    //nose
    public float noseWidth;
    public float noseHeight;
    public float noseDepth;

    //ears
    public float earRotation;
    public float earHeight;
    public float earSize;
}

[System.Serializable]
public struct HumanBodySettings
{
    public HumanHeadSettings headSettings;

    public float upperNeckLength;
    public float upperNeckWidth;
    public float lowerNeckWidth;

    public float atlasLength;
    public float torsoDepth;

    public float torsoWidth;
    public float ribLength;
    public float waist;
    public float bellyLength;
    public float shoulderWidth;
    public float shoulderSize;
    public float armLength;
    public float elbowSize;
    public float forearmLength;
    public float wristSize;

    public float upperHipWidth;
    public float upperHipRadius;
    public float lowerHipRadius;
    public float hipLength;
    public float hipOutRotation;

    public float thighLength;
    public float kneeRadius;
    public float upperCalfLength;
    public float calfRadius;
    public float lowerCalfLength;
    public float ankleRadius;

    public Color skinColor;
    public Material basicInGameObject;

    public HumanBodyCoverageSettings coverSettings;
}

[System.Serializable]
public struct HumanBodyCoverageSettings
{
    public Color color;
    public bool upperNeck;
    public bool lowerNeck;
    public bool shoulders;
    public bool elbows;
    public bool wrists;
    public bool chest;
    public bool waist;
    public bool hips;
    public bool butt;
    public bool knees;
    public bool calves;
    public bool ankles;
}

public class HumanoidBodyCreator : BodyCreator
{
    [SerializeField] GameObject headPrefab;

    public HumanBodySettings bodySettings;

    GameObject head;
    HumanHeadCreator headCreator;

    GeneratedLimb neck;

    Transform atlas;
    HumanTorso torso;


    HumanArm leftArm;
    HumanArm rightArm;

    HumanLeg leftLeg;
    HumanLeg rightLeg;


    enum LowerBodyType
    {
        Human,
        Lizard,
        Fish,
        Pentapus,
    }

    public void CreateHead()
    {
        headCreator.CreateHead(bodySettings.headSettings);
    }

    public override void RecalculateBody()
    {
        CreateBody(out Transform atlasTransform, out Transform cameraTransform);
    }

    public override void CreateBody(out Transform atlasTransform, out Transform cameraTransform)
    {
        CreateTorso(out Transform leftHip, out Transform rightHip);
        atlasTransform = atlas;
        CreateArms();
        CreateLegs(leftHip, rightHip);
        cameraTransform = CreateNeckAndHead();
        cameraTransform.parent.gameObject.AddComponent<HumanNeck>().Initialize(cameraTransform, atlas);
        atlas.GetComponent<HumanTorso>().head = cameraTransform;
    }

    Transform CreateNeckAndHead()
    {
        return PlaceHead(CreateUpperNeck());
    }

    GeneratedLimb CreateUpperNeck()
    {
        if(neck == null)
        {
            GameObject go = new GameObject("Neck");
            go.layer = gameObject.layer;
            go.transform.SetParent(transform, false);
            neck = go.AddComponent<GeneratedLimb>();
            neck.transform.localRotation = Quaternion.LookRotation(Vector3.up, -Vector3.forward);
        }
        neck.length = bodySettings.upperNeckLength;
        neck.startRadius = bodySettings.lowerNeckWidth;
        neck.endRadius = bodySettings.upperNeckWidth;
        neck.mat = new Material(bodySettings.basicInGameObject);
        neck.startColor = bodySettings.coverSettings.lowerNeck ? bodySettings.coverSettings.color : bodySettings.skinColor;
        neck.endColor = bodySettings.coverSettings.upperNeck ? bodySettings.coverSettings.color : bodySettings.skinColor;
        neck.Initialize();
        return neck;
    }

    Transform PlaceHead(GeneratedLimb upperNeck)
    {
        if(head == null)
        {
            head = Instantiate(headPrefab);
            headCreator = head.AddComponent<HumanHeadCreator>();
            head.layer = gameObject.layer;
            foreach (Transform child in head.transform)
            {
                child.gameObject.layer = gameObject.layer;
            }
            head.transform.SetParent(upperNeck.transform, false);
            head.transform.localRotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
            MeshRenderer[] renderers = head.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.sharedMaterial = bodySettings.basicInGameObject;
                Material thisOnesMat = new Material(renderer.material);
                thisOnesMat.SetColor("_MainColor", bodySettings.skinColor);
                thisOnesMat.SetColor("_TransitionColor", bodySettings.skinColor);
                renderer.material = thisOnesMat;
            }
            CreateHead();
        }
       
        head.transform.localPosition = Vector3.forward * upperNeck.length;

        return head.transform;
    }


    void CreateArms()
    {
        if(leftArm == null)
        {
            GameObject go = new GameObject("LeftShoulder");
            go.layer = gameObject.layer;
            go.transform.SetParent(atlas, false);
            leftArm = go.AddComponent<HumanArm>();
            leftArm.transform.localRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        }

        leftArm.transform.localPosition = Vector3.down * bodySettings.atlasLength - Vector3.right * bodySettings.torsoWidth;
        leftArm.Initialize(bodySettings, false);

        if(rightArm == null)
        {
            GameObject go = new GameObject("RightShoulder");
            go.layer = gameObject.layer;
            go.transform.SetParent(atlas, false);
            rightArm = go.AddComponent<HumanArm>();
            rightArm.transform.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        }

        rightArm.transform.localPosition = Vector3.down * bodySettings.atlasLength + Vector3.right * bodySettings.torsoWidth;
        rightArm.Initialize(bodySettings, true);
    }

    void CreateLegs(Transform leftHip, Transform rightHip)
    {
        if(leftLeg == null)
        {
            GameObject go = new GameObject("LeftThigh");
            go.layer = gameObject.layer;
            go.transform.SetParent(leftHip.transform, false);
            leftLeg = go.AddComponent<HumanLeg>();
            leftLeg.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(bodySettings.hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        }
        leftLeg.Initialize(bodySettings, false);

        if(rightLeg == null)
        {
            GameObject go = new GameObject("RightThigh");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightHip.transform, false);
            rightLeg = go.AddComponent<HumanLeg>();
            rightLeg.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-bodySettings.hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        }
        rightLeg.Initialize(bodySettings, true);
    }

    void CreateTorso(out Transform leftHip, out Transform rightHip)
    {
        if(atlas == null)
        {
            GameObject go = new GameObject("Atlas");
            go.layer = gameObject.layer;
            atlas = go.transform;
            atlas.SetParent(transform, false);
            torso = go.AddComponent<HumanTorso>();
        }
        (leftHip, rightHip) = torso.Initialize(bodySettings);
    }
}
