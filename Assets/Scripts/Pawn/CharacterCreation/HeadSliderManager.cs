using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadSliderManager : MonoBehaviour
{
    HumanoidBodyCreator bodyCreator;
    [SerializeField] GameObject customizationSliderPrefab;
    [SerializeField] GameObject customizationTogglePrefab;
    Slider[] headSliders = new Slider[26];
    Toggle[] bodyToggles = new Toggle[12];

    public Vector2 skullSizeSpan = new Vector2(0.9f, 1.1f);
    public Vector2 jawWidthSpan = new Vector2(0.05f, 0);
    public Vector2 jawHeightSpan = new Vector2(0.08f, 0);
    public Vector2 jawDepthSpan = new Vector2(0.1f, 0);

    public Vector2 cheekboneWidthSpan = new Vector2(0, 0);
    public Vector2 cheekboneHeightSpan = new Vector2(0, 0);
    public Vector2 cheekSizeSpan = new Vector2(0, 0);
    public Vector2 chinLengthSpan = new Vector2(0, 0);
    public Vector2 chinWidthSpan = new Vector2(0.15f, 0);
    public Vector2 eyeHeightSpan = new Vector2(0.1f, 0);
    public Vector2 eyeDistanceSpan = new Vector2(0.08f, 10);
    public Vector2 eyeDepthSpan = new Vector2(0.3f, 11);
    public Vector2 eyeSizeSpan = new Vector2(0.04f, 12);
    public Vector2 outerBrowSpan = new Vector2(0.25f, 13);
    public Vector2 innerBrowSpan = new Vector2(0.025f, 14);

    public Vector2 browDistanceSpan = new Vector2(-0.02f, 0.02f);
    public Vector2 browDepthSpan = new Vector2(-0.02f, 0.02f);
    public Vector2 mouthWidthSpan = new Vector2(0.15f, 17);
    public Vector2 mouthHeightSpan = new Vector2(0.09f, 18);
    public Vector2 lipSizeSpan = new Vector2(20, 19);

    public Vector2 noseWidthSpan = new Vector2(0.4f, 20);
    public Vector2 noseHeightSpan = new Vector2(0.05f, 21);
    public Vector2 noseDepthSpan = new Vector2(0.1f, 22);
    public Vector2 earRotationSpan = new Vector2(0.06f, 23);
    public Vector2 earHeightSpan = new Vector2(0.25f, 24);
    public Vector2 earSizeSpan = new Vector2(0.03f, 25);

    void Awake()
    {
        for(int i = 0; i < headSliders.Length; i++)
        {
            GameObject bodySlider = Instantiate(customizationSliderPrefab, transform.GetChild(0));
            headSliders[i] = bodySlider.GetComponent<Slider>();
            headSliders[i].onValueChanged.AddListener(OnSliderChanged);
        }

        headSliders[0].GetComponentInChildren<TMP_Text>().text = "skull size";
        headSliders[1].GetComponentInChildren<TMP_Text>().text = "jaw width";
        headSliders[2].GetComponentInChildren<TMP_Text>().text = "jaw height";
        headSliders[3].GetComponentInChildren<TMP_Text>().text = "jaw depth";
        headSliders[4].GetComponentInChildren<TMP_Text>().text = "cheekbone width";
        headSliders[5].GetComponentInChildren<TMP_Text>().text = "cheekbone height";
        headSliders[6].GetComponentInChildren<TMP_Text>().text = "cheeck size";
        headSliders[7].GetComponentInChildren<TMP_Text>().text = "chin length";
        headSliders[8].GetComponentInChildren<TMP_Text>().text = "chin width";
        headSliders[9].GetComponentInChildren<TMP_Text>().text = "eye height";
        headSliders[10].GetComponentInChildren<TMP_Text>().text = "eye distance";
        headSliders[11].GetComponentInChildren<TMP_Text>().text = "eye depth";
        headSliders[12].GetComponentInChildren<TMP_Text>().text = "eye size";
        headSliders[13].GetComponentInChildren<TMP_Text>().text = "outer brow";
        headSliders[14].GetComponentInChildren<TMP_Text>().text = "inner brow";
        headSliders[15].GetComponentInChildren<TMP_Text>().text = "brow distance";
        headSliders[16].GetComponentInChildren<TMP_Text>().text = "brow depth";
        headSliders[17].GetComponentInChildren<TMP_Text>().text = "mouth width";
        headSliders[18].GetComponentInChildren<TMP_Text>().text = "mouth height";
        headSliders[19].GetComponentInChildren<TMP_Text>().text = "lip size";
        headSliders[20].GetComponentInChildren<TMP_Text>().text = "nose width";
        headSliders[21].GetComponentInChildren<TMP_Text>().text = "nose height";
        headSliders[22].GetComponentInChildren<TMP_Text>().text = "nose depth";
        headSliders[23].GetComponentInChildren<TMP_Text>().text = "ear rotation";
        headSliders[24].GetComponentInChildren<TMP_Text>().text = "ear height";
        headSliders[25].GetComponentInChildren<TMP_Text>().text = "ear size";

    }

    private void Start()
    {
        ReadValues();
    }

    void ReadValues()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }

        headSliders[0].SetValueWithoutNotify(
            InverseHalfSpan(1f, bodyCreator.bodySettings.headSettings.skullSize));

        headSliders[1].SetValueWithoutNotify(
            Mathf.InverseLerp(jawWidthSpan.x, jawWidthSpan.y,
                bodyCreator.bodySettings.headSettings.jawWidth));

        headSliders[2].SetValueWithoutNotify(
            Mathf.InverseLerp(jawHeightSpan.x, jawHeightSpan.y,
                bodyCreator.bodySettings.headSettings.jawHeight));

        headSliders[3].SetValueWithoutNotify(
            Mathf.InverseLerp(jawDepthSpan.x, jawDepthSpan.y,
                bodyCreator.bodySettings.headSettings.jawDepth));

        headSliders[4].SetValueWithoutNotify(
            Mathf.InverseLerp(cheekboneWidthSpan.x, cheekboneWidthSpan.y,
                bodyCreator.bodySettings.headSettings.cheekboneWidth));

        headSliders[5].SetValueWithoutNotify(
            Mathf.InverseLerp(cheekboneHeightSpan.x, cheekboneHeightSpan.y,
                bodyCreator.bodySettings.headSettings.cheekboneHeight));

        headSliders[6].SetValueWithoutNotify(
            Mathf.InverseLerp(cheekSizeSpan.x, cheekSizeSpan.y,
                bodyCreator.bodySettings.headSettings.cheekSize));

        headSliders[7].SetValueWithoutNotify(
            Mathf.InverseLerp(chinLengthSpan.x, chinLengthSpan.y,
                bodyCreator.bodySettings.headSettings.chinLength));

        headSliders[8].SetValueWithoutNotify(
            Mathf.InverseLerp(chinWidthSpan.x, chinWidthSpan.y,
                bodyCreator.bodySettings.headSettings.chinWidth));

        headSliders[9].SetValueWithoutNotify(
            Mathf.InverseLerp(eyeHeightSpan.x, eyeHeightSpan.y,
                bodyCreator.bodySettings.headSettings.eyeHeight));

        headSliders[10].SetValueWithoutNotify(
            Mathf.InverseLerp(eyeDistanceSpan.x, eyeDistanceSpan.y,
                bodyCreator.bodySettings.headSettings.eyeDistance));

        headSliders[11].SetValueWithoutNotify(
            Mathf.InverseLerp(eyeDepthSpan.x, eyeDepthSpan.y,
                bodyCreator.bodySettings.headSettings.eyeDepth));

        headSliders[12].SetValueWithoutNotify(
            Mathf.InverseLerp(eyeSizeSpan.x, eyeSizeSpan.y,
                bodyCreator.bodySettings.headSettings.eyeSize));

        headSliders[13].SetValueWithoutNotify(
            Mathf.InverseLerp(outerBrowSpan.x, outerBrowSpan.y,
                bodyCreator.bodySettings.headSettings.outerBrow));

        headSliders[14].SetValueWithoutNotify(
            Mathf.InverseLerp(innerBrowSpan.x, innerBrowSpan.y,
                bodyCreator.bodySettings.headSettings.innerBrow));

        headSliders[15].SetValueWithoutNotify(
            Mathf.InverseLerp(browDistanceSpan.x, browDistanceSpan.y,
                bodyCreator.bodySettings.headSettings.browDistance));

        headSliders[16].SetValueWithoutNotify(
            Mathf.InverseLerp(browDepthSpan.x, browDepthSpan.y,
                bodyCreator.bodySettings.headSettings.browDepth));

        headSliders[17].SetValueWithoutNotify(
            Mathf.InverseLerp(mouthWidthSpan.x, mouthWidthSpan.y,
                bodyCreator.bodySettings.headSettings.mouthWidth));

        headSliders[18].SetValueWithoutNotify(
            Mathf.InverseLerp(mouthHeightSpan.x, mouthHeightSpan.y,
                bodyCreator.bodySettings.headSettings.mouthHeight));

        headSliders[19].SetValueWithoutNotify(
            Mathf.InverseLerp(lipSizeSpan.x, lipSizeSpan.y,
                bodyCreator.bodySettings.headSettings.lipSize));

        headSliders[20].SetValueWithoutNotify(
            Mathf.InverseLerp(noseWidthSpan.x, noseWidthSpan.y,
                bodyCreator.bodySettings.headSettings.noseWidth));

        headSliders[21].SetValueWithoutNotify(
            Mathf.InverseLerp(noseHeightSpan.x, noseHeightSpan.y,
                bodyCreator.bodySettings.headSettings.noseHeight));

        headSliders[22].SetValueWithoutNotify(
            Mathf.InverseLerp(noseDepthSpan.x, noseDepthSpan.y,
                bodyCreator.bodySettings.headSettings.noseDepth));

        headSliders[23].SetValueWithoutNotify(
            Mathf.InverseLerp(earRotationSpan.x, earRotationSpan.y,
                bodyCreator.bodySettings.headSettings.earRotation));

        headSliders[24].SetValueWithoutNotify(
            Mathf.InverseLerp(earHeightSpan.x, earHeightSpan.y,
                bodyCreator.bodySettings.headSettings.earHeight));

        headSliders[25].SetValueWithoutNotify(
            Mathf.InverseLerp(earSizeSpan.x, earSizeSpan.y,
                bodyCreator.bodySettings.headSettings.earSize));
    }

    float HalfSpan(float midValue, float slideValue)
    {
        return Mathf.Lerp(midValue * 0.5f, midValue * 1.5f, slideValue);
    }

    float InverseHalfSpan(float midValue, float value)
    {
        return value / midValue - 0.5f;
    }


    void OnSliderChanged(float pValue)
    {
        AssignValues();

        bodyCreator?.CreateHead();
    }

    void AssignValues()
    {
        bodyCreator.bodySettings.headSettings.skullSize = HalfSpan(1f, headSliders[0].value);
        bodyCreator.bodySettings.headSettings.jawWidth = Mathf.Lerp(jawWidthSpan.x, jawWidthSpan.y, headSliders[1].value);
        bodyCreator.bodySettings.headSettings.jawHeight = Mathf.Lerp(jawHeightSpan.x, jawHeightSpan.y, headSliders[2].value);
        bodyCreator.bodySettings.headSettings.jawDepth = Mathf.Lerp(jawDepthSpan.x, jawDepthSpan.y, headSliders[3].value);

        bodyCreator.bodySettings.headSettings.cheekboneWidth = Mathf.Lerp(cheekboneWidthSpan.x, cheekboneWidthSpan.y, headSliders[4].value);
        bodyCreator.bodySettings.headSettings.cheekboneHeight = Mathf.Lerp(cheekboneHeightSpan.x, cheekboneHeightSpan.y, headSliders[5].value);
        bodyCreator.bodySettings.headSettings.cheekSize = Mathf.Lerp(cheekSizeSpan.x, cheekSizeSpan.y, headSliders[6].value);
        bodyCreator.bodySettings.headSettings.chinLength = Mathf.Lerp(chinLengthSpan.x, chinLengthSpan.y, headSliders[7].value);
        bodyCreator.bodySettings.headSettings.chinWidth = Mathf.Lerp(chinWidthSpan.x, chinWidthSpan.y, headSliders[8].value);
        bodyCreator.bodySettings.headSettings.eyeHeight = Mathf.Lerp(eyeHeightSpan.x, eyeHeightSpan.y, headSliders[9].value);
        bodyCreator.bodySettings.headSettings.eyeDistance = Mathf.Lerp(eyeDepthSpan.x, eyeDistanceSpan.y, headSliders[10].value);
        bodyCreator.bodySettings.headSettings.eyeDepth = Mathf.Lerp(eyeDepthSpan.x, eyeDepthSpan.y, headSliders[11].value);
        bodyCreator.bodySettings.headSettings.eyeSize = Mathf.Lerp(eyeSizeSpan.x, eyeSizeSpan.y, headSliders[12].value);
        bodyCreator.bodySettings.headSettings.outerBrow = Mathf.Lerp(outerBrowSpan.x, outerBrowSpan.y, headSliders[13].value);
        bodyCreator.bodySettings.headSettings.innerBrow = Mathf.Lerp(innerBrowSpan.x, innerBrowSpan.y, headSliders[14].value);

        bodyCreator.bodySettings.headSettings.browDistance = Mathf.Lerp(browDistanceSpan.x, browDistanceSpan.y, headSliders[15].value);
        bodyCreator.bodySettings.headSettings.browDepth = Mathf.Lerp(browDepthSpan.x, browDepthSpan.y, headSliders[16].value);
        bodyCreator.bodySettings.headSettings.mouthWidth = Mathf.Lerp(mouthWidthSpan.x, mouthWidthSpan.y, headSliders[17].value);
        bodyCreator.bodySettings.headSettings.mouthHeight = Mathf.Lerp(mouthHeightSpan.x, mouthHeightSpan.y, headSliders[18].value);
        bodyCreator.bodySettings.headSettings.lipSize = Mathf.Lerp(lipSizeSpan.x, lipSizeSpan.y, headSliders[19].value);
        bodyCreator.bodySettings.headSettings.noseWidth = Mathf.Lerp(noseWidthSpan.x, noseWidthSpan.y, headSliders[20].value);
        bodyCreator.bodySettings.headSettings.noseHeight = Mathf.Lerp(noseHeightSpan.x, noseHeightSpan.y, headSliders[21].value);
        bodyCreator.bodySettings.headSettings.noseDepth = Mathf.Lerp(noseDepthSpan.x, noseDepthSpan.y, headSliders[22].value);
        bodyCreator.bodySettings.headSettings.earRotation = Mathf.Lerp(earRotationSpan.x, earRotationSpan.y, headSliders[23].value);
        bodyCreator.bodySettings.headSettings.earHeight = Mathf.Lerp(earHeightSpan.x, earHeightSpan.y, headSliders[24].value);
        bodyCreator.bodySettings.headSettings.earSize = Mathf.Lerp(earSizeSpan.x, earSizeSpan.y, headSliders[25].value);
    }

    public void Randomize()
    {
        foreach(Slider slider in headSliders)
        {
            slider.SetValueWithoutNotify(Random.value);
        }
        AssignValues();
    }
}
