using UnityEngine;

public class MeleeWeapon : Tool
{
    public GameObject projectilePrefab; // Drag the projectile prefab in Unity Inspector

    //Get the primary action to create a projectile
    public override PawnStateType StartPrimaryAction(Commands commands, PawnStateType stateType)
    {
        GameObject projectile = Instantiate(projectilePrefab, userProperties.actionPoint.position, userProperties.actionPoint.rotation);
        projectile.GetComponent<Projectile>().Initialize();
        return PawnStateType.Idle;
    }
}
