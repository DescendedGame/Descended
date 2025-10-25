using UnityEngine;

[System.Serializable]
public class DescendedInitializer : MonoBehaviour
{
    [SerializeField] static GameObject waveVariables;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        var go = new GameObject();
        go.AddComponent<WaveVariables>();
        go.name = "WaveVariables";
        DontDestroyOnLoad(go);
    }
}
