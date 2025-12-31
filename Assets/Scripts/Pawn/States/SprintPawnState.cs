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
        if (!m_brain.commands.sprint || m_brain.commands.forwards <= 0)
        {
            return PawnStateType.Idle;
        }

        //Can still change the selected tool during sprint.
        m_properties.selectedToolIndex = m_brain.commands.selected;

        //Can still use a tool during sprint, but with sprint behaviour!
        if (m_brain.commands.primary)
        {
            return m_properties.selectedTool.StartPrimaryAction(m_brain.commands, stateType);
        }

        UpdateRotation();

        for (int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Sprint(m_brain.commands, m_properties, m_brain.commands.actionDirection);
        }

        return stateType;
    }
    //protected override void UpdateRotation()
    //{
    //    //Will need to override the rotation behaviour during sprint....

    //    Vector3 normalizedLook = m_brain.commands.look.normalized;
    //    m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

    //    m_properties.roll_speed += m_brain.commands.roll * m_properties.roll_acc * Time.deltaTime;
    //    m_properties.roll_speed = m_properties.roll_speed * Mathf.Pow(0.5f, Time.deltaTime * 18);
    //    m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_properties.roll_speed * Time.deltaTime, Vector3.forward);
    //}

    public override void FixedUpdate()
    {
        m_properties.m_physics.AddForce(m_properties.eyeTransform.parent.up * m_brain.commands.forwards  *
            m_properties.m_swim_force);
    }

    public override void Exit()
    {
        //Return the friction to normal.
        m_properties.m_physics.linearDamping *= 2;
    }
}
