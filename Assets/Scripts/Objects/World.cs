using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public struct WorldSettings{
    public VeinSettings[] veinSettings;
}

[System.Serializable]
public struct VeinSettings
{
    public Vector3 start_position;
    public Vector3 end_position;
    public Quaternion start_rotation;
    public float end_rotation;
    public float start_size;
    public float end_size;
    public bool is_tunnel;
    public EndType start_type;
    public EndType end_type;
}

[System.Serializable]
public enum EndType
{
    Sphere,
    None,
}

public class World : MonoBehaviour
{
    public string worldName;
    WorldSettings settings;
    [SerializeField] GameObject veinPrefab;

    private void Awake()
    {
        LoadWorld();
    }

    public void SaveWorld()
    {
        Vein[] allVeins = GetComponentsInChildren<Vein>();

        List<VeinSettings> allSettings = new List<VeinSettings>();

        foreach (Vein vein in allVeins)
        {
            allSettings.Add(vein.GetSettings());
        }
        settings.veinSettings = allSettings.ToArray();
        string jsonData = JsonUtility.ToJson(settings);
        if (Application.isEditor)
        {
            Directory.CreateDirectory("Assets/saves/worlds");
            System.IO.File.Delete("Assets/saves/worlds/" + worldName + ".json");
            System.IO.File.WriteAllText("Assets/saves/worlds/" + worldName + ".json", jsonData);
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/saves/worlds");
            System.IO.File.Delete(Application.dataPath + "/saves/worlds/" + worldName + ".json");
            System.IO.File.WriteAllText(Application.dataPath + "/saves/worlds/" + worldName + ".json", jsonData);
        }
    }

    public void LoadWorld()
    {
        string path;
        if (Application.isEditor)
        {
            path = "Assets/saves/worlds/";
        }
        else
        {
            path = Application.dataPath + "/saves/worlds/";
        }
        string myString = File.ReadAllText(path + worldName + ".json");
        settings = (WorldSettings)JsonUtility.FromJson(myString, typeof(WorldSettings));
        foreach (VeinSettings vein in settings.veinSettings)
        {
            GameObject veinObject = Instantiate(veinPrefab, transform);
            veinObject.GetComponent<Vein>().GenerateFromSettings(vein);
        }
    }
}
