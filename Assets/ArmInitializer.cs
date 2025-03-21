using UnityEngine;

[ExecuteInEditMode]
public class ArmInitializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        InitializeRecursively(transform);
    }

    public void InitializeRecursively(Transform current)
    {
        ArmGenerator generator = current.GetComponent<ArmGenerator>();
        if (generator!= null)
        {
            generator.Initialize(this);
        }
        for(int i = 0; i < current.childCount; i++)
        {
            InitializeRecursively(current.GetChild(i));
        }
    }
}
