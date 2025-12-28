using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class PreparePawnState : PawnState
{

    ActionDirection actionDirection;

    public PreparePawnState()
    {
        stateType = PawnStateType.Prepare;
    }

    public override void Enter()
    {
        actionDirection = m_brain.commands.actionDirection;
        Debug.Log("Enter Prepare");
        base.Enter();
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
                return PawnStateType.PrepareGrounded;
            }
        }

        m_properties.prepareTimer -= Time.deltaTime;
        if (!m_brain.commands.primaryHold && m_properties.prepareTimer <= 0)
        {
            return PawnStateType.Attack;
        }


        // Tool usage can result in a different state!
        if (m_brain.commands.secondary)
        {
            return PawnStateType.Idle; //then it will go to whatever the secondary action does from there.
        }

        UpdateRotation();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Prepare(m_brain.commands, m_properties, actionDirection);
        }

        return stateType;
    }

    public override void Exit()
    {
        m_brain.commands.actionDirection = actionDirection;
        Debug.Log("Exit Prepare");
        base.Exit();
    }
}
