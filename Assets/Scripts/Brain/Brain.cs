using UnityEngine;

public class Brain : MonoBehaviour
{
    public Commands commands;
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

    public virtual void OnDamaged(float damage)
    {

    }

    public bool IsTryingToMove()
    {
        return new Vector3(commands.rightwards, commands.upwards, commands.forwards).magnitude > 0;
    }
}
