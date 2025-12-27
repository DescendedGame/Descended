using UnityEngine;

/// <summary>
/// To create a new tool, Inheret from this and add it to a prefab, add a new enum for it, and then add it to AllTools.
/// A tool decides how a pawn's primary, secondary, and tertiary actions behave.
/// </summary>
public class Tool : MonoBehaviour
{
    [SerializeField] ToolType toolType;
    [SerializeField] protected int manaCost = 0;
    [SerializeField] protected PawnProperties userProperties;
    protected bool equipped = false;

    public void Initialize(PawnProperties pawnProperties)
    {
        userProperties = pawnProperties;
        transform.parent = pawnProperties.actionPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// The primary action of this tool. The return value results in a change of its pawn's state.
    /// </summary>
    /// <param name="actionPoint"> The hand of the user, or mouth, or whatever.</param>
    /// <returns></returns>
    public virtual PawnStateType StartPrimaryAction(Commands commands, PawnStateType stateType)
    {
        return PawnStateType.Idle;
    }

    /// <summary>
    /// The secondary action of this tool. The return value results in a change of its pawn's state.
    /// </summary>
    /// <param name="actionPoint"> The hand of the user, or mouth, or whatever.</param>
    /// <returns></returns>
    public virtual PawnStateType StartSecondaryAction(Commands commands, PawnStateType stateType)
    {
        return PawnStateType.Idle;
    }

    /// <summary>
    /// The secondary hold action of this tool. The return value results in a change of its pawn's state.
    /// </summary>
    /// <param name="actionPoint"> The hand of the user, or mouth, or whatever.</param>
    /// <returns></returns>
    public virtual PawnStateType HoldSecondaryAction(Commands commands, PawnStateType stateType)
    {
        return PawnStateType.Idle;
    }

    public virtual void Equip()
    {
        equipped = true;
    }

    public virtual void Unequip()
    {
        equipped = false;
    }

    public float GetManaCost()
    {
        return manaCost;
    }

    public ToolType GetToolType()
    {
        return toolType;
    }

    public virtual void ReleaseAttack()
    {
    }

    public virtual void StopAttack()
    {
    }
}

/// <summary>
/// All tools should have a unique tool type. It is used to request its tool prefab from AllTools.
/// </summary>
public enum ToolType
{
    Dagger,
    PressurePistol,
    ChemicalSpray,
    VeinBuilder,
    AlgaePlanter,
}