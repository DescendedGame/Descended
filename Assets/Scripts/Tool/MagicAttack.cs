using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MagicAttack", order = 1)]
public class MagicAttack : Tool
{
    public GameObject projectilePrefab; // Drag the projectile prefab in Unity Inspector

    public override PawnStateType StartAction(Transform actionPoint)
    {
        GameObject projectile = Instantiate(projectilePrefab, actionPoint.position, actionPoint.rotation);
        projectile.GetComponent<Projectile>().Initialize();
        return PawnStateType.Idle;
    }
}
