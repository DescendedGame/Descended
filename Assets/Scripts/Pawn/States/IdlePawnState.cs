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
        if(m_brain.commands.upwards <0)
        {
            RaycastHit hit;
            string[] layerMaskNames = new string[2];
            layerMaskNames[0] = "Solid";
            layerMaskNames[1] = "Shifting";
            if(Physics.Raycast(m_properties.eyeTransform.position, -m_properties.eyeTransform.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
            {
                return PawnStateType.Grounded;
            }
        }

        // Tool selection
        if(m_properties.selectedToolIndex != m_brain.commands.selected)
        {
            m_properties.tools[m_properties.selectedToolIndex].Unequip();
            m_properties.selectedToolIndex = m_brain.commands.selected;
            m_properties.tools[m_properties.selectedToolIndex].Equip();
        }


        // Tool usage can result in a different state!
        if (m_brain.commands.secondary)
        {
            return m_properties.selectedTool.StartSecondaryAction(m_brain.commands, stateType);
        }
        // Tool usage can result in a different state!
        if (m_brain.commands.secondaryHold)
        {
            return m_properties.selectedTool.HoldSecondaryAction(m_brain.commands, stateType);
        }
        // Tool usage can result in a different state!
        if (m_brain.commands.primary)
        {
            return m_properties.selectedTool.StartPrimaryAction(m_brain.commands, stateType);
        }

        UpdateRotation();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Idle(m_properties, ActionDirection.Down);
        }

        return stateType;
    }
}
