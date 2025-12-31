using UnityEngine;

enum ArmState
{
    Idle,
}

public class HumanArm : BodyLinkage
{
    public bool isRight = true;

    [SerializeField] Transform shoulder;
    [SerializeField] Transform arm;
    [SerializeField] Transform forearm;
    [SerializeField] Transform hand;

    Quaternion initialShoulderRotation;
    Quaternion initialArmRotation;
    Quaternion initialForearmRotation;

    GeneratedLimb shoulderLimb;
    GeneratedLimb armLimb;
    public GeneratedLimb forearmLimb;

    public void Initialize(HumanBodySettings bodySettings, bool isRight)
    {

        if(shoulderLimb == null)
        {
            shoulderLimb = gameObject.AddComponent<GeneratedLimb>();
            shoulder = transform;
        }
        shoulderLimb.length = bodySettings.shoulderWidth;
        shoulderLimb.startRadius = bodySettings.torsoDepth;
        shoulderLimb.endRadius = bodySettings.shoulderSize;
        shoulderLimb.mat = new Material(bodySettings.basicInGameObject);
        shoulderLimb.startColor = bodySettings.coverSettings.chest ? bodySettings.coverSettings.color : bodySettings.skinColor;
        shoulderLimb.endColor = bodySettings.coverSettings.shoulders ? bodySettings.coverSettings.color : bodySettings.skinColor;
        shoulderLimb.Initialize();

        if(armLimb == null)
        {
            this.isRight = isRight;
            string rightOrLeft;
            if (isRight)
                rightOrLeft = "Right";
            else
                rightOrLeft = "Left";

            GameObject go = new GameObject(rightOrLeft + "Arm");
            go.layer = gameObject.layer;
            go.transform.SetParent(shoulder, false);
            armLimb = go.AddComponent<GeneratedLimb>();
            armLimb.snapToParent = true;
            arm = armLimb.transform;

            go = new GameObject(rightOrLeft + "Forearm");
            go.layer = gameObject.layer;
            go.transform.SetParent(armLimb.transform, false);
            forearmLimb = go.AddComponent<GeneratedLimb>();
            forearmLimb.snapToParent = true;
            forearm = forearmLimb.transform;
        }
        armLimb.length = bodySettings.armLength;
        armLimb.startRadius = bodySettings.shoulderSize;
        armLimb.endRadius = bodySettings.elbowSize;
        armLimb.mat = new Material(bodySettings.basicInGameObject);
        armLimb.startColor = bodySettings.coverSettings.shoulders ? bodySettings.coverSettings.color : bodySettings.skinColor;
        armLimb.endColor = bodySettings.coverSettings.elbows ? bodySettings.coverSettings.color : bodySettings.skinColor;
        armLimb.Initialize();

        forearmLimb.length = bodySettings.forearmLength;
        forearmLimb.startRadius = bodySettings.elbowSize;
        forearmLimb.endRadius = bodySettings.wristSize;
        forearmLimb.mat = new Material(bodySettings.basicInGameObject);
        forearmLimb.startColor = bodySettings.coverSettings.elbows ? bodySettings.coverSettings.color : bodySettings.skinColor;
        forearmLimb.endColor = bodySettings.coverSettings.wrists ? bodySettings.coverSettings.color : bodySettings.skinColor;
        forearmLimb.Initialize();

        if (isRight)
            initialShoulderRotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);
        else
            initialShoulderRotation = Quaternion.LookRotation(Vector3.left, Vector3.forward);

        initialArmRotation = Quaternion.identity;
        initialForearmRotation = Quaternion.identity;


