using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;



public class Pawn : Attackable
{
    [SerializeField] PawnAttributes m_attributes;

    [SerializeField] protected Brain m_brain;
    [SerializeField] ParticleSystem m_glitter;

    PawnState currentState;
    Dictionary<PawnStateType, PawnState> m_lookUpState = new Dictionary<PawnStateType, PawnState>();

    protected override void Awake()
    {
        base.Awake();
        m_brain.Initialize(m_attributes);
        currentState = new IdlePawnState();
        currentState.Initialize(m_brain, m_attributes, m_lookUpState);
        currentState.Enter();
    }

    protected virtual void Update()
    {
        var emission = m_glitter.emission;
        emission.rateOverTime = m_attributes.m_physics.linearVelocity.magnitude * 5;
        m_brain.ZeroCommands();

        while (true)
        {
            PawnStateType nextState = currentState.Update();
            if (nextState == currentState.stateType) break;

            currentState.Exit();
            currentState = m_lookUpState[nextState];
            currentState.Enter();
        }
    }


    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}

public enum PawnStateType
{
    Idle,
    Prepare,
    Attack,
    Defend,
    Run,
    Interact,
}

public abstract class PawnState
{
    public PawnStateType stateType;
    Brain m_brain;
    PawnAttributes m_attributes;

    protected PawnState() { }

    public virtual void Initialize(Brain brain, PawnAttributes attributes, Dictionary<PawnStateType, PawnState> lookUpState)
    {
        m_brain = brain;
        m_attributes = attributes;
        lookUpState.Add(stateType, this);
    }

    public virtual void Enter()
    {

    }

    public virtual PawnStateType Update()
    {
        m_brain.UpdateCommands();
        UpdateRotation();

        return this.stateType;
    }
    protected virtual void UpdateRotation()
    {
        Vector3 normalizedLook = m_brain.commands.look.normalized;
        m_attributes.m_pivot.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

        m_attributes.roll_speed += m_brain.commands.roll * m_attributes.roll_acc * Time.deltaTime;
        m_attributes.roll_speed = m_attributes.roll_speed * Mathf.Pow(0.5f, Time.deltaTime * 18);
        m_attributes.m_pivot.rotation *= Quaternion.AngleAxis(m_attributes.roll_speed * Time.deltaTime, Vector3.forward);
    }

    public virtual void FixedUpdate()
    {
        m_attributes.m_physics.AddForce((m_attributes.m_pivot.forward * m_brain.commands.forwards + 
            m_attributes.m_pivot.right * m_brain.commands.rightwards + 
            m_attributes.m_pivot.up * m_brain.commands.upwards).normalized * 
            m_attributes.m_swim_force);
    }

    public virtual void Exit()
    {

    }
}