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


    public void Initialize(bool isRight, float shoulderWidth, float torsoDepth, float shoulderSize, Material basicInGameObject, Color skinColor, float armLength, float elbowSize
        ,float forearmLength, float wristSize)
    {


        GeneratedLimb t_shoulder = gameObject.AddComponent<GeneratedLimb>();
        t_shoulder.length = shoulderWidth;
        t_shoulder.startRadius = torsoDepth;
        t_shoulder.endRadius = shoulderSize;
        t_shoulder.mat = new Material(basicInGameObject);
        t_shoulder.startColor = skinColor;
        t_shoulder.endColor = skinColor;
        t_shoulder.Initialize();
        shoulder = t_shoulder.transform;

        this.isRight = isRight;
        string rightOrLeft;
        if (isRight)
            rightOrLeft = "Right";
        else
            rightOrLeft = "Left";

        GameObject go = new GameObject(rightOrLeft+"Arm");
        go.layer = gameObject.layer;
        go.transform.SetParent(t_shoulder.transform, false);
        GeneratedLimb t_arm = go.AddComponent<GeneratedLimb>();
        t_arm.snapToParent = true;
        t_arm.length = armLength;
        t_arm.startRadius = shoulderSize;
        t_arm.endRadius = elbowSize;
        t_arm.mat = new Material(basicInGameObject);
        t_arm.startColor = skinColor;
        t_arm.endColor = skinColor;
        t_arm.Initialize();
        arm = t_arm.transform;

        go = new GameObject(rightOrLeft+"Forearm");
        go.layer = gameObject.layer;
        go.transform.SetParent(t_arm.transform, false);
        GeneratedLimb t_forearm = go.AddComponent<GeneratedLimb>();
        t_forearm.snapToParent = true;
        t_forearm.length = forearmLength;
        t_forearm.startRadius = elbowSize;
        t_forearm.endRadius = wristSize;
        t_forearm.mat = new Material(basicInGameObject);
        t_forearm.startColor = skinColor;
        t_forearm.endColor = skinColor;
        t_forearm.Initialize();
        forearm = t_forearm.transform;

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

        forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(/*10 * Mathf.Sin(-Time.time) */-90, Vector3.right), Time.deltaTime * 360);

    }

}
