using UnityEngine;

/// <summary>
/// To create a new tool, Inheret from this and add it to a prefab, add a new enum for it, and then add it to AllTools.
/// A tool decides how a pawn's primary, secondary, and tertiary actions behave.
/// </summary>
public class Tool : MonoBehaviour
{
    [SerializeField] ToolType toolType;
    [SerializeField] protected int manaCost = 0;

    /// <summary>
    /// The primary action of this tool. The return value results in a change of its pawn's state.
    /// </summary>
    /// <param name="actionPoint"> The hand of the user, or mouth, or whatever.</param>
    /// <returns></returns>
    public virtual PawnStateType StartPrimaryAction(Transform actionPoint)
    {
        return PawnStateType.Idle;
    }

    public float GetManaCost()
    {
        return manaCost;
    }

    public ToolType GetToolType()
    {
        return toolType;
    }

}

/// <summary>
/// All tools should have a unique tool type. It is used to request its tool prefab from AllTools.
/// </summary>
public enum ToolType
{
    PressurePistol,
    ChemicalSpray,
}