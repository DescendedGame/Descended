using UnityEngine;

public class SprintPawnState : PawnState
{
    public SprintPawnState()
    {
        stateType = PawnStateType.Sprint;
    }

    public override void Enter()
    {
        //Reduce the friction to make the able to accelerate to a higher speed.
        m_properties.m_physics.linearDamping /= 2;
    }

    public override PawnStateType Update()
    {
        //Go back to idle state if sprint input no longer applies.
        if (!m_brain.commands.sprint || !m_brain.IsTryingToMove())
        {
            return PawnStateType.Idle;
        }

        //Can still change the selected tool during sprint.
        m_properties.selectedToolIndex = m_brain.commands.selected;

        //Can still use a tool during sprint, but with sprint behaviour!
        if (m_brain.commands.primary)
        {
            m_properties.selectedTool.StartPrimaryAction(m_properties.actionPoint, m_brain.commands);
        }

        UpdateRotation();

        for (int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Sprint(m_properties, ActionDirection.Down);
        }

        return stateType;
    }
    protected override void UpdateRotation()
    {
        //Will need to override the rotation behaviour during sprint....

        Vector3 normalizedLook = m_brain.commands.look.normalized;
        m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

        m_properties.roll_speed += m_brain.commands.roll * m_properties.roll_acc * Time.deltaTime;
        m_properties.roll_speed = m_properties.roll_speed * Mathf.Pow(0.5f, Time.deltaTime * 18);
        m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_properties.roll_speed * Time.deltaTime, Vector3.forward);
    }

    public override void FixedUpdate()
    {
        m_properties.m_physics.AddForce((m_properties.m_pivot.forward * m_brain.commands.forwards +
            m_properties.m_pivot.right * m_brain.commands.rightwards +
            m_properties.m_pivot.up * m_brain.commands.upwards).normalized *
            m_properties.m_swim_force);
    }

    public override void Exit()
    {
        //Return the friction to normal.
        m_properties.m_physics.linearDamping *= 2;
    }
}
