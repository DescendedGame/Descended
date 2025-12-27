using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class DefendPawnState : PawnState
{
    public DefendPawnState()
    {
        stateType = PawnStateType.Defend;
    }

    public override void Enter()
    {
        Debug.Log("Enter Defend");
    }

    public override PawnStateType Update()
    {

        if(m_brain.commands.upwards <0)
        {
            RaycastHit hit;
            string[] layerMaskNames = new string[2];
            layerMaskNames[0] = "Solid";
            layerMaskNames[1] = "Shifting";
            if(Physics.Raycast(m_properties.eyeTransform.position, -m_properties.eyeTransform.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
            {
                return PawnStateType.DefendGrounded;
            }
        }

        if(m_brain.IsTryingToMove())
        {
            m_properties.dodgeTimer = 1;
            return PawnStateType.Dodge;
        }

        // Tool usage can result in a different state!
        if (!m_brain.commands.secondaryHold)
        {
            return PawnStateType.Idle;
        }
        // Tool usage can result in a different state!
        if (m_brain.commands.primary)
        {
            //PUSH
        }

        UpdateRotation();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Defend(m_properties, m_brain.commands.actionDirection);
        }

        return stateType;
    }

    public override void Exit()
    {
        Debug.Log("Exit Defend");
    }
}
