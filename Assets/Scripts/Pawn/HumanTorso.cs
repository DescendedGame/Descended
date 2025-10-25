using UnityEngine;

public class HumanTorso : BodyPart
{
    public bool isRight = true;

    public Transform head;
    [SerializeField] Transform upperTorso;
    [SerializeField] Transform middleTorso;
    [SerializeField] Transform lowerTorso;

    Quaternion upperTorsoTargetRotation = Quaternion.identity;
    Quaternion middleTorsoTargetRotation = Quaternion.identity;
    Quaternion lowerTorsoTargetRotation = Quaternion.identity;

    Quaternion unalteredUpperTorsoRotation;
    Vector3 unalteredUpperTorsoPosition;
    Quaternion unalteredMiddleTorsoRotation;
    Vector3 unalteredMiddleTorsoPosition;
    Quaternion unalteredLowerTorsoRotation;
    Vector3 unalteredLowerTorsoPosition;

    GeneratedLimb lowerNeck;

    GeneratedLimb leftRib;
    GeneratedLimb rightRib;
    BodyStitcher ribSkin;

    GeneratedLimb leftHip;
    GeneratedLimb rightHip;
    BodyStitcher hipSkin;

    GeneratedLimb leftBelly;
    GeneratedLimb rightBelly;
    BodyStitcher bellySkin;

    /// <returns>Returns left and right slots to be used for leg placement.</returns>
    public (Transform, Transform) Initialize(HumanoidBodyCreator creator)
    {
        upperTorso = transform;

        if(lowerNeck == null)
        {
            GameObject go = new GameObject("LowerNeck");
            go.layer = gameObject.layer;
            go.transform.SetParent(transform, false);
            lowerNeck = go.AddComponent<GeneratedLimb>();
            lowerNeck.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        }
        lowerNeck.length = creator.atlasLength;
        lowerNeck.startRadius = creator.lowerNeckWidth;
        lowerNeck.endRadius = creator.torsoDepth;
        lowerNeck.mat = new Material(creator.basicInGameObject);
        lowerNeck.startColor = creator.skinColor;
        lowerNeck.endColor = creator.skinColor;
        lowerNeck.Initialize();

        Vector3 atlasEnd = Vector3.forward * creator.atlasLength;

        if(middleTorso == null)
        {
            middleTorso = new GameObject("MiddleTorso").transform;
            middleTorso.gameObject.layer = gameObject.layer;
            middleTorso.transform.SetParent(transform.transform, false);
        }
        middleTorso.transform.localPosition = Vector3.down * (creator.atlasLength + creator.ribLength);

        if(lowerTorso == null)
        {
            lowerTorso = new GameObject("LowerTorso").transform;
            lowerTorso.transform.SetParent(middleTorso.transform, false);
        }
        lowerTorso.transform.localPosition = Vector3.down * (creator.bellyLength);

        //Ribs
        //---------------------------------------------------------------------------------------------------

        if(leftRib == null)
        {
            GameObject go = new GameObject("LeftRibs");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerNeck.transform, false);
            leftRib = go.AddComponent<GeneratedLimb>();
        }
        leftRib.transform.localPosition = atlasEnd - Vector3.right * creator.torsoWidth;
        leftRib.length = creator.ribLength;
        leftRib.startRadius = creator.torsoDepth;
        leftRib.endRadius = creator.waist;
        leftRib.mat = new Material(creator.basicInGameObject);
        leftRib.startColor = creator.skinColor;
        leftRib.endColor = creator.skinColor;
        leftRib.Initialize();

