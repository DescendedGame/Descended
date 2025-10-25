using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodySliderManager : MonoBehaviour
{
    HumanoidBodyCreator bodyCreator;
    [SerializeField] GameObject bodySliderPrefab;
    Slider[] bodySliders = new Slider[26];
    void Awake()
    {
        for(int i = 0; i < bodySliders.Length; i++)
        {
            GameObject bodySlider = Instantiate(bodySliderPrefab, transform.GetChild(0));
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
    }

    float FifthSpan(float midValue, float slideValue)
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
        bodyCreator.upperNeckLength = FifthSpan(0.1f, bodySliders[0].value);
        bodyCreator.upperNeckWidth = FifthSpan(0.05f, bodySliders[1].value);
        bodyCreator.lowerNeckWidth = FifthSpan(0.08f, bodySliders[2].value);

        bodyCreator.atlasLength = FifthSpan(0.1f, bodySliders[3].value);
        bodyCreator.torsoDepth = FifthSpan(0.12f, bodySliders[4].value);

        bodyCreator.torsoWidth = FifthSpan(0.05f, bodySliders[5].value);
        bodyCreator.ribLength = FifthSpan(0.2f, bodySliders[6].value);
        bodyCreator.waist = FifthSpan(0.08f, bodySliders[7].value);
        bodyCreator.bellyLength = FifthSpan(0.15f, bodySliders[8].value);
        bodyCreator.shoulderWidth = FifthSpan(0.1f, bodySliders[9].value);
        bodyCreator.shoulderSize = FifthSpan(0.08f, bodySliders[10].value);
        bodyCreator.armLength = FifthSpan(0.3f, bodySliders[11].value);
        bodyCreator.elbowSize = FifthSpan(0.04f, bodySliders[12].value);
        bodyCreator.forearmLength = FifthSpan(0.25f, bodySliders[13].value);
        bodyCreator.wristSize = FifthSpan(0.025f, bodySliders[14].value);

        bodyCreator.upperHipWidth = FifthSpan(0.05f, bodySliders[15].value);
        bodyCreator.upperHipRadius = FifthSpan(0.14f, bodySliders[16].value);
        bodyCreator.lowerHipRadius = FifthSpan(0.15f, bodySliders[17].value);
        bodyCreator.hipLength = FifthSpan(0.09f, bodySliders[18].value);
        bodyCreator.hipOutRotation = FifthSpan(20, bodySliders[19].value);

        bodyCreator.thighLength = FifthSpan(0.4f, bodySliders[20].value);
        bodyCreator.kneeRadius = FifthSpan(0.05f, bodySliders[21].value);
        bodyCreator.upperCalfLength = FifthSpan(0.1f, bodySliders[22].value);
        bodyCreator.calfRadius = FifthSpan(0.06f, bodySliders[23].value);
        bodyCreator.lowerCalfLength = FifthSpan(0.25f, bodySliders[24].value);
        bodyCreator.ankleRadius = FifthSpan(0.03f, bodySliders[25].value);

        bodyCreator.RecalculateBody();
    }
}
