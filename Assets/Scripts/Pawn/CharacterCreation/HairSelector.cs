using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HairSelector : MonoBehaviour
{
    HumanoidBodyCreator bodyCreator;
    [SerializeField] GameObject dropdownPrefab;

    TMP_Dropdown hairDropdown;
    TMP_Dropdown browsDropdown;
    TMP_Dropdown stacheDropdown;
    TMP_Dropdown sideBeardDropdown;
    TMP_Dropdown beardDropdown;

    private void Awake()
    {
        hairDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options1 = new List<TMP_Dropdown.OptionData>();
        options1.Add(GetOptionWithText("Nothing"));
        options1.Add(GetOptionWithText("Hair1"));
        options1.Add(GetOptionWithText("Hair2"));
        options1.Add(GetOptionWithText("Hair3"));
        options1.Add(GetOptionWithText("Hair4"));
        hairDropdown.options = options1;
        hairDropdown.onValueChanged.AddListener(SetHairOption);

        browsDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options2 = new List<TMP_Dropdown.OptionData>();
        options2.Add(GetOptionWithText("Nothing"));
        options2.Add(GetOptionWithText("Brows1"));
        options2.Add(GetOptionWithText("Brows2"));
        options2.Add(GetOptionWithText("Brows3"));
        options2.Add(GetOptionWithText("Brows4"));
        browsDropdown.options = options2;
        browsDropdown.onValueChanged.AddListener(SetBrowsOption);

        stacheDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options3 = new List<TMP_Dropdown.OptionData>();
        options3.Add(GetOptionWithText("Nothing"));
        options3.Add(GetOptionWithText("Stache1"));
        options3.Add(GetOptionWithText("Stache2"));
        options3.Add(GetOptionWithText("Stache3"));
        options3.Add(GetOptionWithText("Stache4"));
        stacheDropdown.options = options3;
        stacheDropdown.onValueChanged.AddListener(SetStacheOption);

        sideBeardDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options4 = new List<TMP_Dropdown.OptionData>();
        options4.Add(GetOptionWithText("Nothing"));
        options4.Add(GetOptionWithText("SideBeard1"));
        options4.Add(GetOptionWithText("SideBeard2"));
        options4.Add(GetOptionWithText("SideBeard3"));
        options4.Add(GetOptionWithText("SideBeard4"));
        sideBeardDropdown.options = options4;
        sideBeardDropdown.onValueChanged.AddListener(SetSideBeardOption);

        beardDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options5 = new List<TMP_Dropdown.OptionData>();
        options5.Add(GetOptionWithText("Nothing"));
        options5.Add(GetOptionWithText("Beard1"));
        options5.Add(GetOptionWithText("Beard2"));
        options5.Add(GetOptionWithText("Beard3"));
        options5.Add(GetOptionWithText("Beard4"));
        beardDropdown.options = options5;
        beardDropdown.onValueChanged.AddListener(SetBeardOption);
    }

    private void Start()
    {
        ReadValues();
    }

    TMP_Dropdown.OptionData GetOptionWithText(string text)
    {
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
        option.text = text;
        return option;
    }

    void SetHairOption(int value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.headSettings.hairStyle = value;
        bodyCreator.CreateHead();
    }

    void SetBrowsOption(int value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.headSettings.browStyle = value;
        bodyCreator.CreateHead();

    }

    void SetStacheOption(int value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.headSettings.stacheStyle = value;
        bodyCreator.CreateHead();

    }

    void SetSideBeardOption(int value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.headSettings.sideBeardStyle = value;
        bodyCreator.CreateHead();

    }

    void SetBeardOption(int value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.headSettings.beardStyle = value;
        bodyCreator.CreateHead();
    }

    void ReadValues()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }

        hairDropdown.SetValueWithoutNotify(bodyCreator.bodySettings.headSettings.hairStyle);
        browsDropdown.SetValueWithoutNotify(bodyCreator.bodySettings.headSettings.browStyle);
        stacheDropdown.SetValueWithoutNotify(bodyCreator.bodySettings.headSettings.stacheStyle);
        sideBeardDropdown.SetValueWithoutNotify(bodyCreator.bodySettings.headSettings.sideBeardStyle);
        beardDropdown.SetValueWithoutNotify(bodyCreator.bodySettings.headSettings.beardStyle);
    }

    public void Randomize()
    {
        bodyCreator.bodySettings.headSettings.hairStyle=Random.Range(0, hairDropdown.options.Count);
        bodyCreator.bodySettings.headSettings.browStyle = Random.Range(0, browsDropdown.options.Count);
        bodyCreator.bodySettings.headSettings.stacheStyle = Random.Range(0, stacheDropdown.options.Count);
        bodyCreator.bodySettings.headSettings.sideBeardStyle = Random.Range(0, sideBeardDropdown.options.Count);
        bodyCreator.bodySettings.headSettings.beardStyle = Random.Range(0, beardDropdown.options.Count);
    }
}
