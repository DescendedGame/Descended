using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class AttackGroundedPawnState : PawnState
{
    bool grounded = false;
    ActionDirection actionDirection;

    public AttackGroundedPawnState()
    {
        stateType = PawnStateType.AttackGrounded;
    }

    public override void Enter()
    {
        actionDirection = m_brain.commands.actionDirection;
        m_properties.eyeTransform.rotation = Quaternion.LookRotation(m_properties.eyeTransform.forward, Vector3.up);
        grounded = true;
        base.Enter();
    }

    public override PawnStateType Update()
    {
        if (m_brain.commands.upwards > 0 || !grounded)
        {
            return PawnStateType.Attack;
        }

        m_properties.attackTimer -= Time.deltaTime;
        if (m_properties.attackTimer <= 0)
        {
            return PawnStateType.Grounded;
        }

        // Tool usage can result in a different state!
        if (m_brain.commands.secondary)
        {
            return PawnStateType.Idle; //then it will go to whatever the secondary action does from there.
        }

        UpdateRotationLockedY();

        // Move all body parts idly
        for (int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].AttackGrounded(m_properties, actionDirection);
        }

        return stateType;
    }

    public override void FixedUpdate()
    {
        RaycastHit hit;
        string[] layerMaskNames = new string[2];
        layerMaskNames[0] = "Solid";
        layerMaskNames[1] = "Shifting";

        if (Physics.SphereCast(m_properties.m_pivot.position, 0.25f, -Vector3.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
        {
            Quaternion flatHeadRotation = m_properties.GetGroundedRotation();

            Vector3 forceToAdd = (flatHeadRotation * Vector3.forward * m_brain.commands.forwards +
            flatHeadRotation * Vector3.right * m_brain.commands.rightwards).normalized *
            m_properties.m_swim_force;


            m_properties.m_physics.AddForce(forceToAdd);
            m_properties.attemptedMoveDirection = forceToAdd;

            if (hit.distance > m_properties.length * 0.5f + 0.05f)
            {
                m_properties.m_physics.AddForce(Vector3.up * -1000);
            }
            else if (hit.distance < m_properties.length * 0.5f - 0.05f)
            {
                m_properties.m_physics.AddForce(Vector3.up * 1000);
            }
        }
        else
        {
            grounded = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
