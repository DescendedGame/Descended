using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class DodgePawnState : PawnState
{
    public DodgePawnState()
    {
        stateType = PawnStateType.Dodge;
    }

    public override void Enter()
    {
        Debug.Log("Enter Dodge");
    }

    public override PawnStateType Update()
    {
        
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Dodge(m_properties, m_brain.commands.actionDirection);
        }

        m_properties.dodgeTimer -= Time.deltaTime;
        if(m_properties.dodgeTimer <= 0)
        {
            return PawnStateType.Idle;
        }
        return stateType;
    }
    public override void Exit()
    {
        Debug.Log("Exit Dodge");
    }
}
