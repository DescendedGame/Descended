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
    public (Transform, Transform) Initialize(HumanBodySettings bodySettings)
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
        lowerNeck.length = bodySettings.atlasLength;
        lowerNeck.startRadius = bodySettings.lowerNeckWidth;
        lowerNeck.endRadius = bodySettings.torsoDepth;
        lowerNeck.mat = new Material(bodySettings.basicInGameObject);
        lowerNeck.startColor = bodySettings.coverSettings.lowerNeck ? bodySettings.coverSettings.color : bodySettings.skinColor;
        lowerNeck.endColor = bodySettings.coverSettings.chest ? bodySettings.coverSettings.color : bodySettings.skinColor;
        lowerNeck.Initialize();

        Vector3 atlasEnd = Vector3.forward * bodySettings.atlasLength;

        if(middleTorso == null)
        {
            middleTorso = new GameObject("MiddleTorso").transform;
            middleTorso.gameObject.layer = gameObject.layer;
            middleTorso.transform.SetParent(transform.transform, false);
        }
        middleTorso.transform.localPosition = Vector3.down * (bodySettings.atlasLength + bodySettings.ribLength);

        if(lowerTorso == null)
        {
            lowerTorso = new GameObject("LowerTorso").transform;
            lowerTorso.transform.SetParent(middleTorso.transform, false);
        }
        lowerTorso.transform.localPosition = Vector3.down * (bodySettings.bellyLength);

        //Ribs
        //---------------------------------------------------------------------------------------------------

        if(leftRib == null)
        {
            GameObject go = new GameObject("LeftRibs");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerNeck.transform, false);
            leftRib = go.AddComponent<GeneratedLimb>();
        }
        leftRib.transform.localPosition = atlasEnd - Vector3.right * bodySettings.torsoWidth;
        leftRib.length = bodySettings.ribLength;
        leftRib.startRadius = bodySettings.torsoDepth;
        leftRib.endRadius = bodySettings.waist;
        leftRib.mat = new Material(bodySettings.basicInGameObject);
        leftRib.startColor = bodySettings.coverSettings.chest ? bodySettings.coverSettings.color : bodySettings.skinColor;
        leftRib.endColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color : bodySettings.skinColor;
        leftRib.Initialize();

        if(rightRib == null)
        {
            GameObject go = new GameObject("RightRibs");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerNeck.transform, false);
            rightRib = go.AddComponent<GeneratedLimb>();
        }
        rightRib.transform.localPosition = atlasEnd + Vector3.right * bodySettings.torsoWidth;
        rightRib.length = bodySettings.ribLength;
        rightRib.startRadius = bodySettings.torsoDepth;
        rightRib.endRadius = bodySettings.waist;
        rightRib.mat = new Material(bodySettings.basicInGameObject);
        rightRib.startColor = bodySettings.coverSettings.chest ? bodySettings.coverSettings.color : bodySettings.skinColor;
        rightRib.endColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color :  bodySettings.skinColor;
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
        ribSkin.mat = new Material(bodySettings.basicInGameObject);
        ribSkin.startColor = bodySettings.coverSettings.chest ? bodySettings.coverSettings.color : bodySettings.skinColor;
        ribSkin.endColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
        leftHip.transform.localPosition = -Vector3.right * bodySettings.upperHipWidth;
        leftHip.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-bodySettings.hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        leftHip.length = bodySettings.hipLength;
        leftHip.startRadius = bodySettings.upperHipRadius;
        leftHip.endRadius = bodySettings.lowerHipRadius;
        leftHip.mat = new Material(bodySettings.basicInGameObject);
        leftHip.startColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
        leftHip.endColor = bodySettings.coverSettings.butt ? bodySettings.coverSettings.color : bodySettings.skinColor;
        leftHip.Initialize();

        if(rightHip == null)
        {
            GameObject go = new GameObject("RightHip");
            go.layer = gameObject.layer;
            go.transform.SetParent(lowerTorso.transform, false);
            rightHip = go.AddComponent<GeneratedLimb>();
        }
        rightHip.transform.localPosition = Vector3.right * bodySettings.upperHipWidth;
        rightHip.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(bodySettings.hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        rightHip.length = bodySettings.hipLength;
        rightHip.startRadius = bodySettings.upperHipRadius;
        rightHip.endRadius = bodySettings.lowerHipRadius;
        rightHip.mat = new Material(bodySettings.basicInGameObject);
        rightHip.startColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
        rightHip.endColor = bodySettings.coverSettings.butt ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
        hipSkin.mat = new Material(bodySettings.basicInGameObject);
        hipSkin.startColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
        hipSkin.endColor = bodySettings.coverSettings.butt ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
        leftBelly.startRadius = bodySettings.waist;
        leftBelly.endRadius = bodySettings.upperHipRadius;
        leftBelly.target = leftHip.transform;
        leftBelly.mat = new Material(bodySettings.basicInGameObject);
        leftBelly.startColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color : bodySettings.skinColor;
        leftBelly.endColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
        rightBelly.startRadius = bodySettings.waist;
        rightBelly.endRadius = bodySettings.upperHipRadius;
        rightBelly.target = rightHip.transform;
        rightBelly.mat = new Material(bodySettings.basicInGameObject);
        rightBelly.startColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color : bodySettings.skinColor;
        rightBelly.endColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
        bellySkin.mat = new Material(bodySettings.basicInGameObject);
        bellySkin.startColor = bodySettings.coverSettings.waist ? bodySettings.coverSettings.color : bodySettings.skinColor;
        bellySkin.endColor = bodySettings.coverSettings.hips ? bodySettings.coverSettings.color : bodySettings.skinColor;
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
