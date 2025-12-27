using UnityEngine;

public class HumanNeck : BodyPart
{
    Transform m_head;
    Transform m_atlas;
    Quaternion upperTorsoTargetRotation = Quaternion.identity;
    public void Initialize(Transform head, Transform atlas)
    {
        m_head = head;
        m_atlas = atlas;
    }

    public override void Idle(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        //SMOOTHNESS


        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        float angleToHead = Quaternion.Angle(upperTorsoTargetRotation, m_head.rotation);
        if (angleToHead > 60) upperTorsoTargetRotation = Quaternion.RotateTowards(upperTorsoTargetRotation, m_head.rotation, angleToHead - 60);
        m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, upperTorsoTargetRotation * Quaternion.AngleAxis(10 * WaveVariables.sinTimeRushQuarter, Vector3.right), Time.deltaTime * 360);

        float rotationDifference = Quaternion.Angle(m_atlas.rotation, currentEyeRotation);
        if (rotationDifference > 60) m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, rotationDifference -60);
        transform.rotation =Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, Quaternion.Angle(m_atlas.rotation, currentEyeRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);
        m_head.rotation = currentEyeRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }

    public override void Prepare(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        float rotationDifference = Quaternion.Angle(m_atlas.rotation, currentEyeRotation);
        m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation * Quaternion.AngleAxis(-30, Vector3.up), rotationDifference);
        transform.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, Quaternion.Angle(m_atlas.rotation, currentEyeRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);

        m_head.rotation = currentEyeRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }
    public override void Attack(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Prepare(pawnProperties, actionDirection);
    }

    public override void Grounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        Quaternion flatHeadRotation = pawnProperties.GetGroundedRotation() * Quaternion.AngleAxis(45, Vector3.right);

        if(pawnProperties.attemptedMoveDirection.magnitude != 0)
        {
            m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, Quaternion.LookRotation(pawnProperties.attemptedMoveDirection, Vector3.up) * Quaternion.AngleAxis(45, Vector3.right), Time.deltaTime * 360);
        }
        float yAxisAngle = Quaternion.Angle(flatHeadRotation, m_atlas.rotation);

        if (yAxisAngle > 60)
        {
            m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, flatHeadRotation, yAxisAngle - 60);
        }
        

        m_head.rotation = currentEyeRotation;
        float xAxisDifference = Quaternion.Angle(flatHeadRotation, currentEyeRotation);

        Quaternion headRotation = currentEyeRotation;
        if (xAxisDifference > 60)
        {
            headRotation = Quaternion.RotateTowards(currentEyeRotation, flatHeadRotation, xAxisDifference - 60);
        }

        transform.rotation = Quaternion.RotateTowards(m_atlas.rotation, headRotation, Quaternion.Angle(m_atlas.rotation, headRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);
        m_head.rotation = headRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }

    public override void PrepareGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        Quaternion flatHeadRotation = pawnProperties.GetGroundedRotation() * Quaternion.AngleAxis(45, Vector3.right);

        m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, Quaternion.AngleAxis(-30, Vector3.up)*flatHeadRotation, 360);


        m_head.rotation = currentEyeRotation;
        float xAxisDifference = Quaternion.Angle(flatHeadRotation, currentEyeRotation);

        Quaternion headRotation = currentEyeRotation;
        if (xAxisDifference > 60)
        {
            headRotation = Quaternion.RotateTowards(currentEyeRotation, flatHeadRotation, xAxisDifference - 60);
        }

        transform.rotation = Quaternion.RotateTowards(m_atlas.rotation, headRotation, Quaternion.Angle(m_atlas.rotation, headRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);
        m_head.rotation = headRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }

    public override void AttackGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        PrepareGrounded(pawnProperties, actionDirection);
    }

    public override void DefendGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        base.Grounded(pawnProperties, actionDirection);
    }
}
