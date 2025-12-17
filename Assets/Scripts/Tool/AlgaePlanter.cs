using UnityEngine;

public class AlgaePlanter : Tool
{
    [SerializeField] GameObject[] algaePrefabs;

    Transform actionPoint;

    int selectedAlgae = 0;

    public override PawnStateType StartPrimaryAction(Transform actionPoint, Commands commands)
    {
        RaycastHit hit;
        if(Physics.Raycast(actionPoint.position, actionPoint.forward, out hit, 2))
        {
            GameObject algae = Instantiate(algaePrefabs[selectedAlgae]);
            algae.transform.position = hit.point;
            algae.GetComponent<Algae>().Initialize(actionPoint.rotation);
        }
        return PawnStateType.Idle;
    }

    public override PawnStateType StartSecondaryAction(Transform actionPoint, Commands commands)
    {
        selectedAlgae += 1;
        if (selectedAlgae >= algaePrefabs.Length) selectedAlgae = 0;
        return PawnStateType.Idle;
    }

    public override void Equip(Transform pActionPoint)
    {
        base.Equip(pActionPoint);
        actionPoint = pActionPoint;
    }

    public override void Unequip()
    {
        base.Unequip();
    }

    private void Update()
    {
        // Maybe show some placeholder?
    }
}
