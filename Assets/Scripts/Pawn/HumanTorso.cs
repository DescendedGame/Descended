using UnityEngine;

public class HumanTorso : BodyPart
{
    public bool isRight = true;

    Quaternion targetRotation = Quaternion.identity;
    public Transform head;
    [SerializeField] Transform upperTorso;
    [SerializeField] Transform middleTorso;
    [SerializeField] Transform lowerTorso;

    /// <summary>
    /// Returns left and right slots to be used for leg placement.
    /// </summary>
    /// <returns></returns>
    public (Transform, Transform) Initialize(float atlasLength, float lowerNeckWidth, float torsoDepth, float torsoWidth,
        float ribLength, float bellyLength, float waist, float upperHipWidth, float hipLength, float hipOutRotation, float upperHipRadius, float lowerHipRadius, Material basicInGameObject, Color skinColor)
    {
        upperTorso = transform;
        GameObject go = new GameObject("LowerNeck");
        go.layer = gameObject.layer;
        go.transform.SetParent(transform, false);
        GeneratedLimb lowerNeck = go.AddComponent<GeneratedLimb>();
        lowerNeck.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        lowerNeck.length = atlasLength;
        lowerNeck.startRadius = lowerNeckWidth;
        lowerNeck.endRadius = torsoDepth;
        lowerNeck.mat = new Material(basicInGameObject);
        lowerNeck.startColor = skinColor;
        lowerNeck.endColor = skinColor;
        lowerNeck.Initialize();

        Vector3 atlasEnd = Vector3.forward * atlasLength;

        middleTorso = new GameObject("MiddleTorso").transform;
        middleTorso.gameObject.layer = gameObject.layer;
        middleTorso.transform.SetParent(transform.transform, false);
        middleTorso.transform.localPosition = Vector3.down * (atlasLength + ribLength);
        lowerTorso = new GameObject("LowerTorso").transform;
        lowerTorso.transform.SetParent(middleTorso.transform, false);
        lowerTorso.transform.localPosition = Vector3.down * (bellyLength);

        //Ribs
        //---------------------------------------------------------------------------------------------------
        go = new GameObject("LeftRibs");
        go.layer = gameObject.layer;
        go.transform.SetParent(lowerNeck.transform, false);
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
        go.layer = gameObject.layer;
        go.transform.SetParent(lowerNeck.transform, false);
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
        go.layer = gameObject.layer;
        go.transform.SetParent(rightRib.transform, false);
        BodyStitcher sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftRib;
        sticher.rightSide = rightRib;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();
        //----------------------------------------------------------------------------

        //Hips
        //----------------------------------------------------------------------------
        //The hips will depend on what kind of lower body is desired.... but now just human.

        go = new GameObject("LeftHip");
        go.layer = gameObject.layer;
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = -Vector3.right * upperHipWidth;
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
        go.layer = gameObject.layer;
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = Vector3.right * upperHipWidth;
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        GeneratedLimb rightHip = go.AddComponent<GeneratedLimb>();
        rightHip.length = hipLength;
        rightHip.startRadius = upperHipRadius;
        rightHip.endRadius = lowerHipRadius;
        rightHip.mat = new Material(basicInGameObject);
        rightHip.startColor = skinColor;
        rightHip.endColor = skinColor;
        rightHip.Initialize();

        go = new GameObject("HipSkin");
        go.layer = gameObject.layer;
        go.transform.SetParent(rightHip.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftHip;
        sticher.rightSide = rightHip;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        //--------------------------------------------------------------------------------
        // Connect rib and hip with belly:
        //--------------------------------------------------------------------

        go = new GameObject("LeftBelly");
        go.layer = gameObject.layer;
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
        go.layer = gameObject.layer;
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
        go.layer = gameObject.layer;
        go.transform.SetParent(rightBelly.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftBelly;
        sticher.rightSide = rightBelly;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        //--------------------------------------------------------------------

        return (leftHip.transform, rightHip.transform);
    }

    public override void Idle()
    {
        float angleToHead = Quaternion.Angle(targetRotation, head.rotation);
        if (angleToHead > 45) targetRotation = Quaternion.RotateTowards(targetRotation, head.rotation, angleToHead - 45);

        float upperRotationSin = WaveVariables.sinTimeRushQuarter;
        float middleRotationSin = WaveVariables.sinTime;
        float lowerRotationSin = -WaveVariables.sinTimeRushQuarter;

        //upperTorso.rotation = Quaternion.RotateTowards(upperTorso.rotation, targetRotation, Time.deltaTime * 360);
        upperTorso.rotation = Quaternion.RotateTowards(upperTorso.rotation, targetRotation*Quaternion.AngleAxis(10 * upperRotationSin, Vector3.right), Time.deltaTime * 360);
        middleTorso.localRotation = Quaternion.RotateTowards(middleTorso.localRotation, Quaternion.AngleAxis(10 * middleRotationSin, Vector3.right), Time.deltaTime * 360);
        lowerTorso.localRotation = Quaternion.RotateTowards(lowerTorso.localRotation, Quaternion.AngleAxis(20 * lowerRotationSin, Vector3.right), Time.deltaTime * 360);
    }

}
