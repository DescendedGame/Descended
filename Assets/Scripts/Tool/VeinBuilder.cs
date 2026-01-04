using UnityEngine;

public class VeinBuilder : Tool
{

    [SerializeField] GameObject veinPrefab;

    bool isBuilding = false;

    Vein vein;
    Vein placeHolderVein;

    float creationDistance = 2;

    float startSize = 1;
    float endSize = 1;

    //Get the primary action to create a projectile
    public override PawnStateType StartPrimaryAction(Commands commands, PawnStateType stateType)
    {
        if (!isBuilding)
        {
            isBuilding = true;
            vein = Instantiate(veinPrefab, userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, Quaternion.LookRotation(-userProperties.eyeTransform.forward, userProperties.eyeTransform.up)).GetComponent<Vein>();
            vein.Generate(userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, startSize, startSize, 0, true, EndType.None, EndType.None);
        }
        else
        {
            if(userProperties.hard > vein.GetCost() && (startSize > 0 || endSize > 0))
            {
                userProperties.hard -= vein.GetCost();
                vein.Generate(userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, startSize, endSize, 0, true, EndType.None, EndType.None);
                vein.transform.SetParent(GameObject.Find("World").transform, true);
                vein.GetComponent<Collider>().enabled = true;
                GameObject.Find("World").GetComponent<World>().SaveWorld();
                vein = null;
                isBuilding = false;
            }
        }
        return stateType == PawnStateType.Grounded ? stateType : PawnStateType.Idle;
    }
    //Get the primary action to create a projectile
    public override PawnStateType HoldSecondaryAction(Commands commands, PawnStateType stateType)
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
        return stateType ==  PawnStateType.Grounded? stateType: PawnStateType.Idle;
    }

    public override void Equip()
    {
        base.Equip();
        if (placeHolderVein == null)
        {
            placeHolderVein = Instantiate(veinPrefab, userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, Quaternion.LookRotation(-userProperties.eyeTransform.forward, userProperties.eyeTransform.up)).GetComponent<Vein>();
            placeHolderVein.Generate(userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance * 0.1f, 1, 1, 0, true, EndType.None, EndType.None);
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
            vein.Generate(userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, startSize, endSize, 0, true, EndType.None, EndType.None);
        }
        else
        {
            if (placeHolderVein == null)
            {
                placeHolderVein = Instantiate(veinPrefab, userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance, Quaternion.LookRotation(-userProperties.eyeTransform.forward, userProperties.eyeTransform.up)).GetComponent<Vein>();
            }
            placeHolderVein.transform.position = userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance;
            placeHolderVein.transform.rotation = Quaternion.LookRotation(-userProperties.eyeTransform.forward, userProperties.eyeTransform.up);
            placeHolderVein.Generate(userProperties.eyeTransform.position + userProperties.eyeTransform.forward * creationDistance * 0.9f, startSize, endSize, 0, true, EndType.None, EndType.None);
        }
    }
}
