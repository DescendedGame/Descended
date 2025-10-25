using UnityEngine;

[ExecuteInEditMode]
public class LimbInitializer : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public bool SetColor = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Initialize();   
    }

    public void Initialize()
    {
        InitializeRecursively(transform);
        StitchRecursively(transform);
    }

    public void InitializeRecursively(Transform current)
    {
        GeneratedLimb generator = current.GetComponent<GeneratedLimb>();
        if (generator!= null)
        {
            generator.Initialize();
        }
        for(int i = 0; i < current.childCount; i++)
        {
            InitializeRecursively(current.GetChild(i));
        }
    }
    public void ColorRecursively(Transform current)
    {
        GeneratedLimb generator = current.GetComponent<GeneratedLimb>();
        if (generator != null)
        {
            generator.startColor = startColor;
            generator.endColor = endColor;
        }
        BodyStitcher stitcher = current.GetComponent<BodyStitcher>();
        if (stitcher != null)
        {
            stitcher.startColor = startColor;
            stitcher.endColor = endColor;
        }
        for (int i = 0; i < current.childCount; i++)
        {
            ColorRecursively(current.GetChild(i));
        }
    }

    public void StitchRecursively(Transform current)
    {
        BodyStitcher generator = current.GetComponent<BodyStitcher>();
        if (generator != null)
        {
            generator.Initialize();
        }
        for (int i = 0; i < current.childCount; i++)
        {
            StitchRecursively(current.GetChild(i));
     
        }
    }

    private void Update()
    {
        if(SetColor)
        {
            SetColor = false;
            ColorRecursively(transform);
            InitializeRecursively(transform);
            StitchRecursively(transform);
        }
    }
}
