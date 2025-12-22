using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A "living" game object. Needs a brain to function, can carry and use tools. Create a prefab variant from the prefab "Pawn" to get started on a new pawn!
/// </summary>
public class Pawn : Attackable
{
    [SerializeField] PawnProperties m_properties;
    [SerializeField] protected Brain m_brain;
    //[SerializeField] ParticleSystem m_glitter;

    /// <summary>
    /// This will call its Updated() in Update(). Can result in a change then, or for example when this pawn takes damage.
    /// </summary>
    PawnState currentState;

    /// <summary>
    /// Contains all the pawn's states. Pass the wanted state type to get a reference to a specific state.
    /// </summary>
    Dictionary<PawnStateType, PawnState> m_lookUpState = new Dictionary<PawnStateType, PawnState>();

    protected override void Awake()
    {
        base.Awake();

        GetComponent<BodyCreator>().CreateBody(out m_properties.m_pivot, out m_properties.eyeTransform);
        m_properties.actionPoint = m_properties.eyeTransform;
        m_properties.bodyParts = GetComponentsInChildren<BodyPart>(); // This should already be in place when the character is created. Or maybe this is simpler (but less optimized)
        RememberBodyTransforms();
        m_brain = GetComponent<Brain>();
        
        InitializeTools();

        // Add the states
        new IdlePawnState().Initialize(m_brain, m_properties, m_lookUpState);
        new SprintPawnState().Initialize(m_brain, m_properties, m_lookUpState);
        new ToppledPawnState().Initialize(m_brain, m_properties, m_lookUpState);
        new GroundedPawnState().Initialize(m_brain, m_properties, m_lookUpState);

        // Set the initial state
        currentState = m_lookUpState[PawnStateType.Idle];
        currentState.Enter();
    }

    private void Start()
    {
        m_brain.Initialize(m_properties);
    }

    void InitializeTools()
    {
        m_properties.tools = new Tool[m_properties.toolTypes.Length];
        for(int i = 0; i < m_properties.toolTypes.Length; i++)
        {
            GameObject tool = Instantiate(m_properties.toolStorage.GetTool(m_properties.toolTypes[i]), transform);
            m_properties.tools[i] = tool.GetComponent<Tool>();
        }
    }

    protected virtual void Update()
    {
        // All pawns trigger small bioluminescent things around them when they move.
        //var emission = m_glitter.emission;
        //emission.rateOverTime = m_properties.m_physics.linearVelocity.magnitude * 5;
        m_brain.UpdateCommands();

        while (true)
        {
            PawnStateType nextState = currentState.Update();

            if (nextState == currentState.stateType) break;

            SetState(nextState);
        }

        // Remember the orientation of body parts so they can move smoothly towards their parent next frame if they want to.
        RememberBodyTransforms();
    }

    void RememberBodyTransforms()
    {
        for(int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].RememberTransform();
        }
    }

    void SetState(PawnStateType nextState)
    {
        currentState.Exit();
        currentState = m_lookUpState[nextState];
        currentState.Enter();
    }

    protected virtual void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public override void Hit(Hazard damage)
    {
        base.Hit(damage);

        // alert the brain of the hazard
        m_brain.OnDamaged(damage);

        // add the push force from the hazard
        m_properties.m_physics.AddForce(damage.pushForce, ForceMode.Impulse);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //Now the pawn always becomes toppled by damage...
        SetState(PawnStateType.Toppled);
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
        m_properties.eyeTransform.rotation *= Quaternion.AngleAxis(m_brain.commands.look.magnitude, Vector3.right * normalizedLook.y + Vector3.up * normalizedLook.x);

        m_properties.roll_speed += m_brain.commands.roll * m_properties.roll_acc * Time.deltaTime;
        m_properties.roll_speed = m_properties.roll_speed * Mathf.Pow(0.5f, Time.deltaTime * 18);
        m_properties.eyeTransform.rotation *= Quaternion.AngleAxis(m_properties.roll_speed * Time.deltaTime, Vector3.forward);
    }

    protected virtual void UpdateRotationLockedY()
    {
        m_properties.eyeTransform.rotation = Quaternion.AngleAxis(m_brain.commands.look.x, Vector3.up)* m_properties.eyeTransform.rotation;

        float looky = m_brain.commands.look.y;
        float angleDistance = Vector3.Angle(m_properties.eyeTransform.forward, Vector3.up);

        if(looky < 0) //camera looking upwards
        {
            float dAngle = angleDistance + looky;
            if (dAngle < 0) looky -= dAngle; 
        }

        if (looky > 0) //camera looking upwards
        {
            float dAngle = angleDistance + looky -180;
            if (dAngle > 0) looky -= dAngle;
        }

        m_properties.eyeTransform.rotation *= Quaternion.AngleAxis(looky, Vector3.right);
    }

    /// <summary>
    /// The physical behaviour of this state, called in the pawn's FixedUpdate().
    /// </summary>
    public virtual void FixedUpdate()
    {
        m_properties.m_physics.AddForce((m_properties.eyeTransform.forward * m_brain.commands.forwards + 
            m_properties.eyeTransform.right * m_brain.commands.rightwards + 
            m_properties.eyeTransform.up * m_brain.commands.upwards).normalized * 
            m_properties.m_swim_force);
    }

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public virtual void Exit()
    {

    }
}