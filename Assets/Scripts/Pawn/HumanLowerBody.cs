using UnityEngine;

public class HumanLowerBody : BodyPart
{

    [SerializeField] HumanLeg leftLeg;
    [SerializeField] HumanLeg rightLeg;

    Vector3 leftFootPos = Vector3.zero;
    bool leftGrounded = false;
    Vector3 rightFootPos;
    bool rightGrounded = false;

    bool walkingLeg = false; //false is left

    public void Initialize(HumanLeg pLeftLeg, HumanLeg pRightLeg)
    {
        leftLeg = pLeftLeg;
        rightLeg = pRightLeg;
    }

    public override void Grounded(Vector3 movementDirection, ActionDirection actionDirection)
    {
        float leftFootTargetDistance = rightGrounded ? 1f : 0.3f;
        if ((leftLeg.transform.position - leftFootPos).magnitude > leftFootTargetDistance)
        {
            RaycastHit hit;
            string[] layerMaskNames = new string[2];
            layerMaskNames[0] = "Solid";
            layerMaskNames[1] = "Shifting";

            if (Physics.SphereCast(transform.position, 0.25f, -Vector3.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
            {
                leftFootPos = hit.point;
            }
        }
        else
        {
            leftGrounded = true;
            leftLeg.ReachFor(leftFootPos);
        }

        float rightFootTargetDistance = leftGrounded ? 1f : 0.3f;
        if ((rightLeg.transform.position - rightFootPos).magnitude > rightFootTargetDistance)
        {
            RaycastHit hit;
            string[] layerMaskNames = new string[2];
            layerMaskNames[0] = "Solid";
            layerMaskNames[1] = "Shifting";

            if (Physics.SphereCast(transform.position, 0.25f, -Vector3.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
            {
                rightFootPos = hit.point;
            }
        }
        else
        {
            rightGrounded = true;
            rightLeg.ReachFor(rightFootPos);
        }
    }

}
