using UnityEngine;

enum LegState
{
    Idle,
}

public class HumanLeg : BodyLinkage
{
    public bool isRight = true;

    [SerializeField] Transform leftHip;
    [SerializeField] Transform rightHip;

    [SerializeField] Transform thigh;
    [SerializeField] Transform calf;
    [SerializeField] Transform foot;

    Quaternion initialThighRotation;
    Quaternion initialCalfRotation;
    Quaternion initialFootRotation;

    public void Initialize(bool isRight, float thighLength, float lowerHipRadius, 
        float kneeRadius, float upperCalfLength, float calfRadius, float lowerCalfLength, float ankleRadius, Material basicInGameObject, Color skinColor)
    {
        initialThighRotation = transform.localRotation;
        GeneratedLimb t_thigh = gameObject.AddComponent<GeneratedLimb>();
        t_thigh.snapToParent = true;
        t_thigh.length = thighLength;
        t_thigh.startRadius = lowerHipRadius;
        t_thigh.endRadius = kneeRadius;
        t_thigh.mat = new Material(basicInGameObject);
        t_thigh.startColor = skinColor;
        t_thigh.endColor = skinColor;
        t_thigh.Initialize();
        thigh = t_thigh.transform;

        this.isRight = isRight;
        string rightOrLeft;
        if (isRight)
            rightOrLeft = "Right";
        else
            rightOrLeft = "Left";

        GameObject go = new GameObject(rightOrLeft + "UpperCalf");
        go.transform.SetParent(t_thigh.transform, false);
        GeneratedLimb t_upperCalf = go.AddComponent<GeneratedLimb>();
        t_upperCalf.snapToParent = true;
        t_upperCalf.length = upperCalfLength;
        t_upperCalf.startRadius = kneeRadius;
        t_upperCalf.endRadius = calfRadius;
        t_upperCalf.mat = new Material(basicInGameObject);
        t_upperCalf.startColor = skinColor;
        t_upperCalf.endColor = skinColor;
        t_upperCalf.Initialize();
        calf = t_upperCalf.transform;

        go = new GameObject(rightOrLeft + "LowerCalf");
        go.transform.SetParent(t_upperCalf.transform, false);
        GeneratedLimb t_lowerCalf = go.AddComponent<GeneratedLimb>();
        t_lowerCalf.snapToParent = true;
        t_lowerCalf.length = lowerCalfLength;
        t_lowerCalf.startRadius = calfRadius;
        t_lowerCalf.endRadius = ankleRadius;
        t_lowerCalf.mat = new Material(basicInGameObject);
        t_lowerCalf.startColor = skinColor;
        t_lowerCalf.endColor = skinColor;
        t_lowerCalf.Initialize();
    }

    private void Awake()
    {
        //Quaternion initialWorldRotation = Quaternion.Lerp(leftHip.rotation, rightHip.rotation, 0.5f);
        //thigh.rotation = initialWorldRotation;
        //initialThighRotation = thigh.localRotation;
        initialCalfRotation = Quaternion.identity;
        initialFootRotation = Quaternion.identity;
    }

    public override void Idle()
    {
        if (isRight)
        {
            thigh.localRotation = Quaternion.RotateTowards(thigh.localRotation,
                initialThighRotation*Quaternion.AngleAxis(10 * WaveVariables.sinTime  + 10, Vector3.up) * Quaternion.AngleAxis(-10 * WaveVariables.sinTimeRushQuarter - 10, Vector3.right),
                Time.deltaTime * 360);
        }
        else
        {
            thigh.localRotation = Quaternion.RotateTowards(thigh.localRotation,
                initialThighRotation * Quaternion.AngleAxis(-10 * WaveVariables.sinTime - 10, Vector3.up) * Quaternion.AngleAxis(-10 * WaveVariables.sinTimeRushQuarter - 10, Vector3.right),
                Time.deltaTime * 360);
        }

        calf.localRotation = Quaternion.RotateTowards(calf.localRotation,
                Quaternion.AngleAxis(30 * WaveVariables.sinTimeRushQuarter + 30, Vector3.right),
                Time.deltaTime * 360);
    }

}
