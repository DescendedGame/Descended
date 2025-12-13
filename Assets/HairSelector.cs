using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HairSelector : MonoBehaviour
{
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
        options1.Add(GetOptionWithText("Hair1"));
        options1.Add(GetOptionWithText("Hair2"));
        options1.Add(GetOptionWithText("Hair3"));
        options1.Add(GetOptionWithText("Hair4"));
        hairDropdown.options = options1;

        browsDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options2 = new List<TMP_Dropdown.OptionData>();
        options2.Add(GetOptionWithText("Brows1"));
        options2.Add(GetOptionWithText("Brows2"));
        options2.Add(GetOptionWithText("Brows3"));
        options2.Add(GetOptionWithText("Brows4"));
        browsDropdown.options = options2;

        stacheDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options3 = new List<TMP_Dropdown.OptionData>();
        options3.Add(GetOptionWithText("Stache1"));
        options3.Add(GetOptionWithText("Stache2"));
        options3.Add(GetOptionWithText("Stache3"));
        options3.Add(GetOptionWithText("Stache4"));
        stacheDropdown.options = options3;

        sideBeardDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options4 = new List<TMP_Dropdown.OptionData>();
        options4.Add(GetOptionWithText("SideBeard1"));
        options4.Add(GetOptionWithText("SideBeard2"));
        options4.Add(GetOptionWithText("SideBeard3"));
        options4.Add(GetOptionWithText("SideBeard4"));
        sideBeardDropdown.options = options4;

        beardDropdown = Instantiate(dropdownPrefab, transform.GetChild(0)).GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown.OptionData> options5 = new List<TMP_Dropdown.OptionData>();
        options5.Add(GetOptionWithText("Beard1"));
        options5.Add(GetOptionWithText("Beard2"));
        options5.Add(GetOptionWithText("Beard3"));
        options5.Add(GetOptionWithText("Beard4"));
        beardDropdown.options = options5;
    }

    TMP_Dropdown.OptionData GetOptionWithText(string text)
    {
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
        option.text = text;
        return option;
    }

}
