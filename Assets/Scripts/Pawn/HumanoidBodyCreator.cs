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

    enum LowerBodyType
    {
        Human,
        Lizard,
        Fish,
        Pentapus,
    }

    public override void CreateBody(out Transform atlas, out Transform cameraTransform)
    {
        CreateTorso(out atlas, out Transform leftHip, out Transform rightHip);
        CreateArms(atlas);
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
        GameObject go = new GameObject("Neck");
        go.layer = gameObject.layer;
        go.transform.SetParent(transform, false);
        GeneratedLimb upperNeck = go.AddComponent<GeneratedLimb>();
        upperNeck.transform.localRotation = Quaternion.LookRotation(Vector3.up, -Vector3.forward);
        upperNeck.length = upperNeckLength;
        upperNeck.startRadius = lowerNeckWidth;
        upperNeck.endRadius = upperNeckWidth;
        upperNeck.mat = new Material(basicInGameObject);
        upperNeck.startColor = skinColor;
        upperNeck.endColor = skinColor;
        upperNeck.Initialize();
        return upperNeck;
    }

    Transform CreateHead(GeneratedLimb upperNeck)
    {
        GameObject head = Instantiate(headPrefab);
        head.layer = gameObject.layer;
        head.transform.SetParent(upperNeck.transform, false);
        head.transform.localPosition = Vector3.forward * upperNeck.length;
        head.transform.localRotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
        MeshRenderer[] renderers = head.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.sharedMaterial = basicInGameObject;
        }

        return head.transform;
    }


    void CreateArms(Transform atlas)
    {
        GameObject go = new GameObject("LeftShoulder");
        go.layer = gameObject.layer;
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = Vector3.down * atlasLength - Vector3.right * torsoWidth;
        go.transform.localRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        HumanArm leftArm = go.AddComponent<HumanArm>();
        leftArm.Initialize(false, shoulderWidth, torsoDepth, shoulderSize, basicInGameObject, skinColor, armLength, elbowSize, forearmLength, wristSize);

        go = new GameObject("RightShoulder");
        go.layer = gameObject.layer;
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = Vector3.down * atlasLength + Vector3.right * torsoWidth;
        go.transform.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        HumanArm rightArm = go.AddComponent<HumanArm>();
        rightArm.Initialize(true, shoulderWidth, torsoDepth, shoulderSize, basicInGameObject, skinColor, armLength, elbowSize, forearmLength, wristSize);
    }

    void CreateLegs(Transform leftHip, Transform rightHip)
    {
        //Legs
        //------------------------------------------------------------------------------
        GameObject go = new GameObject("LeftThigh");
        go.layer = gameObject.layer;
        go.transform.SetParent(leftHip.transform, false);
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        HumanLeg leftLeg = go.AddComponent<HumanLeg>();
        leftLeg.Initialize(false, thighLength, lowerHipRadius, kneeRadius, upperCalfLength, calfRadius, lowerCalfLength, ankleRadius, basicInGameObject, skinColor);

        go = new GameObject("RightThigh");
        go.layer = gameObject.layer;
        go.transform.SetParent(rightHip.transform, false);
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-hipOutRotation, Vector3.up) * Vector3.forward, Vector3.up);
        HumanLeg rightLeg = go.AddComponent<HumanLeg>();
        rightLeg.Initialize(true, thighLength, lowerHipRadius, kneeRadius, upperCalfLength, calfRadius, lowerCalfLength, ankleRadius, basicInGameObject, skinColor);
        //------------------------------------------------------------------------------
    }

    void CreateTorso(out Transform atlasTransform, out Transform leftHip, out Transform rightHip)
    {
        //Torso
        //-----------------------------------------------------
        GameObject go = new GameObject("Atlas");
        go.layer = gameObject.layer;
        go.transform.SetParent(transform, false);
        HumanTorso atlas = go.AddComponent<HumanTorso>();
        atlasTransform = atlas.transform;

        (leftHip, rightHip) = atlas.Initialize(atlasLength, lowerNeckWidth, torsoDepth, torsoWidth, ribLength, bellyLength, waist, upperHipWidth, hipLength, hipOutRotation, upperHipRadius, lowerHipRadius, basicInGameObject, skinColor); ;
        ////------------------------------------------------------

        ////Arms
        ////----------------------------------------------------------------------------
        
        ////----------------------------------------------------------------------------

        //GameObject middleTorso = new GameObject("MiddleTorso");
        //middleTorso.layer = gameObject.layer;
        //middleTorso.transform.SetParent(atlas.transform, false);
        //middleTorso.transform.localRotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
        //middleTorso.transform.localPosition = Vector3.forward * (atlasLength + ribLength);
        //GameObject lowerTorso = new GameObject("LowerTorso");
        //lowerTorso.layer = gameObject.layer;
        //lowerTorso.transform.SetParent(middleTorso.transform, false);
        //lowerTorso.transform.localPosition = Vector3.down * (bellyLength);
    }
}