        if (hand == null)
        {
            hand = new GameObject("RightHand").transform;
            hand.SetParent(forearm.transform);
        }
        hand.transform.localPosition = new Vector3(0, 0, forearmLimb.length);
        hand.transform.localRotation = Quaternion.identity;

    }

    public Transform GetHand()
    {
        return hand;
    }

    public override void Idle(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);

        if (isRight)
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation, Quaternion.AngleAxis(15 * WaveVariables.sinTimeDragQuarter - 45, Vector3.up) * 
                Quaternion.AngleAxis(20 * WaveVariables.sinTime, Vector3.forward), Time.deltaTime * 360);
        else
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation, Quaternion.AngleAxis(15 * WaveVariables.sinTimeRushQuarter + 45, Vector3.up) * 
                Quaternion.AngleAxis(-20 * WaveVariables.sinTime, Vector3.forward), Time.deltaTime * 360);

        forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.right), Time.deltaTime * 360);

    }

    public override void Sprint(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        if (isRight)
        {
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation,
                initialArmRotation * Quaternion.AngleAxis(-60, Vector3.up) * Quaternion.AngleAxis(10 * WaveVariables.sinTime8 - 10, Vector3.right),
                Time.deltaTime * 360);
            forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation,
                    Quaternion.AngleAxis(30 * WaveVariables.sinTimeRushQuarter8 - 30, Vector3.right),
                    Time.deltaTime * 360);
        }
        else
        {
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation,
                initialArmRotation * Quaternion.AngleAxis(60, Vector3.up) * Quaternion.AngleAxis(10 * WaveVariables.sinTime8 - 10, Vector3.right),
                Time.deltaTime * 360);
            forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation,
                    Quaternion.AngleAxis(30 * WaveVariables.sinTimeRushQuarter8 - 30, Vector3.right),
                    Time.deltaTime * 360);
        }
    }

    public override void Prepare(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        if(isRight)
        {
            Quaternion torsoOffset = Quaternion.AngleAxis(30, Vector3.right);
            switch (actionDirection)
            {
                case ActionDirection.Up:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation*torsoOffset * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(-180, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Down:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(45, Vector3.forward) * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Left:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation*torsoOffset * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Right:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation*torsoOffset * Quaternion.AngleAxis(-30, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    break;
                case ActionDirection.None:
                    break;
                default:
                    break;
            }
        }
    }

    public override void PrepareGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        if (isRight)
        {
            float angleDifference = Quaternion.Angle(pawnProperties.GetGroundedRotation(), pawnProperties.eyeTransform.rotation);
            angleDifference *= Mathf.Sign(pawnProperties.eyeTransform.forward.y);
            Quaternion torsoOffset = Quaternion.AngleAxis(-45, Vector3.forward) * Quaternion.AngleAxis(30, Vector3.right) * Quaternion.AngleAxis(-angleDifference, Vector3.forward);
            switch (actionDirection)
            {
                case ActionDirection.Up:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(-180, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Down:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(45, Vector3.forward) * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Left:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Right:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-30, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(-120, Vector3.right), Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    break;
                case ActionDirection.None:
                    break;
                default:
                    break;
            }
        }
    }

    public override void Attack(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        if (isRight)
        {
            Quaternion torsoOffset = Quaternion.AngleAxis(30, Vector3.right);
            switch (actionDirection)
            {
                case ActionDirection.Up:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation* torsoOffset * Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(-30, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Down:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation* torsoOffset * Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(10, Vector3.up) * Quaternion.AngleAxis(45, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity * Quaternion.AngleAxis(45, Vector3.forward), Time.deltaTime * 180);
                    break;
                case ActionDirection.Left:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-60, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Right:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-120, Vector3.right)* Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    
                    break;
                case ActionDirection.None:
                    break;
                default:
                    break;
            }
        }
    }

    public override void AttackGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        if (isRight)
        {
            float angleDifference = Quaternion.Angle(pawnProperties.GetGroundedRotation(), pawnProperties.eyeTransform.rotation);
            angleDifference *= Mathf.Sign(pawnProperties.eyeTransform.forward.y);
            Quaternion torsoOffset = Quaternion.AngleAxis(-45, Vector3.forward) * Quaternion.AngleAxis(30, Vector3.right) * Quaternion.AngleAxis(-angleDifference, Vector3.forward);
            switch (actionDirection)
            {
                case ActionDirection.Up:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(-30, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Down:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-90, Vector3.right) * Quaternion.AngleAxis(10, Vector3.up) * Quaternion.AngleAxis(45, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.identity * Quaternion.AngleAxis(45, Vector3.forward), Time.deltaTime * 180);
                    break;
                case ActionDirection.Left:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-60, Vector3.right), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);
                    break;
                case ActionDirection.Right:
                    shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime * 360);
                    arm.localRotation = Quaternion.RotateTowards(arm.localRotation, initialArmRotation * torsoOffset * Quaternion.AngleAxis(-120, Vector3.right) * Quaternion.AngleAxis(-90, Vector3.forward), Time.deltaTime * 360);
                    forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity, Time.deltaTime * 360);

                    break;
                case ActionDirection.None:
                    break;
                default:
                    break;
            }
        }
    }
}
