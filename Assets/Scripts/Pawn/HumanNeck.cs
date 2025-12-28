using UnityEngine;

public class HumanNeck : BodyPart
{
    Transform m_head;
    Transform m_atlas;
    Quaternion upperTorsoTargetRotation = Quaternion.identity;
    Vector3 previousPosition;
    Quaternion previousRotation;
    public void Initialize(Transform head, Transform atlas)
    {
        m_head = head;
        m_atlas = atlas;
    }

    public override void RememberTransform()
    {
        previousPosition = transform.position;
        previousRotation = upperTorsoTargetRotation;
    }

    public override void Idle(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {


        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        float angleToHead = Quaternion.Angle(upperTorsoTargetRotation, m_head.rotation);
        if (angleToHead > 60) upperTorsoTargetRotation = Quaternion.RotateTowards(upperTorsoTargetRotation, m_head.rotation, angleToHead - 60);

        if(commands.upwards < 0)
        {
            upperTorsoTargetRotation = Quaternion.RotateTowards(upperTorsoTargetRotation, currentEyeRotation, Time.deltaTime * 90);
        }
        else upperTorsoTargetRotation = DragBehind(previousPosition, previousRotation, transform.position, upperTorsoTargetRotation, -Vector3.up);

        m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, upperTorsoTargetRotation * Quaternion.AngleAxis(10 * WaveVariables.sinTimeRushQuarter, Vector3.right), Time.deltaTime * 360);
        
        
        float rotationDifference = Quaternion.Angle(m_atlas.rotation, currentEyeRotation);
        if (rotationDifference > 60) m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, rotationDifference -60);
        transform.rotation =Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, Quaternion.Angle(m_atlas.rotation, currentEyeRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);
        m_head.rotation = currentEyeRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }

    public override void Prepare(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        float rotationDifference = Quaternion.Angle(m_atlas.rotation, currentEyeRotation);
        m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation * Quaternion.AngleAxis(-30, Vector3.up), rotationDifference);
        transform.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, Quaternion.Angle(m_atlas.rotation, currentEyeRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);

        m_head.rotation = currentEyeRotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
    }
    public override void Attack(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Prepare(commands, pawnProperties, actionDirection);
    }

    public override void Grounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
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

    public override void PrepareGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
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

    public override void AttackGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        PrepareGrounded(commands, pawnProperties, actionDirection);
    }

    public override void DefendGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        base.Grounded(commands, pawnProperties, actionDirection);
    }
}
