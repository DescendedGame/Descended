using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorMixer : MonoBehaviour
{
    [SerializeField] GameObject customizationSliderPrefab;
    [SerializeField] GameObject targetDropdownPrefab;
    Slider[] colorSliders = new Slider[4];
    TMP_Dropdown targetDropdown;

    [SerializeField] Image colorRepresentation;

    public Material[] targetMaterials;

    HumanoidBodyCreator bodyCreator;

    Color skinColor;

    public string targetColor = "skin";

    private void Awake()
    {
        for (int i = 0; i < colorSliders.Length; i++)
        {
            GameObject bodySlider = Instantiate(customizationSliderPrefab, transform.GetChild(0));
            colorSliders[i] = bodySlider.GetComponent<Slider>();
            colorSliders[i].onValueChanged.AddListener(MixColor);
        }
        colorSliders[0].GetComponentInChildren<TMP_Text>().text = "                 Red";
        colorSliders[1].GetComponentInChildren<TMP_Text>().text = "                 Green";
        colorSliders[2].GetComponentInChildren<TMP_Text>().text = "                 Blue";
        colorSliders[3].GetComponentInChildren<TMP_Text>().text = "                 Opacity";
        GameObject targetDropdownObject = Instantiate(targetDropdownPrefab, transform.GetChild(0));
        targetDropdown = targetDropdownObject.GetComponent<TMP_Dropdown>();
        targetDropdown.onValueChanged.AddListener(SwitchTarget);
    }

    public void SwitchTarget(int index)
    {
        Debug.Log(targetDropdown.options[index].text);
    }

    public void SetColor(Color color, float opacity)
    {
        colorSliders[0].SetValueWithoutNotify(color.r);
        colorSliders[1].SetValueWithoutNotify(color.g);
        colorSliders[2].SetValueWithoutNotify(color.b);
        colorSliders[3].SetValueWithoutNotify(opacity);
    }

    public void MixColor(float value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }

        Color mixedColor = Color.Lerp(bodyCreator.bodySettings.skinColor, new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value), colorSliders[3].value);
        foreach (Material material in targetMaterials)
        {
            material.SetColor("_MainColor", mixedColor);
        }
        colorRepresentation.color = mixedColor;
    }
}
