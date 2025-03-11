using UnityEngine;
public class IdlePawnState : PawnState
{
    public IdlePawnState()
    {
        stateType = PawnStateType.Idle;
    }

    public override PawnStateType Update()
    {
        if (m_brain.commands.sprint && m_brain.IsTryingToMove())
        {
            Debug.Log("Go to sprint!");
            return PawnStateType.Sprint;
        }

        m_properties.selectedToolIndex = m_brain.commands.selected;
        UpdateRotation();
        if (m_brain.commands.primary)
        {
            m_properties.selectedTool.StartAction(m_properties.actionPoint);
        }

        return this.stateType;
    }
}
