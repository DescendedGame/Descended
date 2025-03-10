using UnityEngine;

public class Brain : MonoBehaviour
{
    public Commands commands;
    protected PawnAttributes m_attributes;

    public virtual void Initialize(PawnAttributes attributes)
    {
        ZeroCommands();
        m_attributes = attributes;
    }
    public virtual void UpdateCommands()
    {

    }

    public virtual void ZeroCommands()
    {
        commands.forwards = 0;
        commands.rightwards = 0;
        commands.look = Vector2.zero;
        commands.primary = false;
        commands.roll = 0;
    }
}
