using UnityEngine;

public class AlgaePlanter : Tool
{
    [SerializeField] GameObject[] algaePrefabs;

    int selectedAlgae = 0;

    public override PawnStateType StartPrimaryAction(Commands commands, PawnStateType stateType)
    {
        RaycastHit hit;
        if(Physics.Raycast(userProperties.eyeTransform.position, userProperties.eyeTransform.forward, out hit, 2))
        {
            GameObject algae = Instantiate(algaePrefabs[selectedAlgae]);
            algae.transform.position = hit.point;
            algae.GetComponent<Algae>().Initialize(userProperties.eyeTransform.rotation);
        }
        return stateType == PawnStateType.Grounded ? stateType : PawnStateType.Idle;
    }

    public override PawnStateType StartSecondaryAction(Commands commands, PawnStateType stateType)
    {
        selectedAlgae += 1;
        if (selectedAlgae >= algaePrefabs.Length) selectedAlgae = 0;
        return stateType == PawnStateType.Grounded ? stateType : PawnStateType.Idle;
    }

    private void Update()
    {
        // Maybe show some placeholder?
    }
}
