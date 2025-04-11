using UnityEngine;

public class HumanTorso : BodyLinkage
{
    public bool isRight = true;

    [SerializeField] Transform upperTorso;
    [SerializeField] Transform middleTorso;
    [SerializeField] Transform lowerTorso;

    Quaternion initialUpperRotation;
    Quaternion initialMiddleRotation;
    Quaternion initialLowerRotation;

    private void Awake()
    {
        initialUpperRotation = Quaternion.identity;
        initialMiddleRotation = Quaternion.identity;
        initialLowerRotation = Quaternion.identity;
    }

    private void Update()
    {
        float upperRotationSin = WaveVariables.sinTimeRushQuarter;
        float middleRotationSin = WaveVariables.sinTime;
        float lowerRotationSin = -WaveVariables.sinTimeRushQuarter;

        upperTorso.localRotation = Quaternion.RotateTowards(upperTorso.localRotation, Quaternion.AngleAxis(10 * upperRotationSin, Vector3.right), Time.deltaTime * 360);
        middleTorso.localRotation = Quaternion.RotateTowards(middleTorso.localRotation, Quaternion.AngleAxis(10 * middleRotationSin, Vector3.right), Time.deltaTime * 360);
        lowerTorso.localRotation = Quaternion.RotateTowards(lowerTorso.localRotation, Quaternion.AngleAxis(20 * lowerRotationSin, Vector3.right), Time.deltaTime * 360);
    }

}
