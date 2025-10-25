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

    public void Initialize(HumanoidBodyCreator creator, bool isRight)
    {

        if(shoulderLimb == null)
        {
            shoulderLimb = gameObject.AddComponent<GeneratedLimb>();
            shoulder = transform;
        }
        shoulderLimb.length = creator.shoulderWidth;
        shoulderLimb.startRadius = creator.torsoDepth;
        shoulderLimb.endRadius = creator.shoulderSize;
        shoulderLimb.mat = new Material(creator.basicInGameObject);
        shoulderLimb.startColor = creator.skinColor;
        shoulderLimb.endColor = creator.skinColor;
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
        armLimb.length = creator.armLength;
        armLimb.startRadius = creator.shoulderSize;
        armLimb.endRadius = creator.elbowSize;
        armLimb.mat = new Material(creator.basicInGameObject);
        armLimb.startColor = creator.skinColor;
        armLimb.endColor = creator.skinColor;
        armLimb.Initialize();

        forearmLimb.length = creator.forearmLength;
        forearmLimb.startRadius = creator.elbowSize;
        forearmLimb.endRadius = creator.wristSize;
        forearmLimb.mat = new Material(creator.basicInGameObject);
        forearmLimb.startColor = creator.skinColor;
        forearmLimb.endColor = creator.skinColor;
        forearmLimb.Initialize();

        if (isRight)
            initialShoulderRotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);
        else
            initialShoulderRotation = Quaternion.LookRotation(Vector3.left, Vector3.forward);

        initialArmRotation = Quaternion.identity;
        initialForearmRotation = Quaternion.identity;

    }

    public override void Idle(Vector3 movementDirection, ActionDirection actionDirection)
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
