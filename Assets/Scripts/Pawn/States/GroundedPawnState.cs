using UnityEngine;

/// <summary>
/// The default state used for basic movement and whatnot.
/// </summary>
public class GroundedPawnState : PawnState
{

    bool grounded = false;

    public GroundedPawnState()
    {
        stateType = PawnStateType.Grounded;
    }

    public override void Enter()
    {
        m_properties.eyeTransform.rotation = Quaternion.LookRotation(m_properties.eyeTransform.forward, Vector3.up);
        grounded = true;
    }

    public override PawnStateType Update()
    {
        // Go to sprint if sprint conditions are met.
        if (m_brain.commands.sprint && m_brain.IsTryingToMove())
        {
            return PawnStateType.Sprint;
        }

        // Go to sprint if sprint conditions are met.
        if (m_brain.commands.upwards > 0 || !grounded)
        {
            return PawnStateType.Idle;
        }

        // grounded placement
        RaycastHit hit;
        string[] layerMaskNames = new string[2];
        layerMaskNames[0] = "Solid";
        layerMaskNames[1] = "Shifting";


        // Tool selection
        if (m_properties.selectedToolIndex != m_brain.commands.selected)
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

        UpdateRotationLockedY();
        
        // Move all body parts idly
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Grounded(m_properties, ActionDirection.Down);
        }

        return stateType;
    }

    public override void FixedUpdate()
    {
        RaycastHit hit;
        string[] layerMaskNames = new string[2];
        layerMaskNames[0] = "Solid";
        layerMaskNames[1] = "Shifting";

        if(Physics.SphereCast(m_properties.m_pivot.position, 0.25f, -Vector3.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
        {
            Quaternion flatHeadRotation = m_properties.GetGroundedRotation();

            Vector3 forceToAdd = (flatHeadRotation * Vector3.forward * m_brain.commands.forwards +
            flatHeadRotation * Vector3.right * m_brain.commands.rightwards).normalized *
            m_properties.m_swim_force;


            m_properties.m_physics.AddForce(forceToAdd);
            m_properties.attemptedMoveDirection = forceToAdd;

            if (hit.distance > 0.7f)
            {
                m_properties.m_physics.AddForce(Vector3.up * -1000);
            }
            else if(hit.distance < 0.6f)
            {
                m_properties.m_physics.AddForce(Vector3.up * 1000);
            }
        }
        else
        {
            grounded = false;
        }
    }
}
