using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodySliderManager : MonoBehaviour
{
    HumanoidBodyCreator bodyCreator;
    [SerializeField] GameObject customizationSliderPrefab;
    [SerializeField] GameObject customizationTogglePrefab;
    Slider[] bodySliders = new Slider[26];
    Toggle[] bodyToggles = new Toggle[12];
    void Awake()
    {
        for(int i = 0; i < bodySliders.Length; i++)
        {
            GameObject bodySlider = Instantiate(customizationSliderPrefab, transform.GetChild(0));
            bodySliders[i] = bodySlider.GetComponent<Slider>();
            bodySliders[i].onValueChanged.AddListener(OnSliderChanged);
        }

        bodySliders[0].GetComponentInChildren<TMP_Text>().text = "upper neck length";
        bodySliders[1].GetComponentInChildren<TMP_Text>().text = "upper neck width";
        bodySliders[2].GetComponentInChildren<TMP_Text>().text = "lower neck width";
        bodySliders[3].GetComponentInChildren<TMP_Text>().text = "atlas length";
        bodySliders[4].GetComponentInChildren<TMP_Text>().text = "torso depth";
        bodySliders[5].GetComponentInChildren<TMP_Text>().text = "torso width";
        bodySliders[6].GetComponentInChildren<TMP_Text>().text = "rib length";
        bodySliders[7].GetComponentInChildren<TMP_Text>().text = "waist";
        bodySliders[8].GetComponentInChildren<TMP_Text>().text = "belly length";
        bodySliders[9].GetComponentInChildren<TMP_Text>().text = "shoulder width";
        bodySliders[10].GetComponentInChildren<TMP_Text>().text = "shoulder size";
        bodySliders[11].GetComponentInChildren<TMP_Text>().text = "arm length";
        bodySliders[12].GetComponentInChildren<TMP_Text>().text = "elbow size";
        bodySliders[13].GetComponentInChildren<TMP_Text>().text = "forearm length";
        bodySliders[14].GetComponentInChildren<TMP_Text>().text = "wrist size";
        bodySliders[15].GetComponentInChildren<TMP_Text>().text = "upper hip width";
        bodySliders[16].GetComponentInChildren<TMP_Text>().text = "upper hip radius";
        bodySliders[17].GetComponentInChildren<TMP_Text>().text = "lower hip radius";
        bodySliders[18].GetComponentInChildren<TMP_Text>().text = "hip length";
        bodySliders[19].GetComponentInChildren<TMP_Text>().text = "hip out rotation";
        bodySliders[20].GetComponentInChildren<TMP_Text>().text = "thigh length";
        bodySliders[21].GetComponentInChildren<TMP_Text>().text = "knee radius";
        bodySliders[22].GetComponentInChildren<TMP_Text>().text = "upper calf length";
        bodySliders[23].GetComponentInChildren<TMP_Text>().text = "calf radius";
        bodySliders[24].GetComponentInChildren<TMP_Text>().text = "lower calf length";
        bodySliders[25].GetComponentInChildren<TMP_Text>().text = "ankle radius";

        for (int i = 0; i < bodyToggles.Length; i++)
        {
            GameObject bodyToggle = Instantiate(customizationTogglePrefab, transform.GetChild(1));
            bodyToggles[i] = bodyToggle.GetComponent<Toggle>();
            bodyToggles[i].onValueChanged.AddListener(OnToggleChanged);
        }
        bodyToggles[0].GetComponentInChildren<Text>().text = "upper neck";
        bodyToggles[1].GetComponentInChildren<Text>().text = "lower neck";
        bodyToggles[2].GetComponentInChildren<Text>().text = "shoulders";
        bodyToggles[3].GetComponentInChildren<Text>().text = "elbows";
        bodyToggles[4].GetComponentInChildren<Text>().text = "wrists";
        bodyToggles[5].GetComponentInChildren<Text>().text = "chest";
        bodyToggles[6].GetComponentInChildren<Text>().text = "waist";
        bodyToggles[7].GetComponentInChildren<Text>().text = "hips";
        bodyToggles[8].GetComponentInChildren<Text>().text = "butt";
        bodyToggles[9].GetComponentInChildren<Text>().text = "knees";
        bodyToggles[10].GetComponentInChildren<Text>().text = "calves";
        bodyToggles[11].GetComponentInChildren<Text>().text = "ankles";
    }

    float HalfSpan(float midValue, float slideValue)
    {
        return Mathf.Lerp(midValue * 0.5f, midValue * 1.5f, slideValue);
    }


    void OnSliderChanged(float pValue)
    {
        if(bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.upperNeckLength = HalfSpan(0.1f, bodySliders[0].value);
        bodyCreator.bodySettings.upperNeckWidth = HalfSpan(0.05f, bodySliders[1].value);
        bodyCreator.bodySettings.lowerNeckWidth = HalfSpan(0.08f, bodySliders[2].value);

        bodyCreator.bodySettings.atlasLength = HalfSpan(0.1f, bodySliders[3].value);
        bodyCreator.bodySettings.torsoDepth = HalfSpan(0.12f, bodySliders[4].value);

        bodyCreator.bodySettings.torsoWidth = HalfSpan(0.05f, bodySliders[5].value);
        bodyCreator.bodySettings.ribLength = HalfSpan(0.2f, bodySliders[6].value);
        bodyCreator.bodySettings.waist = HalfSpan(0.08f, bodySliders[7].value);
        bodyCreator.bodySettings.bellyLength = HalfSpan(0.15f, bodySliders[8].value);
        bodyCreator.bodySettings.shoulderWidth = HalfSpan(0.1f, bodySliders[9].value);
        bodyCreator.bodySettings.shoulderSize = HalfSpan(0.08f, bodySliders[10].value);
        bodyCreator.bodySettings.armLength = HalfSpan(0.3f, bodySliders[11].value);
        bodyCreator.bodySettings.elbowSize = HalfSpan(0.04f, bodySliders[12].value);
        bodyCreator.bodySettings.forearmLength = HalfSpan(0.25f, bodySliders[13].value);
        bodyCreator.bodySettings.wristSize = HalfSpan(0.025f, bodySliders[14].value);

        bodyCreator.bodySettings.upperHipWidth = HalfSpan(0.05f, bodySliders[15].value);
        bodyCreator.bodySettings.upperHipRadius = HalfSpan(0.14f, bodySliders[16].value);
        bodyCreator.bodySettings.lowerHipRadius = HalfSpan(0.15f, bodySliders[17].value);
        bodyCreator.bodySettings.hipLength = HalfSpan(0.09f, bodySliders[18].value);
        bodyCreator.bodySettings.hipOutRotation = HalfSpan(20, bodySliders[19].value);

        bodyCreator.bodySettings.thighLength = HalfSpan(0.4f, bodySliders[20].value);
        bodyCreator.bodySettings.kneeRadius = HalfSpan(0.05f, bodySliders[21].value);
        bodyCreator.bodySettings.upperCalfLength = HalfSpan(0.1f, bodySliders[22].value);
        bodyCreator.bodySettings.calfRadius = HalfSpan(0.06f, bodySliders[23].value);
        bodyCreator.bodySettings.lowerCalfLength = HalfSpan(0.25f, bodySliders[24].value);
        bodyCreator.bodySettings.ankleRadius = HalfSpan(0.03f, bodySliders[25].value);

        bodyCreator.RecalculateBody();
    }
    void OnToggleChanged(bool value)
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.coverSettings.upperNeck = bodyToggles[0].isOn;
        bodyCreator.bodySettings.coverSettings.lowerNeck = bodyToggles[1].isOn;
        bodyCreator.bodySettings.coverSettings.shoulders = bodyToggles[2].isOn;
        bodyCreator.bodySettings.coverSettings.elbows = bodyToggles[3].isOn;
        bodyCreator.bodySettings.coverSettings.wrists = bodyToggles[4].isOn;
        bodyCreator.bodySettings.coverSettings.chest = bodyToggles[5].isOn;
        bodyCreator.bodySettings.coverSettings.waist = bodyToggles[6].isOn;
        bodyCreator.bodySettings.coverSettings.hips = bodyToggles[7].isOn;
        bodyCreator.bodySettings.coverSettings.butt = bodyToggles[8].isOn;
        bodyCreator.bodySettings.coverSettings.knees = bodyToggles[9].isOn;
        bodyCreator.bodySettings.coverSettings.calves = bodyToggles[10].isOn;
        bodyCreator.bodySettings.coverSettings.ankles = bodyToggles[11].isOn;

        bodyCreator.RecalculateBody();
    }
}
