using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class DefendGroundedPawnState : PawnState
{

    bool grounded = false;

    public DefendGroundedPawnState()
    {
        stateType = PawnStateType.DefendGrounded;
    }

    public override void Enter()
    {
        m_properties.eyeTransform.rotation = Quaternion.LookRotation(m_properties.eyeTransform.forward, Vector3.up);
        grounded = true;
        Debug.Log("Enter DefendGrounded");
    }

    public override PawnStateType Update()
    {

        if (m_brain.commands.upwards > 0 || !grounded)
        {
            return PawnStateType.Defend;
        }


        if (!m_brain.commands.secondaryHold)
        {
            return PawnStateType.Grounded;
        }

        if (m_brain.commands.primary)
        {
            //PUSH
        }

        UpdateRotationLockedY();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].DefendGrounded(m_brain.commands, m_properties, m_brain.commands.actionDirection);
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
        Debug.Log("Exit DefendGrounded");
    }
}
