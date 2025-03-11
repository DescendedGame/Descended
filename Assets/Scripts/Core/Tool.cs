using UnityEngine;

public class Tool : ScriptableObject
{
    [SerializeField] protected int manaCost = 0;

    public virtual PawnStateType StartAction(Transform actionPoint)
    {
        return PawnStateType.Idle;
    }

    public float GetManaCost()
    {
        return manaCost;
    }
}
