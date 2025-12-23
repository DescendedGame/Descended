using UnityEngine;

public class HumanLowerBody : BodyPart
{

    [SerializeField] HumanLeg leftLeg;
    [SerializeField] HumanLeg rightLeg;

    Vector3 leftFootPos = Vector3.zero;
    bool leftGrounded = false;
    Vector3 rightFootPos;
    bool rightGrounded = false;


    public void Initialize(HumanLeg pLeftLeg, HumanLeg pRightLeg)
    {
        leftLeg = pLeftLeg;
        rightLeg = pRightLeg;
    }

    public override void Grounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        float leftLegDistance = (leftLeg.transform.position - leftFootPos).magnitude;
        float rightLegDistance = (rightLeg.transform.position - rightFootPos).magnitude;



        if (rightGrounded && pawnProperties.attemptedMoveDirection.magnitude != 0)
        {
            leftLeg.MakeReady();
            leftGrounded = false;
        }
        else
        {
            if (leftLegDistance > leftLeg.GetLength())
            {
                leftGrounded = false;
                RaycastHit hit;
                string[] layerMaskNames = new string[2];
                layerMaskNames[0] = "Solid";
                layerMaskNames[1] = "Shifting";

                if (Physics.SphereCast(leftLeg.transform.position, 0.25f, -Vector3.up +pawnProperties.attemptedMoveDirection / pawnProperties.m_swim_force, out hit, leftLeg.GetLength() - 0.25f, LayerMask.GetMask(layerMaskNames)))
                {
                    leftFootPos = hit.point;
                }

                leftLeg.MakeReady();
            }
            else
            {
                if (leftLegDistance < leftLeg.GetLength() * 0.95f) leftGrounded = true;
                else leftGrounded = false;
                leftLeg.ReachFor(leftFootPos);
            }
        }

        if(leftGrounded && pawnProperties.attemptedMoveDirection.magnitude != 0)
        {
            rightLeg.MakeReady();
            rightGrounded = false;
        }
        else
        {
            if (rightLegDistance > rightLeg.GetLength())
            {
                rightGrounded = false;
                RaycastHit hit;
                string[] layerMaskNames = new string[2];
                layerMaskNames[0] = "Solid";
                layerMaskNames[1] = "Shifting";

                if (Physics.SphereCast(rightLeg.transform.position, 0.25f, -Vector3.up + pawnProperties.attemptedMoveDirection/pawnProperties.m_swim_force, out hit, rightLeg.GetLength() - 0.25f, LayerMask.GetMask(layerMaskNames)))
                {
                    rightFootPos = hit.point;
                }

                rightLeg.MakeReady();
            }
            else
            {
                if (rightLegDistance < rightLeg.GetLength() * 0.95f) rightGrounded = true;
                else rightGrounded = false;

                rightLeg.ReachFor(rightFootPos);
            }
        }
        
    }

}
