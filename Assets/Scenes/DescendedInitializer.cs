using UnityEngine;

[System.Serializable]
public class DescendedInitializer : MonoBehaviour
{
    [SerializeField] static GameObject waveVariables;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        var go1 = new GameObject();
        go1.AddComponent<WaveVariables>();
        go1.name = "WaveVariables";
        DontDestroyOnLoad(go1);

        var go2 = new GameObject();
        go2.AddComponent<LocalPlayerData>();
        go2.name = "LocalPlayerData";
        DontDestroyOnLoad(go2);

    }
}
