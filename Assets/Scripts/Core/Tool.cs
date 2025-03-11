using UnityEngine;

/// <summary>
/// Inheret from this and add: 
/// [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WHATEVER NAME YOU WANT", order = 1)]
/// to create a new tool!
/// </summary>
public class Tool : ScriptableObject
{
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
}
