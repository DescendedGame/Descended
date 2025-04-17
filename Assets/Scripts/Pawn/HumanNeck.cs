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

    public override void Idle()
    {
        Quaternion currentHeadRotation = m_head.rotation;

        float rotationDifference = Quaternion.Angle(m_atlas.rotation, m_head.rotation);
        if (rotationDifference > 60) m_atlas.rotation = Quaternion.RotateTowards(m_atlas.rotation, m_head.rotation, rotationDifference -60);
        transform.rotation =Quaternion.RotateTowards(m_atlas.rotation, m_head.rotation, Quaternion.Angle(m_atlas.rotation, m_head.rotation) / 2) * Quaternion.LookRotation(Vector3.up, Vector3.back);

        m_head.rotation = currentHeadRotation;
    }
}
