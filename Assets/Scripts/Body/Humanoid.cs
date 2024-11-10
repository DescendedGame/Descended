using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Humanoid : Body
{
    float roll_acc = 1500;
    float roll_speed;

    protected override void Awake()
    {
        m_swim_drag = 5;
    }

    protected override void UpdateRotation()
    {
        Vector3 normalizedLook = m_brain.commands.look.normalized;
        m_pivot.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

        roll_speed += m_brain.commands.roll * roll_acc * Time.deltaTime;
        roll_speed = roll_speed * Mathf.Pow(0.5f, Time.deltaTime *18);
        m_pivot.rotation *= Quaternion.AngleAxis(roll_speed * Time.deltaTime, Vector3.forward);
    }

    protected override void FixedUpdate()
    {
        m_physics.AddForce(Vector3.Normalize(m_brain.commands.rightwards * m_pivot.right + m_brain.commands.upwards * m_pivot.up + m_brain.commands.forwards * m_pivot.forward) * 1000);
    }
}
