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

    private void Awake()
    {
        Quaternion initialWorldRotation = Quaternion.Lerp(leftHip.rotation, rightHip.rotation, 0.5f);
        thigh.rotation = initialWorldRotation;
        initialThighRotation = thigh.localRotation;
        initialCalfRotation = Quaternion.identity;
        initialFootRotation = Quaternion.identity;
    }

    private void Update()
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
