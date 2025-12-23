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

    Quaternion initialShoulderRotation;
    Quaternion initialArmRotation;
    Quaternion initialForearmRotation;

    GeneratedLimb shoulderLimb;
    GeneratedLimb armLimb;
    GeneratedLimb forearmLimb;

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

    }

    public override void Idle(PawnProperties pawnProperties, ActionDirection actionDirection)
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

}
