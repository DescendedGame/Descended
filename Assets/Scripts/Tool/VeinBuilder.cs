using UnityEngine;

public class VeinBuilder : Tool
{

    [SerializeField] GameObject veinPrefab;

    bool isBuilding = false;

    Vein vein;
    Vein placeHolderVein;

    Transform target;
    float creationDistance = 2;

    float startSize = 1;
    float endSize = 1;

    //Get the primary action to create a projectile
    public override PawnStateType StartPrimaryAction(Transform actionPoint, Commands commands)
    {
        if (!isBuilding)
        {
            target = actionPoint;
            isBuilding = true;
            vein = Instantiate(veinPrefab, target.position + target.forward * creationDistance, Quaternion.LookRotation(-target.forward, target.up)).GetComponent<Vein>();
            vein.Generate(target.position + target.forward * creationDistance, startSize, startSize, 0, true, Vein.EndType.None, Vein.EndType.None);
        }
        else
        {
            vein.Generate(target.position + target.forward * creationDistance, startSize,endSize, 0, true, Vein.EndType.None, Vein.EndType.None);
            //vein.Save()
            vein = null;
            isBuilding = false;
        }
        return PawnStateType.Idle;
    }
    //Get the primary action to create a projectile
    public override PawnStateType HoldSecondaryAction(Transform actionPoint, Commands commands)
    {
        if (!isBuilding)
        {
            startSize += commands.look.y;
            if (startSize < 0) startSize = 0;
        }
        else
        {

            endSize += commands.look.y;
            if (endSize < 0) endSize = 0;
        }
        return PawnStateType.Idle;
    }

    public override void Equip(Transform actionPoint)
    {
        base.Equip(actionPoint);
        target = actionPoint;
        if (placeHolderVein == null)
        {
            placeHolderVein = Instantiate(veinPrefab, target.position + target.forward * creationDistance, Quaternion.LookRotation(-target.forward, target.up)).GetComponent<Vein>();
            placeHolderVein.Generate(target.position + target.forward * creationDistance * 0.1f, 1, 1, 0, true, Vein.EndType.None, Vein.EndType.None);
        }
        else
        {
            placeHolderVein.gameObject.SetActive(true);
        }
    }

    public override void Unequip()
    {
        base.Unequip();
        if(placeHolderVein != null)
        {
            placeHolderVein.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!equipped) return;
        if(vein != null)
        {
            vein.Generate(target.position + target.forward * creationDistance, startSize, endSize, 0, true, Vein.EndType.None, Vein.EndType.None);
        }
        else
        {
            if(placeHolderVein == null)
            {
                placeHolderVein = Instantiate(veinPrefab, target.position + target.forward * creationDistance, Quaternion.LookRotation(-target.forward, target.up)).GetComponent<Vein>();
            }
            placeHolderVein.transform.position = target.position + target.forward * creationDistance;
            placeHolderVein.transform.rotation = Quaternion.LookRotation(-target.forward, target.up);
            placeHolderVein.Generate(target.position + target.forward * creationDistance * 0.9f, 1, 1, 0, true, Vein.EndType.None, Vein.EndType.None);
        }
    }
}
