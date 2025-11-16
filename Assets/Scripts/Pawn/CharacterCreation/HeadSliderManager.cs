using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadSliderManager : MonoBehaviour
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

        bodySliders[0].GetComponentInChildren<TMP_Text>().text = "skull size";
        bodySliders[1].GetComponentInChildren<TMP_Text>().text = "jaw width";
        bodySliders[2].GetComponentInChildren<TMP_Text>().text = "jaw height";
        bodySliders[3].GetComponentInChildren<TMP_Text>().text = "jaw depth";
        bodySliders[4].GetComponentInChildren<TMP_Text>().text = "cheekbone width";
        bodySliders[5].GetComponentInChildren<TMP_Text>().text = "cheekbone height";
        bodySliders[6].GetComponentInChildren<TMP_Text>().text = "cheeck size";
        bodySliders[7].GetComponentInChildren<TMP_Text>().text = "chin length";
        bodySliders[8].GetComponentInChildren<TMP_Text>().text = "chin width";
        bodySliders[9].GetComponentInChildren<TMP_Text>().text = "eye height";
        bodySliders[10].GetComponentInChildren<TMP_Text>().text = "eye distance";
        bodySliders[11].GetComponentInChildren<TMP_Text>().text = "eye depth";
        bodySliders[12].GetComponentInChildren<TMP_Text>().text = "eye size";
        bodySliders[13].GetComponentInChildren<TMP_Text>().text = "outer brow";
        bodySliders[14].GetComponentInChildren<TMP_Text>().text = "inner brow";
        bodySliders[15].GetComponentInChildren<TMP_Text>().text = "brow distance";
        bodySliders[16].GetComponentInChildren<TMP_Text>().text = "brow depth";
        bodySliders[17].GetComponentInChildren<TMP_Text>().text = "mouth width";
        bodySliders[18].GetComponentInChildren<TMP_Text>().text = "mouth height";
        bodySliders[19].GetComponentInChildren<TMP_Text>().text = "lip size";
        bodySliders[20].GetComponentInChildren<TMP_Text>().text = "nose width";
        bodySliders[21].GetComponentInChildren<TMP_Text>().text = "nose height";
        bodySliders[22].GetComponentInChildren<TMP_Text>().text = "nose depth";
        bodySliders[23].GetComponentInChildren<TMP_Text>().text = "ear rotation";
        bodySliders[24].GetComponentInChildren<TMP_Text>().text = "ear height";
        bodySliders[25].GetComponentInChildren<TMP_Text>().text = "ear size";

        /*for (int i = 0; i < bodyToggles.Length; i++)
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
        bodyToggles[11].GetComponentInChildren<Text>().text = "ankles";*/
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
        bodyCreator.bodySettings.headSettings.skullSize = HalfSpan(0.1f, bodySliders[0].value);
        bodyCreator.bodySettings.headSettings.jawWidth = HalfSpan(0.05f, bodySliders[1].value);
        bodyCreator.bodySettings.headSettings.jawHeight = HalfSpan(0.08f, bodySliders[2].value);
        bodyCreator.bodySettings.headSettings.jawDepth = HalfSpan(0.1f, bodySliders[3].value);

        bodyCreator.bodySettings.headSettings.cheekboneWidth = HalfSpan(0.12f, bodySliders[4].value);
        bodyCreator.bodySettings.headSettings.cheekboneHeight = HalfSpan(0.05f, bodySliders[5].value);
        bodyCreator.bodySettings.headSettings.cheekSize = HalfSpan(0.2f, bodySliders[6].value);
        bodyCreator.bodySettings.headSettings.chinLength = HalfSpan(0.08f, bodySliders[7].value);
        bodyCreator.bodySettings.headSettings.chinWidth = HalfSpan(0.15f, bodySliders[8].value);
        bodyCreator.bodySettings.headSettings.eyeHeight = HalfSpan(0.1f, bodySliders[9].value);
        bodyCreator.bodySettings.headSettings.eyeDistance = HalfSpan(0.08f, bodySliders[10].value);
        bodyCreator.bodySettings.headSettings.eyeDepth = HalfSpan(0.3f, bodySliders[11].value);
        bodyCreator.bodySettings.headSettings.eyeSize = HalfSpan(0.04f, bodySliders[12].value);
        bodyCreator.bodySettings.headSettings.outerBrow = HalfSpan(0.25f, bodySliders[13].value);
        bodyCreator.bodySettings.headSettings.innerBrow = HalfSpan(0.025f, bodySliders[14].value);

        bodyCreator.bodySettings.headSettings.browDistance = HalfSpan(0.05f, bodySliders[15].value);
        bodyCreator.bodySettings.headSettings.browDepth = HalfSpan(0.14f, bodySliders[16].value);
        bodyCreator.bodySettings.headSettings.mouthWidth = HalfSpan(0.15f, bodySliders[17].value);
        bodyCreator.bodySettings.headSettings.mouthHeight = HalfSpan(0.09f, bodySliders[18].value);
        bodyCreator.bodySettings.headSettings.lipSize = HalfSpan(20, bodySliders[19].value);

        bodyCreator.bodySettings.headSettings.noseWidth = HalfSpan(0.4f, bodySliders[20].value);
        bodyCreator.bodySettings.headSettings.noseHeight = HalfSpan(0.05f, bodySliders[21].value);
        bodyCreator.bodySettings.headSettings.noseDepth = HalfSpan(0.1f, bodySliders[22].value);
        bodyCreator.bodySettings.headSettings.earRotation = HalfSpan(0.06f, bodySliders[23].value);
        bodyCreator.bodySettings.headSettings.earHeight = HalfSpan(0.25f, bodySliders[24].value);
        bodyCreator.bodySettings.headSettings.earSize = HalfSpan(0.03f, bodySliders[25].value);

        bodyCreator.CreateHead();
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
