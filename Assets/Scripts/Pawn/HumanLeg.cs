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

    public float GetLength()
    {
        return thighLimb.length + upperCalfLimb.length + lowerCalfLimb.length;
    }

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

    public override void Idle(PawnProperties pawnProperties, ActionDirection actionDirection)
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

    public override void Grounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {

    }

    public void ReachFor(Vector3 position)
    {
        float distance = (transform.position - position).magnitude;
        float angle = GetIKAngle(thighLimb.length, upperCalfLimb.length + lowerCalfLimb.length, distance);
        float angleOffset = (Mathf.Sin(angle * Mathf.Deg2Rad) / distance) * (upperCalfLimb.length + lowerCalfLimb.length);
        angleOffset = Mathf.Asin(angleOffset) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.LookRotation(position-transform.position, Vector3.Cross( position - transform.position, transform.parent.parent.right))*Quaternion.AngleAxis(angleOffset, -Vector3.right);
        thigh.rotation = Quaternion.RotateTowards(thigh.rotation, targetRotation, Time.deltaTime *360);
        calf.localRotation = Quaternion.RotateTowards(calf.localRotation, Quaternion.identity*Quaternion.AngleAxis(angle, Vector3.right), Time.deltaTime * 360);
    }

    public void MakeReady()
    {
        thigh.localRotation = Quaternion.RotateTowards(thigh.localRotation,
                initialThighRotation * Quaternion.AngleAxis(-130, Vector3.right),
                Time.deltaTime * 180);
        calf.localRotation = Quaternion.RotateTowards(calf.localRotation,
                Quaternion.AngleAxis(90, Vector3.right),
                Time.deltaTime * 180);
    }
}
