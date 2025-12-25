using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class AttackPawnState : PawnState
{
    public AttackPawnState()
    {
        stateType = PawnStateType.Attack;
    }

    public override void Enter()
    {
        Debug.Log("Enter Attack");
        base.Enter();
    }

    public override PawnStateType Update()
    {
        if (m_brain.commands.upwards < 0)
        {
            RaycastHit hit;
            string[] layerMaskNames = new string[2];
            layerMaskNames[0] = "Solid";
            layerMaskNames[1] = "Shifting";
            if (Physics.Raycast(m_properties.eyeTransform.position, -m_properties.eyeTransform.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
            {
                return PawnStateType.AttackGrounded;
            }
        }

        m_properties.attackTimer -= Time.deltaTime;
        if (m_properties.attackTimer <= 0)
        {
            return PawnStateType.Idle;
        }

        if (m_brain.commands.secondary)
        {
            return PawnStateType.Idle; //then it will go to whatever the secondary action does from there.
        }

        UpdateRotation();

        // Move all body parts idly
        for (int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Attack(m_properties, ActionDirection.Down);
        }

        return stateType;
    }

    public override void Exit()
    {
        Debug.Log("Exit Attack");
        base.Exit();
    }
}
