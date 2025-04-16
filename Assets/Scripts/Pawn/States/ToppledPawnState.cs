using UnityEngine;

public class ToppledPawnState : PawnState
{
    /// <summary>
    /// The remaining time until the pawn stands again.
    /// </summary>
    float timeToRaise;

    public ToppledPawnState()
    {
        stateType = PawnStateType.Toppled;
    }

    public override void Enter()
    {
        // Sets for how long the pawn is toppled.
        timeToRaise = 0.5f;
    }

    public override PawnStateType Update()
    {
        // As timeToRaise is not reduced before this check, the pawn will be stuck for atleast one frame, no matter how laggy it is.
        if (timeToRaise <= 0)
        {
            return PawnStateType.Idle;
        }
        timeToRaise -= Time.deltaTime;

        for (int i = 0; i < m_properties.bodyParts.Length; i++)
        {
            m_properties.bodyParts[i].Toppled();
        }

        return stateType;
    }

    public override void FixedUpdate() 
    {
        // Just overrides the base's one with one that doesn't try to move, the focus is on getting up! 
    }
}
