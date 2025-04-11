using UnityEngine;

[System.Serializable]
public class DescendedInitializer : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        var go = new GameObject();
        go.name = "Initializers";
        DontDestroyOnLoad(go);
    }
}
