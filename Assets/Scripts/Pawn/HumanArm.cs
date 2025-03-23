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

    private void Awake()
    {
        if(isRight)
            initialShoulderRotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);
        else
            initialShoulderRotation = Quaternion.LookRotation(Vector3.left, Vector3.forward);

        initialArmRotation = Quaternion.identity;
        initialForearmRotation = Quaternion.identity;
    }

    private void Update()
    {
        shoulder.localRotation = Quaternion.RotateTowards(shoulder.localRotation, initialShoulderRotation, Time.deltaTime *360);

        //shoulder.localRotation = initialShoulderRotation;
        if(isRight)
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation, Quaternion.AngleAxis(15 * Mathf.Sin(Time.time - Mathf.PI/2) - 45, Vector3.up) * Quaternion.AngleAxis(20 * Mathf.Sin(Time.time), Vector3.forward), Time.deltaTime * 360);
        else
            arm.localRotation = Quaternion.RotateTowards(arm.localRotation, Quaternion.AngleAxis(15 * Mathf.Sin(Time.time - Mathf.PI / 2 + Mathf.PI) + 45, Vector3.up) * Quaternion.AngleAxis(20 * Mathf.Sin(Time.time + Mathf.PI), Vector3.forward), Time.deltaTime * 360);

        forearm.localRotation = Quaternion.RotateTowards(forearm.localRotation, Quaternion.identity * Quaternion.AngleAxis(/*10 * Mathf.Sin(-Time.time) */-90, Vector3.right), Time.deltaTime * 360);
    }

}
