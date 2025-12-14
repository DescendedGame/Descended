using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class ColorMixer : MonoBehaviour
{
    [SerializeField] GameObject customizationSliderPrefab;
    [SerializeField] GameObject targetDropdownPrefab;
    Slider[] colorSliders = new Slider[4];
    TMP_Dropdown targetDropdown;

    [SerializeField] Image colorRepresentation;

    public Material[] targetMaterials;

    HumanoidBodyCreator bodyCreator;

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

    private void Start()
    {
        ReadValues();
    }

    public void SwitchTarget(int index)
    {
        ReadValues();
    }

    public void SetColor(Color color)
    {
        colorSliders[0].SetValueWithoutNotify(color.r);
        colorSliders[1].SetValueWithoutNotify(color.g);
        colorSliders[2].SetValueWithoutNotify(color.b);
        colorSliders[3].SetValueWithoutNotify(color.a);
        colorRepresentation.color = color;
    }

    public void ReadValues()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        switch (targetDropdown.options[targetDropdown.value].text)
        {
            case "Skin":
                SetColor(bodyCreator.bodySettings.skinColor);
                break;

            case "Hair":
                SetColor(bodyCreator.bodySettings.hairColor);
                break;

            case "Eye Lids":
                SetColor(bodyCreator.bodySettings.headSettings.eyeLidColor);
                break;

            case "Sclera":
                SetColor(bodyCreator.bodySettings.headSettings.scleraColor);
                break;

            case "Iris":
                SetColor(bodyCreator.bodySettings.headSettings.irisColor);
                break;

            case "Pupil":
                SetColor(bodyCreator.bodySettings.headSettings.pupilColor);
                break;

            case "Makeup":
                SetColor(bodyCreator.bodySettings.headSettings.makeupColor);
                break;

            case "Lips":
                SetColor(bodyCreator.bodySettings.headSettings.lipColor);
                break;

            case "Cover":
                SetColor(bodyCreator.bodySettings.coverSettings.color);
                break;

            default:
                break;
        }
        Color displayColor;
        if (targetDropdown.options[targetDropdown.value].text != "Skin")
        {
            displayColor = Color.Lerp(bodyCreator.bodySettings.skinColor, new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value), colorSliders[3].value);

        }
        else displayColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);
        colorRepresentation.color = displayColor;
    }

    public void MixColor(float value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        Color displayColor;
        if (targetDropdown.options[targetDropdown.value].text != "Skin")
        {
            displayColor = Color.Lerp(bodyCreator.bodySettings.skinColor, new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value), colorSliders[3].value);

        }
        else displayColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);

        Color mixedColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value, colorSliders[3].value);

        switch (targetDropdown.options[targetDropdown.value].text)
        {
            case "Skin":
                bodyCreator.bodySettings.skinColor = displayColor;
                break;
            case "Hair":
                bodyCreator.bodySettings.hairColor = mixedColor;
                break;
            case "Eye Lids":
                Debug.Log("setting eye lid color");
                bodyCreator.bodySettings.headSettings.eyeLidColor = mixedColor;
                break;
            case "Sclera":
                bodyCreator.bodySettings.headSettings.scleraColor = mixedColor;
                break;
            case "Iris":
                bodyCreator.bodySettings.headSettings.irisColor = mixedColor;
                break;
            case "Pupil":
                bodyCreator.bodySettings.headSettings.pupilColor = mixedColor;
                break;
            case "Makeup":
                bodyCreator.bodySettings.headSettings.makeupColor = mixedColor;
                break;
            case "Lips":
                bodyCreator.bodySettings.headSettings.lipColor = mixedColor;
                break;
            case "Cover":
                bodyCreator.bodySettings.coverSettings.color = displayColor;
                break;
            default:
                break;
        }
        colorRepresentation.color = displayColor;
        bodyCreator.RecalculateBody();
    }
}
