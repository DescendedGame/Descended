using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] GameObject characterCreationButtonPrefab;
    [SerializeField] GameObject brainDeadHuman;
    [SerializeField] Transform characterPivot;
    [SerializeField] Material basicInGameObject;

    public HumanBodySettings selectedSettings;

    string[] names;
    Button[] selectableCharacterButtons;

    GameObject[] bodies;

    private void Awake()
    {
        string path;
        if (Application.isEditor)
        {
            path = "Assets/saves/avatars";
        }
        else
        {
            path = Application.dataPath + "/saves/avatars";
        }
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();

        List<string> nameList = new List<string>();
        foreach (FileInfo file in fileInfo)
        {
            if(file.Name.EndsWith(".json"))
            {
                nameList.Add(file.Name.Split(".")[0]);
            }
        }
        names = nameList.ToArray();
        selectableCharacterButtons = new Button[names.Length];
        bodies = new GameObject[names.Length];
        
        for(int i = 0; i < names.Length; i++)
        {
            int index = i;
            selectableCharacterButtons[i] = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
            selectableCharacterButtons[i].GetComponentInChildren<TMP_Text>().text = names[i];
            selectableCharacterButtons[i].onClick.AddListener(delegate { SelectBody(index); });
            bodies[i] = LoadBody(names[i]);
            if (i == 0)
            { bodies[i].SetActive(true);
                FindFirstObjectByType<LocalPlayerData>().settings = bodies[i].GetComponent<HumanoidBodyCreator>().bodySettings;
            }
        }
    }


    public GameObject LoadBody(string name)
    {
        string path;
        if (Application.isEditor)
        {
            path = "Assets/saves/avatars/";
        }
        else
        {
            path = Application.dataPath + "/saves/avatars/";
        }
        string myString = File.ReadAllText(path + name + ".json");
        HumanBodySettings bodySettings = (HumanBodySettings)JsonUtility.FromJson(myString, typeof(HumanBodySettings));
        bodySettings.basicInGameObject ??= basicInGameObject;
        GameObject body = Instantiate(brainDeadHuman, characterPivot);
        body.GetComponent<HumanoidBodyCreator>().bodySettings = bodySettings;
        body.SetActive(false);
        return body;
    }

    void SelectBody(int index)
    {
        foreach(GameObject body in bodies)
        {
            body.SetActive(false);
        }
        bodies[index].SetActive(true);
        FindFirstObjectByType<LocalPlayerData>().settings = bodies[index].GetComponent<HumanoidBodyCreator>().bodySettings;
    }
}
