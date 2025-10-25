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

    public void Initialize(HumanoidBodyCreator creator, bool isRight)
    {

        if(thighLimb == null)
        {
            initialThighRotation = transform.localRotation;
            thighLimb = gameObject.AddComponent<GeneratedLimb>();
            thighLimb.snapToParent = true;
            thigh = transform;
        }
        thighLimb.length = creator.thighLength;
        thighLimb.startRadius = creator.lowerHipRadius;
        thighLimb.endRadius = creator.kneeRadius;
        thighLimb.mat = new Material(creator.basicInGameObject);
        thighLimb.startColor = creator.skinColor;
        thighLimb.endColor = creator.skinColor;
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


        upperCalfLimb.length = creator.upperCalfLength;
        upperCalfLimb.startRadius = creator.kneeRadius;
        upperCalfLimb.endRadius = creator.calfRadius;
        upperCalfLimb.mat = new Material(creator.basicInGameObject);
        upperCalfLimb.startColor = creator.skinColor;
        upperCalfLimb.endColor = creator.skinColor;
        upperCalfLimb.Initialize();


        lowerCalfLimb.length = creator.lowerCalfLength;
        lowerCalfLimb.startRadius = creator.calfRadius;
        lowerCalfLimb.endRadius = creator.ankleRadius;
        lowerCalfLimb.mat = new Material(creator.basicInGameObject);
        lowerCalfLimb.startColor = creator.skinColor;
        lowerCalfLimb.endColor = creator.skinColor;
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
