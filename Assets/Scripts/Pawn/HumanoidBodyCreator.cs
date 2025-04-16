using UnityEngine;

public class HumanoidBodyCreator : MonoBehaviour
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

    private void Awake()
    {
        CreateNeckAndHead();
        CreateUpperTorso();
    }

    void CreateNeckAndHead()
    {
        CreateHead(CreateUpperNeck());
    }

    GeneratedLimb CreateUpperNeck()
    {
        GameObject go = new GameObject("Neck");
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
        head.transform.SetParent(upperNeck.transform, false);
        head.transform.localPosition = Vector3.forward * upperNeck.length;
        head.transform.localRotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
        MeshRenderer[] renderers = head.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in renderers)
        {
            renderer.sharedMaterial = basicInGameObject;
        }

        return head.transform;
    }

    Transform CreateUpperTorso()
    {
        CreateTorso(CreateAtlas());
        return null;
    }

    GeneratedLimb CreateAtlas()
    {
        GameObject go = new GameObject("Atlas");
        go.transform.SetParent(transform, false);
        GeneratedLimb atlas = go.AddComponent<GeneratedLimb>();
        atlas.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        atlas.length = atlasLength;
        atlas.startRadius = lowerNeckWidth;
        atlas.endRadius = torsoDepth;
        atlas.mat = new Material(basicInGameObject);
        atlas.startColor = skinColor;
        atlas.endColor = skinColor;
        atlas.Initialize();
        return atlas;
    }

    Vector3 CreateTorso(GeneratedLimb atlas)
    {
        Vector3 atlasEnd = atlas.LastPoint - Vector3.forward * atlas.endRadius;

        //Ribs
        //---------------------------------------------------------------------------------------------------
        GameObject go = new GameObject("LeftRibs");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd - Vector3.right * torsoWidth;
        GeneratedLimb leftRib = go.AddComponent<GeneratedLimb>();
        leftRib.length = ribLength;
        leftRib.startRadius = torsoDepth;
        leftRib.endRadius = waist;
        leftRib.mat = new Material(basicInGameObject);
        leftRib.startColor = skinColor;
        leftRib.endColor = skinColor;
        leftRib.Initialize();

        go = new GameObject("RightRibs");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd + Vector3.right * torsoWidth;
        GeneratedLimb rightRib = go.AddComponent<GeneratedLimb>();
        rightRib.length = ribLength;
        rightRib.startRadius = torsoDepth;
        rightRib.endRadius = waist;
        rightRib.mat = new Material(basicInGameObject);
        rightRib.startColor = skinColor;
        rightRib.endColor = skinColor;
        rightRib.Initialize();

        go = new GameObject("RibSkin");
        go.transform.SetParent(rightRib.transform, false);
        BodyStitcher sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftRib;
        sticher.rightSide = rightRib;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();
        //----------------------------------------------------------------------------

        //Shoulders and arms
        //----------------------------------------------------------------------------
        go = new GameObject("LeftShoulder");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd - Vector3.right * torsoWidth;
        go.transform.localRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        GeneratedLimb leftShoulder = go.AddComponent<GeneratedLimb>();
        leftShoulder.length = shoulderWidth;
        leftShoulder.startRadius = torsoDepth;
        leftShoulder.endRadius = shoulderSize;
        leftShoulder.mat = new Material(basicInGameObject);
        leftShoulder.startColor = skinColor;
        leftShoulder.endColor = skinColor;
        leftShoulder.Initialize();

        go = new GameObject("RightShoulder");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd + Vector3.right * torsoWidth;
        go.transform.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        GeneratedLimb rightShoulder = go.AddComponent<GeneratedLimb>();
        rightShoulder.length = shoulderWidth;
        rightShoulder.startRadius = torsoDepth;
        rightShoulder.endRadius = shoulderSize;
        rightShoulder.mat = new Material(basicInGameObject);
        rightShoulder.startColor = skinColor;
        rightShoulder.endColor = skinColor;
        rightShoulder.Initialize();

        go = new GameObject("LeftArm");
        go.transform.SetParent(leftShoulder.transform, false);
        GeneratedLimb leftArm = go.AddComponent<GeneratedLimb>();
        leftArm.snapToParent = true;
        leftArm.length = armLength;
        leftArm.startRadius = shoulderSize;
        leftArm.endRadius = elbowSize;
        leftArm.mat = new Material(basicInGameObject);
        leftArm.startColor = skinColor;
        leftArm.endColor = skinColor;
        leftArm.Initialize();

        go = new GameObject("RightArm");
        go.transform.SetParent(rightShoulder.transform, false);
        GeneratedLimb rightArm = go.AddComponent<GeneratedLimb>();
        rightArm.snapToParent = true;
        rightArm.length = armLength;
        rightArm.startRadius = shoulderSize;
        rightArm.endRadius = elbowSize;
        rightArm.mat = new Material(basicInGameObject);
        rightArm.startColor = skinColor;
        rightArm.endColor = skinColor;
        rightArm.Initialize();

        go = new GameObject("LeftForearm");
        go.transform.SetParent(leftArm.transform, false);
        GeneratedLimb leftForearm = go.AddComponent<GeneratedLimb>();
        leftForearm.snapToParent = true;
        leftForearm.length = forearmLength;
        leftForearm.startRadius = elbowSize;
        leftForearm.endRadius = wristSize;
        leftForearm.mat = new Material(basicInGameObject);
        leftForearm.startColor = skinColor;
        leftForearm.endColor = skinColor;
        leftForearm.Initialize();

        go = new GameObject("RightForearm");
        go.transform.SetParent(rightArm.transform, false);
        GeneratedLimb rightForearm = go.AddComponent<GeneratedLimb>();
        rightForearm.snapToParent = true;
        rightForearm.length = forearmLength;
        rightForearm.startRadius = elbowSize;
        rightForearm.endRadius = wristSize;
        rightForearm.mat = new Material(basicInGameObject);
        rightForearm.startColor = skinColor;
        rightForearm.endColor = skinColor;
        rightForearm.Initialize();

        //----------------------------------------------------------------------------

        GameObject middleTorso = new GameObject("MiddleTorso");
        middleTorso.transform.SetParent(atlas.transform,false);
        middleTorso.transform.localRotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
        middleTorso.transform.localPosition = Vector3.forward * (atlasLength + ribLength);
        GameObject lowerTorso = new GameObject("LowerTorso");
        lowerTorso.transform.SetParent(middleTorso.transform, false);
        lowerTorso.transform.localPosition = Vector3.down * (bellyLength);

        //Hips
        //----------------------------------------------------------------------------
        //The hips will depend on what kind of lower body is desired.... but now just human.

        go = new GameObject("LeftHip");
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = - Vector3.right * upperHipWidth;
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        GeneratedLimb leftHip = go.AddComponent<GeneratedLimb>();
        leftHip.length = hipLength;
        leftHip.startRadius = upperHipRadius;
        leftHip.endRadius = lowerHipRadius;
        leftHip.mat = new Material(basicInGameObject);
        leftHip.startColor = skinColor;
        leftHip.endColor = skinColor;
        leftHip.Initialize();

        go = new GameObject("RightHip");
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = Vector3.right * upperHipWidth;
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(hipOutRotation, Vector3.forward)*Vector3.down, Vector3.forward);
        GeneratedLimb rightHip = go.AddComponent<GeneratedLimb>();
        rightHip.length = hipLength;
        rightHip.startRadius = upperHipRadius;
        rightHip.endRadius = lowerHipRadius;
        rightHip.mat = new Material(basicInGameObject);
        rightHip.startColor = skinColor;
        rightHip.endColor = skinColor;
        rightHip.Initialize();

        go = new GameObject("HipSkin");
        go.transform.SetParent(rightHip.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftHip;
        sticher.rightSide = rightHip;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        //--------------------------------------------------------------------------------

        //Legs
        //------------------------------------------------------------------------------
        go = new GameObject("LeftThigh");
        go.transform.SetParent(leftHip.transform, false);
        GeneratedLimb leftThigh = go.AddComponent<GeneratedLimb>();
        leftThigh.snapToParent = true;
        leftThigh.length = thighLength;
        leftThigh.startRadius = lowerHipRadius;
        leftThigh.endRadius = kneeRadius;
        leftThigh.mat = new Material(basicInGameObject);
        leftThigh.startColor = skinColor;
        leftThigh.endColor = skinColor;
        leftThigh.Initialize();

        go = new GameObject("RightThigh");
        go.transform.SetParent(rightHip.transform, false);
        GeneratedLimb rightThigh = go.AddComponent<GeneratedLimb>();
        rightThigh.snapToParent = true;
        rightThigh.length = thighLength;
        rightThigh.startRadius = lowerHipRadius;
        rightThigh.endRadius = kneeRadius;
        rightThigh.mat = new Material(basicInGameObject);
        rightThigh.startColor = skinColor;
        rightThigh.endColor = skinColor;
        rightThigh.Initialize();

        go = new GameObject("LeftUpperCalf");
        go.transform.SetParent(leftThigh.transform, false);
        GeneratedLimb leftUpperCalf = go.AddComponent<GeneratedLimb>();
        leftUpperCalf.snapToParent = true;
        leftUpperCalf.length = upperCalfLength;
        leftUpperCalf.startRadius = kneeRadius;
        leftUpperCalf.endRadius = calfRadius;
        leftUpperCalf.mat = new Material(basicInGameObject);
        leftUpperCalf.startColor = skinColor;
        leftUpperCalf.endColor = skinColor;
        leftUpperCalf.Initialize();

        go = new GameObject("RightUpperCalf");
        go.transform.SetParent(rightThigh.transform, false);
        GeneratedLimb rightUpperCalf = go.AddComponent<GeneratedLimb>();
        rightUpperCalf.snapToParent = true;
        rightUpperCalf.length = upperCalfLength;
        rightUpperCalf.startRadius = kneeRadius;
        rightUpperCalf.endRadius = calfRadius;
        rightUpperCalf.mat = new Material(basicInGameObject);
        rightUpperCalf.startColor = skinColor;
        rightUpperCalf.endColor = skinColor;
        rightUpperCalf.Initialize();

        go = new GameObject("LeftLowerCalf");
        go.transform.SetParent(leftUpperCalf.transform, false);
        GeneratedLimb leftLowerCalf = go.AddComponent<GeneratedLimb>();
        leftLowerCalf.snapToParent = true;
        leftLowerCalf.length = lowerCalfLength;
        leftLowerCalf.startRadius = calfRadius;
        leftLowerCalf.endRadius = ankleRadius;
        leftLowerCalf.mat = new Material(basicInGameObject);
        leftLowerCalf.startColor = skinColor;
        leftLowerCalf.endColor = skinColor;
        leftLowerCalf.Initialize();

        go = new GameObject("RightLowerCalf");
        go.transform.SetParent(rightUpperCalf.transform, false);
        GeneratedLimb rightLowerCalf = go.AddComponent<GeneratedLimb>();
        rightLowerCalf.snapToParent = true;
        rightLowerCalf.length = lowerCalfLength;
        rightLowerCalf.startRadius = calfRadius;
        rightLowerCalf.endRadius = ankleRadius;
        rightLowerCalf.mat = new Material(basicInGameObject);
        rightLowerCalf.startColor = skinColor;
        rightLowerCalf.endColor = skinColor;
        rightLowerCalf.Initialize();

        //------------------------------------------------------------------------------

        // Connect rib and hip with belly:
        //--------------------------------------------------------------------

        go = new GameObject("LeftBelly");
        go.transform.SetParent(leftRib.transform, false);
        GeneratedLimb leftBelly = go.AddComponent<GeneratedLimb>();
        leftBelly.snapToParent = true;
        leftBelly.startRadius = waist;
        leftBelly.endRadius = upperHipRadius;
        leftBelly.target = leftHip.transform;
        leftBelly.mat = new Material(basicInGameObject);
        leftBelly.startColor = skinColor;
        leftBelly.endColor = skinColor;
        leftBelly.Initialize();
        leftBelly.UpdateVertices();

        go = new GameObject("RightBelly");
        go.transform.SetParent(rightRib.transform, false);
        GeneratedLimb rightBelly = go.AddComponent<GeneratedLimb>();
        rightBelly.snapToParent = true;
        rightBelly.startRadius = waist;
        rightBelly.endRadius = upperHipRadius;
        rightBelly.target = rightHip.transform;
        rightBelly.mat = new Material(basicInGameObject);
        rightBelly.startColor = skinColor;
        rightBelly.endColor = skinColor;
        rightBelly.Initialize();
        rightBelly.UpdateVertices();

        go = new GameObject("BellySkin");
        go.transform.SetParent(rightBelly.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftBelly;
        sticher.rightSide = rightBelly;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        return atlasEnd + atlas.transform.forward * ribLength;
    }

    Transform CreateMiddleTorso()
    {
        return null;
    }

    Transform CreateLowerTorso()
    {
        return null;
    }
}
