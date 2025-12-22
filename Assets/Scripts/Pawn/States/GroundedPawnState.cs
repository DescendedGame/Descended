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
            m_properties.bodyParts[i].Grounded(m_brain.GetDesiredMovementDirection(), ActionDirection.Down);
        }

        return stateType;
    }

    public override void FixedUpdate()
    {
        RaycastHit hit;
        string[] layerMaskNames = new string[2];
        layerMaskNames[0] = "Solid";
        layerMaskNames[1] = "Shifting";

        if(Physics.SphereCast(m_properties.eyeTransform.position, 0.25f, -Vector3.up, out hit, 2, LayerMask.GetMask(layerMaskNames)))
        {
            m_properties.m_physics.AddForce((Vector3.ProjectOnPlane(m_properties.eyeTransform.forward, hit.normal).normalized * m_brain.commands.forwards +
            Vector3.ProjectOnPlane(m_properties.eyeTransform.right, hit.normal).normalized * m_brain.commands.rightwards).normalized *
            m_properties.m_swim_force);

            if(hit.distance > 0.75f)
            {
                m_properties.m_physics.AddForce(Vector3.up * -1000);
            }
            else if(hit.distance < 0.65f)
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
