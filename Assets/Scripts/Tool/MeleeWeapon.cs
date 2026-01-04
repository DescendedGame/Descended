using UnityEngine;

public class MeleeWeapon : Tool
{
    Hazard dangerZone;

    public override void Initialize(PawnProperties pawnProperties)
    {
        base.Initialize(pawnProperties);
        dangerZone = GetComponent<Hazard>();
    }

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

    public override PawnStateType StartSecondaryAction(Commands commands, PawnStateType stateType)
    {
        return HoldSecondaryAction(commands, stateType);
    }

    public override void ReleaseAttack()
    {
        dangerZone.ResetHitCounter();
        GetComponentInChildren<BoxCollider>().enabled = true;
    }

    public override void StopAttack()
    {
        GetComponentInChildren<BoxCollider>().enabled = false;
    }
}
