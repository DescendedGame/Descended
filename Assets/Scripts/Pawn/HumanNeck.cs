using UnityEngine;

public class HumanNeck : BodyPart
{
    Transform m_head;
    Transform m_atlas;
    public void Initialize(Transform head, Transform atlas)
    {
        m_head = head;
        m_atlas = atlas;
    }

    public override void Idle(PawnProperties pawnProperties, ActionDirection actionDirection)
    {

        Quaternion currentEyeRotation = pawnProperties.eyeTransform.rotation;

        float rotationDifference = Quaternion.Angle(m_atlas.rotation, currentEyeRotation);
        if (rotationDifference > 60) m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, rotationDifference -60);
        transform.rotation =Quaternion.RotateTowards(m_atlas.rotation, currentEyeRotation, Quaternion.Angle(m_atlas.rotation, currentEyeRotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);

        m_head.rotation = pawnProperties.eyeTransform.rotation;
        pawnProperties.eyeTransform.rotation = currentEyeRotation;
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
}
