using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{

    [SerializeField] GameObject characterCreationButtonPrefab;

    Button randomizeAll;
    Button randomizeHead;
    Button randomizeBody;
    Button randomizeColors;
    Button done;

    [SerializeField] HairSelector hairSelector;
    [SerializeField] HeadSliderManager headSliderManager;
    [SerializeField] BodySliderManager bodySliderManager;
    [SerializeField] ColorMixer colorMixer;
    [SerializeField] InputField nameEntry;

    HumanoidBodyCreator bodyCreator;


    private void Awake()
    {
        randomizeAll = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
        randomizeAll.GetComponentInChildren<TMP_Text>().text = "Randomize All";
        randomizeAll.onClick.AddListener(RandomizeAll);
        randomizeAll.onClick.AddListener(RecalculateBody);

        randomizeHead = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
        randomizeHead.GetComponentInChildren<TMP_Text>().text = "Randomize Head";
        randomizeHead.onClick.AddListener(RandomizeHead);
        randomizeHead.onClick.AddListener(RecalculateBody);

        randomizeBody = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
        randomizeBody.GetComponentInChildren<TMP_Text>().text = "Randomize Body";
        randomizeBody.onClick.AddListener(RandomizeBody);
        randomizeBody.onClick.AddListener(RecalculateBody);

        randomizeColors = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
        randomizeColors.GetComponentInChildren<TMP_Text>().text = "Randomize Colors";
        randomizeColors.onClick.AddListener(RandomizeColors);
        randomizeColors.onClick.AddListener(RecalculateBody);

        done = Instantiate(characterCreationButtonPrefab, transform.GetChild(0)).GetComponent<Button>();
        done.GetComponentInChildren<TMP_Text>().text = "Finish";
        done.onClick.AddListener(Done);
    }

    void RandomizeAll()
    {
        RandomizeHead();
        RandomizeColors();
        RandomizeBody();
    }

    void RandomizeHead()
    {
        headSliderManager.Randomize();
        hairSelector.Randomize();
    }

    void RandomizeBody()
    {
        bodySliderManager.Randomize();
    }

    void RandomizeColors()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.bodySettings.skinColor = Random.ColorHSV();
        bodyCreator.bodySettings.hairColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.eyeLidColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.scleraColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.irisColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.pupilColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.makeupColor = RandomTransparentColor();
        bodyCreator.bodySettings.headSettings.lipColor = RandomTransparentColor();
        bodyCreator.bodySettings.coverSettings.color = RandomTransparentColor();

        colorMixer.ReadValues();
    }

    Color RandomTransparentColor()
    {
        Color targetColor = new Color(Random.value, Random.value, Random.value, Random.value);
        
        Color newColor = Color.Lerp(bodyCreator.bodySettings.skinColor, targetColor, targetColor.a);
        newColor.a = targetColor.a;
        return newColor;
    }

    void RecalculateBody()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.RecalculateBody();
    }

    void Done()
    {
        if (bodyCreator == null)
        {
            bodyCreator = FindFirstObjectByType<HumanoidBodyCreator>();
            if (bodyCreator == null) return;
        }
        bodyCreator.SaveBody(nameEntry.text);
        SceneManager.LoadScene("Lobby");
    }
}