        if(rightRib == null)
        {
            GameObject go = new GameObject("RightRibs");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerNeck.transform, false);
            rightRib = go.AddComponent<GeneratedLimb>();
        }
        rightRib.transform.localPosition = atlasEnd + Vector3.right * creator.torsoWidth;
        rightRib.length = creator.ribLength;
        rightRib.startRadius = creator.torsoDepth;
        rightRib.endRadius = creator.waist;
        rightRib.mat = new Material(creator.basicInGameObject);
        rightRib.startColor = creator.skinColor;
        rightRib.endColor = creator.skinColor;
        rightRib.Initialize();

        if(ribSkin == null)
        {
            GameObject go = new GameObject("RibSkin");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightRib.transform, false);
            ribSkin = go.AddComponent<BodyStitcher>();
        }
        ribSkin.leftSide = leftRib;
        ribSkin.rightSide = rightRib;
        ribSkin.mat = new Material(creator.basicInGameObject);
        ribSkin.startColor = creator.skinColor;
        ribSkin.endColor = creator.skinColor;
        ribSkin.Initialize();
        //----------------------------------------------------------------------------

        //Hips
        //----------------------------------------------------------------------------
        //The hips will depend on what kind of lower body is desired.... but now just human.

        if(leftHip == null)
        {
            GameObject go = new GameObject("LeftHip");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerTorso.transform, false);
            leftHip = go.AddComponent<GeneratedLimb>();
        }
        leftHip.transform.localPosition = -Vector3.right * creator.upperHipWidth;
        leftHip.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-creator.hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        leftHip.length = creator.hipLength;
        leftHip.startRadius = creator.upperHipRadius;
        leftHip.endRadius = creator.lowerHipRadius;
        leftHip.mat = new Material(creator.basicInGameObject);
        leftHip.startColor = creator.skinColor;
        leftHip.endColor = creator.skinColor;
        leftHip.Initialize();

        if(rightHip == null)
        {
            GameObject go = new GameObject("RightHip");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerTorso.transform, false);
            rightHip = go.AddComponent<GeneratedLimb>();
        }
        rightHip.transform.localPosition = Vector3.right * creator.upperHipWidth;
        rightHip.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(creator.hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        rightHip.length = creator.hipLength;
        rightHip.startRadius = creator.upperHipRadius;
        rightHip.endRadius = creator.lowerHipRadius;
        rightHip.mat = new Material(creator.basicInGameObject);
        rightHip.startColor = creator.skinColor;
        rightHip.endColor = creator.skinColor;
        rightHip.Initialize();

        if(hipSkin == null)
        {
            GameObject go = new GameObject("HipSkin");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightHip.transform, false);
            hipSkin = go.AddComponent<BodyStitcher>();
        }
        hipSkin.leftSide = leftHip;
        hipSkin.rightSide = rightHip;
        hipSkin.mat = new Material(creator.basicInGameObject);
        hipSkin.startColor = creator.skinColor;
        hipSkin.endColor = creator.skinColor;
        hipSkin.Initialize();

        //--------------------------------------------------------------------------------
        // Connect rib and hip with belly:
        //--------------------------------------------------------------------

        if(leftBelly == null)
        {
            GameObject go = new GameObject("LeftBelly");
            go.layer = gameObject.layer;
            go.transform.SetParent(leftRib.transform, false);
            leftBelly = go.AddComponent<GeneratedLimb>();
        }
        leftBelly.snapToParent = true;
        leftBelly.startRadius = creator.waist;
        leftBelly.endRadius = creator.upperHipRadius;
        leftBelly.target = leftHip.transform;
        leftBelly.mat = new Material(creator.basicInGameObject);
        leftBelly.startColor = creator.skinColor;
        leftBelly.endColor = creator.skinColor;
        leftBelly.Initialize();
        leftBelly.UpdateVertices();

        if(rightBelly == null)
        {
            GameObject go = new GameObject("RightBelly");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightRib.transform, false);
            rightBelly = go.AddComponent<GeneratedLimb>();
        }
        rightBelly.snapToParent = true;
        rightBelly.startRadius = creator.waist;
        rightBelly.endRadius = creator.upperHipRadius;
        rightBelly.target = rightHip.transform;
        rightBelly.mat = new Material(creator.basicInGameObject);
        rightBelly.startColor = creator.skinColor;
        rightBelly.endColor = creator.skinColor;
        rightBelly.Initialize();
        rightBelly.UpdateVertices();

        if(bellySkin == null)
        {
            GameObject go = new GameObject("BellySkin");
            go.layer = gameObject.layer;
            go.transform.SetParent(rightBelly.transform, false);
            bellySkin = go.AddComponent<BodyStitcher>();
        }
        bellySkin.leftSide = leftBelly;
        bellySkin.rightSide = rightBelly;
        bellySkin.mat = new Material(creator.basicInGameObject);
        bellySkin.startColor = creator.skinColor;
        bellySkin.endColor = creator.skinColor;
        bellySkin.Initialize();

        //--------------------------------------------------------------------

        return (leftHip.transform, rightHip.transform);
    }

    public override void RememberTransform()
    {
        unalteredUpperTorsoPosition = upperTorso.position;
        //unalteredUpperTorsoRotation = unanimatedUpperTorsoRotation;
        unalteredMiddleTorsoPosition = middleTorso.position;
        //unalteredMiddleTorsoRotation = unanimatedMiddleTorsoRotation;
        unalteredLowerTorsoPosition = lowerTorso.position;
        //unalteredLowerTorsoRotation = unanimatedLowerTorsoRotation;
    }

    public override void Idle(Vector3 movementDirection, ActionDirection actionDirection)
    {

        float angleToHead = Quaternion.Angle(upperTorsoTargetRotation, head.rotation);
        if (angleToHead > 45) upperTorsoTargetRotation = Quaternion.RotateTowards(upperTorsoTargetRotation, head.rotation, angleToHead - 45);

        float upperRotationSin = WaveVariables.sinTimeRushQuarter;
        float middleRotationSin = WaveVariables.sinTime;
        float lowerRotationSin = -WaveVariables.sinTimeRushQuarter;

        //upperTorso.rotation = Quaternion.RotateTowards(upperTorso.rotation, targetRotation, Time.deltaTime * 360);



        upperTorso.rotation = Quaternion.RotateTowards(upperTorso.rotation, upperTorsoTargetRotation * Quaternion.AngleAxis(10 * upperRotationSin, Vector3.right), Time.deltaTime * 360);


        //unanimatedMiddleTorsoRotation *= FollowParentSmoothly(middleTorso, unalteredMiddleTorsoPosition, unalteredMiddleTorsoRotation, Vector3.down, 2);

        //Vector3 t_vectorToLast = unalteredMiddleTorsoPosition + middleTorsoTargetRotation * Vector3.down - middleTorso.position;
        //Vector3 t_vectorToCurrent = middleTorsoTargetRotation * Vector3.down;
        //middleTorsoTargetRotation = Quaternion.FromToRotation(t_vectorToCurrent, t_vectorToLast) * middleTorsoTargetRotation;

        middleTorsoTargetRotation = FollowParentSmoothly(unalteredMiddleTorsoPosition, middleTorsoTargetRotation, middleTorso.position, Vector3.down, 2);
        middleTorsoTargetRotation = Quaternion.RotateTowards(middleTorsoTargetRotation, upperTorso.rotation, Time.deltaTime * 10);
        float t_angle = Quaternion.Angle(middleTorsoTargetRotation, upperTorso.rotation);
        if (t_angle > 25)
        {
            middleTorsoTargetRotation = Quaternion.RotateTowards(middleTorsoTargetRotation, upperTorso.rotation, t_angle-25);
        }

        middleTorso.rotation = Quaternion.RotateTowards(middleTorso.rotation, middleTorsoTargetRotation * Quaternion.AngleAxis(10 * middleRotationSin, Vector3.right), Time.deltaTime * 360);


        //unanimatedLowerTorsoRotation *= FollowParentSmoothly(lowerTorso, unalteredLowerTorsoPosition, unalteredLowerTorsoRotation, Vector3.down, 2);

        lowerTorsoTargetRotation = FollowParentSmoothly(unalteredLowerTorsoPosition, lowerTorsoTargetRotation, lowerTorso.position, Vector3.down, 2);
        lowerTorsoTargetRotation = Quaternion.RotateTowards(lowerTorsoTargetRotation, middleTorso.rotation, Time.deltaTime * 10);
        t_angle = Quaternion.Angle(lowerTorsoTargetRotation, middleTorso.rotation);
        if (t_angle > 25)
        {
            lowerTorsoTargetRotation = Quaternion.RotateTowards(lowerTorsoTargetRotation, middleTorso.rotation, t_angle - 25);
        }

        lowerTorso.rotation = Quaternion.RotateTowards(lowerTorso.rotation, lowerTorsoTargetRotation * Quaternion.AngleAxis(20 * lowerRotationSin, Vector3.right), Time.deltaTime * 360);


        // If moving...
        //FollowParentSmoothly(middleTorso, unalteredMiddleTorsoPosition, unalteredMiddleTorsoRotation, Vector3.down, 2);
        //FollowParentSmoothly(lowerTorso, unalteredLowerTorsoPosition, unalteredLowerTorsoRotation, Vector3.down, 2);

    }

}
