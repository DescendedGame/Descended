using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class IdlePawnState : PawnState
{
    public IdlePawnState()
    {
        stateType = PawnStateType.Idle;
    }

    public override PawnStateType Update()
    {
        // Go to sprint if sprint conditions are met.
        if (m_brain.commands.sprint && m_brain.IsTryingToMove())
        {
            return PawnStateType.Sprint;
        }

        // Tool selection
        if(m_properties.selectedToolIndex != m_brain.commands.selected)
        {
            m_properties.tools[m_properties.selectedToolIndex].Unequip();
            m_properties.selectedToolIndex = m_brain.commands.selected;
            m_properties.tools[m_properties.selectedToolIndex].Equip(m_properties.actionPoint);
        }

        // Tool usage can result in a different state!
        if (m_brain.commands.primary)
        {
            m_properties.selectedTool.StartPrimaryAction(m_properties.actionPoint, m_brain.commands);
        }
        // Tool usage can result in a different state!
        if (m_brain.commands.secondary)
        {
            m_properties.selectedTool.StartSecondaryAction(m_properties.actionPoint, m_brain.commands);
        }
        // Tool usage can result in a different state!
        if (m_brain.commands.secondaryHold)
        {
            m_properties.selectedTool.HoldSecondaryAction(m_properties.actionPoint, m_brain.commands);
        }

        UpdateRotation();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Idle(m_brain.GetDesiredMovementDirection(), ActionDirection.Down);
        }

        return stateType;
    }
}
