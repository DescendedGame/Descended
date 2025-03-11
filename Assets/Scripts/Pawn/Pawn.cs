using System.Collections.Generic;
using UnityEngine;

public class Pawn : Attackable
{
    [SerializeField] PawnProperties m_properties;

    [SerializeField] protected Brain m_brain;
    [SerializeField] ParticleSystem m_glitter;

    PawnState currentState;
    [SerializeField] Dictionary<PawnStateType, PawnState> m_lookUpState = new Dictionary<PawnStateType, PawnState>();

    protected override void Awake()
    {
        base.Awake();
        m_brain.Initialize(m_properties);
        currentState = new IdlePawnState();
        currentState.Initialize(m_brain, m_properties, m_lookUpState);
        new SprintPawnState().Initialize(m_brain, m_properties, m_lookUpState);
        new ToppledPawnState().Initialize(m_brain, m_properties, m_lookUpState);
        currentState.Enter();
    }

    protected virtual void Update()
    {
        var emission = m_glitter.emission;
        emission.rateOverTime = m_properties.m_physics.linearVelocity.magnitude * 5;
        m_brain.UpdateCommands();

        while (true)
        {
            PawnStateType nextState = currentState.Update();
            if (nextState == currentState.stateType) break;

            SetState(nextState);
        }
    }
    void SetState(PawnStateType nextState)
    {
        currentState.Exit();
        currentState = m_lookUpState[nextState];
        currentState.Enter();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        SetState(PawnStateType.Toppled);
    }

    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}

public abstract class PawnState
{
    public PawnStateType stateType;
    protected Brain m_brain;
    protected PawnProperties m_properties;

    /// <summary>
    /// In a state's public constructor, define its PawnStateType!
    /// </summary>
    protected PawnState() { }

    /// <summary>
    /// Use this to set up the state properly.
    /// </summary>
    public virtual void Initialize(Brain brain, PawnProperties properties, Dictionary<PawnStateType, PawnState> lookUpState)
    {
        m_brain = brain;
        m_properties = properties;
        lookUpState.Add(stateType, this);
    }

    /// <summary>
    /// Called when this state is entered.
    /// </summary>
    public virtual void Enter()
    {

    }

    /// <summary>
    /// Called in the pawn's Update() and makes the pawn switch state to match its return value.
    /// First check if conditions are met to switch state, if not, do the update logic!
    /// </summary>
    /// <returns></returns>
    public virtual PawnStateType Update()
    {
        UpdateRotation();
        return this.stateType;
    }

    protected virtual void UpdateRotation()
    {
        Vector3 normalizedLook = m_brain.commands.look.normalized;
        m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

        m_properties.roll_speed += m_brain.commands.roll * m_properties.roll_acc * Time.deltaTime;
        m_properties.roll_speed = m_properties.roll_speed * Mathf.Pow(0.5f, Time.deltaTime * 18);
        m_properties.m_pivot.rotation *= Quaternion.AngleAxis(m_properties.roll_speed * Time.deltaTime, Vector3.forward);
    }

    /// <summary>
    /// The physical behaviour of this state, called in the pawn's FixedUpdate().
    /// </summary>
    public virtual void FixedUpdate()
    {
        m_properties.m_physics.AddForce((m_properties.m_pivot.forward * m_brain.commands.forwards + 
            m_properties.m_pivot.right * m_brain.commands.rightwards + 
            m_properties.m_pivot.up * m_brain.commands.upwards).normalized * 
            m_properties.m_swim_force);
    }

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public virtual void Exit()
    {

    }
}