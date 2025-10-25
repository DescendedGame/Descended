using UnityEngine;

enum LegState
{
    Idle,
}

public class HumanLeg : BodyLinkage
{
    public bool isRight = true;

    [SerializeField] Transform leftHip;
    [SerializeField] Transform rightHip;

    [SerializeField] Transform thigh;
    [SerializeField] Transform calf;
    [SerializeField] Transform foot;

    Quaternion initialThighRotation;
    Quaternion initialCalfRotation;
    Quaternion initialFootRotation;

    GeneratedLimb thighLimb;
    GeneratedLimb upperCalfLimb;
    GeneratedLimb lowerCalfLimb;

    public void Initialize(HumanBodySettings bodySettings, bool isRight)
    {

        if(thighLimb == null)
        {
            initialThighRotation = transform.localRotation;
            thighLimb = gameObject.AddComponent<GeneratedLimb>();
            thighLimb.snapToParent = true;
            thigh = transform;
        }
        thighLimb.length = bodySettings.thighLength;
        thighLimb.startRadius = bodySettings.lowerHipRadius;
        thighLimb.endRadius = bodySettings.kneeRadius;
        thighLimb.mat = new Material(bodySettings.basicInGameObject);
        thighLimb.startColor = bodySettings.coverSettings.butt ? bodySettings.coverSettings.color : bodySettings.skinColor;
        thighLimb.endColor = bodySettings.coverSettings.knees ? bodySettings.coverSettings.color : bodySettings.skinColor;
        thighLimb.Initialize();

        if(upperCalfLimb == null)
        {
            this.isRight = isRight;
            string rightOrLeft;
            if (isRight)
                rightOrLeft = "Right";
            else
                rightOrLeft = "Left";
            GameObject go = new GameObject(rightOrLeft + "UpperCalf");
            calf = go.transform;
            go.layer = gameObject.layer;
            go.transform.SetParent(thigh, false);
            upperCalfLimb = go.AddComponent<GeneratedLimb>();
            upperCalfLimb.snapToParent = true;

            go = new GameObject(rightOrLeft + "LowerCalf");
            go.layer = gameObject.layer;
            go.transform.SetParent(upperCalfLimb.transform, false);
            lowerCalfLimb = go.AddComponent<GeneratedLimb>();
            lowerCalfLimb.snapToParent = true;
        }


        upperCalfLimb.length = bodySettings.upperCalfLength;
        upperCalfLimb.startRadius = bodySettings.kneeRadius;
        upperCalfLimb.endRadius = bodySettings.calfRadius;
        upperCalfLimb.mat = new Material(bodySettings.basicInGameObject);
        upperCalfLimb.startColor = bodySettings.coverSettings.knees ? bodySettings.coverSettings.color : bodySettings.skinColor;
        upperCalfLimb.endColor = bodySettings.coverSettings.calves ? bodySettings.coverSettings.color : bodySettings.skinColor;
        upperCalfLimb.Initialize();


        lowerCalfLimb.length = bodySettings.lowerCalfLength;
        lowerCalfLimb.startRadius = bodySettings.calfRadius;
        lowerCalfLimb.endRadius = bodySettings.ankleRadius;
        lowerCalfLimb.mat = new Material(bodySettings.basicInGameObject);
        lowerCalfLimb.startColor = bodySettings.coverSettings.calves ? bodySettings.coverSettings.color : bodySettings.skinColor;
        lowerCalfLimb.endColor = bodySettings.coverSettings.ankles ? bodySettings.coverSettings.color : bodySettings.skinColor;
        lowerCalfLimb.Initialize();
    }

    private void Awake()
    {
        //Quaternion initialWorldRotation = Quaternion.Lerp(leftHip.rotation, rightHip.rotation, 0.5f);
        //thigh.rotation = initialWorldRotation;
        //initialThighRotation = thigh.localRotation;
        initialCalfRotation = Quaternion.identity;
        initialFootRotation = Quaternion.identity;
    }

    public override void Idle(Vector3 movementDirection, ActionDirection actionDirection)
    {
        if (isRight)
        {
            thigh.localRotation = Quaternion.RotateTowards(thigh.localRotation,
                initialThighRotation*Quaternion.AngleAxis(10 * WaveVariables.sinTime  + 10, Vector3.up) * Quaternion.AngleAxis(-10 * WaveVariables.sinTimeRushQuarter - 10, Vector3.right),
                Time.deltaTime * 360);
        }
        else
        {
            thigh.localRotation = Quaternion.RotateTowards(thigh.localRotation,
                initialThighRotation * Quaternion.AngleAxis(-10 * WaveVariables.sinTime - 10, Vector3.up) * Quaternion.AngleAxis(-10 * WaveVariables.sinTimeRushQuarter - 10, Vector3.right),
                Time.deltaTime * 360);
        }

        calf.localRotation = Quaternion.RotateTowards(calf.localRotation,
                Quaternion.AngleAxis(30 * WaveVariables.sinTimeRushQuarter + 30, Vector3.right),
                Time.deltaTime * 360);
    }

}
