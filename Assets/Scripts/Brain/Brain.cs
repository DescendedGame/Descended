using UnityEngine;

/// <summary>
/// That which controls a pawn.
/// </summary>
public class Brain : MonoBehaviour
{
    //Should maybe have this private and only send out a copy with Get so that other's can't alter something's commands.
    public Commands commands;

    /// <summary>
    /// Contains vital data of the pawn, important for AI and GUI!
    /// </summary>
    protected PawnProperties m_properties;

    public virtual void Initialize(PawnProperties properties)
    {
        ZeroCommands();
        m_properties = properties;
    }
    public virtual void UpdateCommands()
    {
        ZeroCommands();
    }

    /// <summary>
    /// Most commands are intended to be set every frame, and should be set to default before that.
    /// </summary>
    public virtual void ZeroCommands()
    {
        commands.forwards = 0;
        commands.rightwards = 0;
        commands.upwards = 0;
        commands.look = Vector2.zero;
        commands.primary = false;
        commands.secondary = false;
        commands.tertiary = false;
        commands.primaryHold = false;
        commands.secondaryHold = false;
        commands.tertiaryHold = false;
        commands.sprint = false;
        commands.roll = 0;
    }

    /// <summary>
    /// GUI and AI should be able to react to when the pawn is hit.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnDamaged(Hazard damage)
    {

    }

    public bool IsTryingToMove()
    {
        return new Vector3(commands.rightwards, commands.upwards, commands.forwards).magnitude > 0;
    }
}
