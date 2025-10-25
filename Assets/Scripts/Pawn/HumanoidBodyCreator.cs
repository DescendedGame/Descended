using UnityEngine;

public class HumanoidBodyCreator : BodyCreator
{
    public Material basicInGameObject;
    public GameObject headPrefab;

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

    GameObject head;
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
        return CreateHead(CreateUpperNeck());
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
        neck.length = upperNeckLength;
        neck.startRadius = lowerNeckWidth;
        neck.endRadius = upperNeckWidth;
        neck.mat = new Material(basicInGameObject);
        neck.startColor = skinColor;
        neck.endColor = skinColor;
        neck.Initialize();
        return neck;
    }

    Transform CreateHead(GeneratedLimb upperNeck)
    {
        if(head == null)
        {
            head = Instantiate(headPrefab);
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
                renderer.sharedMaterial = basicInGameObject;
            }
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

        leftArm.transform.localPosition = Vector3.down * atlasLength - Vector3.right * torsoWidth;
        leftArm.Initialize(this, false);

        if(rightArm == null)
        {
            GameObject go = new GameObject("RightShoulder");
            go.layer = gameObject.layer;
            go.transform.SetParent(atlas, false);
            rightArm = go.AddComponent<HumanArm>();
            rightArm.transform.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        }

        rightArm.transform.localPosition = Vector3.down * atlasLength + Vector3.right * torsoWidth;
        rightArm.Initialize(this, true);
    }

    void CreateLegs(Transform leftHip, Transform rightHip)
    {
        if(leftLeg == null)
        {
            GameObject go = new GameObject("LeftThigh");
            go.layer = gameObject.layer;
            go.transform.SetParent(leftHip.transform, false);
            leftLeg = go.AddComponent<HumanLeg>();
            leftLeg.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        }
        leftLeg.Initialize(this, false);

        if(rightLeg == null)
        {
            GameObject go = new GameObject("RightThigh");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightHip.transform, false);
            rightLeg = go.AddComponent<HumanLeg>();
            rightLeg.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        }
        rightLeg.Initialize(this, true);
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
        (leftHip, rightHip) = torso.Initialize(this);
    }
}
