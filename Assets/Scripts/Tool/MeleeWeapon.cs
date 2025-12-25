using UnityEngine;

public class MeleeWeapon : Tool
{
    public GameObject projectilePrefab; // Drag the projectile prefab in Unity Inspector

    //Get the primary action to create a projectile
    public override PawnStateType StartPrimaryAction(Commands commands, PawnStateType stateType)
    {
        userProperties.prepareTimer = 1;
        userProperties.attackTimer = 1;
        
        if (stateType == PawnStateType.Idle)
        {
            return PawnStateType.Prepare;
        }
        else return PawnStateType.PrepareGrounded;
    }

    public override PawnStateType HoldSecondaryAction(Commands commands, PawnStateType stateType)
    {
        if (stateType == PawnStateType.Idle)
        {
            return PawnStateType.Defend;
        }
        else return PawnStateType.DefendGrounded;
    }
}
