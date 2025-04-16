using UnityEngine;

public class HumanoidBodyCreator : MonoBehaviour
{
    public Material basicInGameObject;
    public GameObject headPrefab;
    public float upperNeckLength;
    public float upperNeckWidth;
    public float lowerNeckWidth;

    public float atlasLength;
    public float torsoDepth;

    public float torsoWidth;
    public float ribLength;
    public float waist;
    public float bellyLength;
    public float shoulderWidth;

    public float upperHipWidth;
    public float upperHipRadius;
    public float lowerHipRadius;
    public float hipLength;
    public float hipOutRotation;

    public Color skinColor;

    enum LowerBodyType
    {
        Human,
        Lizard,
        Fish,
        Pentapus,
    }

    private void Awake()
    {
        CreateNeckAndHead();
        CreateUpperTorso();
    }

    void CreateNeckAndHead()
    {
        CreateHead(CreateUpperNeck());
    }

    GeneratedLimb CreateUpperNeck()
    {
        GameObject go = new GameObject("Neck");
        go.transform.SetParent(transform, false);
        GeneratedLimb upperNeck = go.AddComponent<GeneratedLimb>();
        upperNeck.transform.localRotation = Quaternion.LookRotation(Vector3.up, -Vector3.forward);
        upperNeck.length = upperNeckLength;
        upperNeck.startRadius = lowerNeckWidth;
        upperNeck.endRadius = upperNeckWidth;
        upperNeck.mat = new Material(basicInGameObject);
        upperNeck.startColor = skinColor;
        upperNeck.endColor = skinColor;
        upperNeck.Initialize();
        return upperNeck;
    }

    Transform CreateHead(GeneratedLimb upperNeck)
    {
        GameObject head = Instantiate(headPrefab);
        head.transform.SetParent(upperNeck.transform, false);
        head.transform.localPosition = Vector3.forward * upperNeck.length;
        head.transform.localRotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
        MeshRenderer[] renderers = head.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in renderers)
        {
            renderer.sharedMaterial = basicInGameObject;
        }

        return head.transform;
    }

    Transform CreateUpperTorso()
    {
        CreateTorso(CreateAtlas());
        return null;
    }

    GeneratedLimb CreateAtlas()
    {
        GameObject go = new GameObject("Atlas");
        go.transform.SetParent(transform, false);
        GeneratedLimb atlas = go.AddComponent<GeneratedLimb>();
        atlas.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        atlas.length = atlasLength;
        atlas.startRadius = lowerNeckWidth;
        atlas.endRadius = torsoDepth;
        atlas.mat = new Material(basicInGameObject);
        atlas.startColor = skinColor;
        atlas.endColor = skinColor;
        atlas.Initialize();
        return atlas;
    }

    Vector3 CreateTorso(GeneratedLimb atlas)
    {
        Vector3 atlasEnd = atlas.LastPoint - Vector3.forward * atlas.endRadius;

        //Ribs
        //---------------------------------------------------------------------------------------------------
        GameObject go = new GameObject("LeftRibs");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd - Vector3.right * torsoWidth;
        GeneratedLimb leftRib = go.AddComponent<GeneratedLimb>();
        leftRib.length = ribLength;
        leftRib.startRadius = torsoDepth;
        leftRib.endRadius = waist;
        leftRib.mat = new Material(basicInGameObject);
        leftRib.startColor = skinColor;
        leftRib.endColor = skinColor;
        leftRib.Initialize();

        go = new GameObject("RightRibs");
        go.transform.SetParent(atlas.transform, false);
        go.transform.localPosition = atlasEnd + Vector3.right * torsoWidth;
        GeneratedLimb rightRib = go.AddComponent<GeneratedLimb>();
        rightRib.length = ribLength;
        rightRib.startRadius = torsoDepth;
        rightRib.endRadius = waist;
        rightRib.mat = new Material(basicInGameObject);
        rightRib.startColor = skinColor;
        rightRib.endColor = skinColor;
        rightRib.Initialize();

        go = new GameObject("RibSkin");
        go.transform.SetParent(rightRib.transform, false);
        BodyStitcher sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftRib;
        sticher.rightSide = rightRib;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();
        //----------------------------------------------------------------------------

        //Shoulders
        //----------------------------------------------------------------------------


        //----------------------------------------------------------------------------

        GameObject middleTorso = new GameObject("MiddleTorso");
        middleTorso.transform.SetParent(atlas.transform,false);
        middleTorso.transform.localRotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
        middleTorso.transform.localPosition = Vector3.forward * (atlasLength + ribLength);
        GameObject lowerTorso = new GameObject("LowerTorso");
        lowerTorso.transform.SetParent(middleTorso.transform, false);
        lowerTorso.transform.localPosition = Vector3.down * (bellyLength);

        //Hips
        //----------------------------------------------------------------------------
        //The hips will depend on what kind of lower body is desired.... but now just human.

        go = new GameObject("LeftHip");
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = - Vector3.right * upperHipWidth;
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-hipOutRotation, Vector3.forward) * Vector3.down, Vector3.forward);
        GeneratedLimb leftHip = go.AddComponent<GeneratedLimb>();
        leftHip.length = hipLength;
        leftHip.startRadius = upperHipRadius;
        leftHip.endRadius = lowerHipRadius;
        leftHip.mat = new Material(basicInGameObject);
        leftHip.startColor = skinColor;
        leftHip.endColor = skinColor;
        leftHip.Initialize();

        go = new GameObject("RightHip");
        go.transform.SetParent(lowerTorso.transform, false);
        go.transform.localPosition = Vector3.right * upperHipWidth;
        go.transform.localRotation = Quaternion.LookRotation(Quaternion.AngleAxis(hipOutRotation, Vector3.forward)*Vector3.down, Vector3.forward);
        GeneratedLimb rightHip = go.AddComponent<GeneratedLimb>();
        rightHip.length = hipLength;
        rightHip.startRadius = upperHipRadius;
        rightHip.endRadius = lowerHipRadius;
        rightHip.mat = new Material(basicInGameObject);
        rightHip.startColor = skinColor;
        rightHip.endColor = skinColor;
        rightHip.Initialize();

        go = new GameObject("HipSkin");
        go.transform.SetParent(rightHip.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftHip;
        sticher.rightSide = rightHip;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        //--------------------------------------------------------------------------------


        // Connect rib and hip with belly:
        //--------------------------------------------------------------------

        go = new GameObject("LeftBelly");
        go.transform.SetParent(leftRib.transform, false);
        GeneratedLimb leftBelly = go.AddComponent<GeneratedLimb>();
        leftBelly.snapToParent = true;
        leftBelly.startRadius = waist;
        leftBelly.endRadius = upperHipRadius;
        leftBelly.target = leftHip.transform;
        leftBelly.mat = new Material(basicInGameObject);
        leftBelly.startColor = skinColor;
        leftBelly.endColor = skinColor;
        leftBelly.Initialize();
        leftBelly.UpdateVertices();

        go = new GameObject("RightBelly");
        go.transform.SetParent(rightRib.transform, false);
        GeneratedLimb rightBelly = go.AddComponent<GeneratedLimb>();
        rightBelly.snapToParent = true;
        rightBelly.startRadius = waist;
        rightBelly.endRadius = upperHipRadius;
        rightBelly.target = rightHip.transform;
        rightBelly.mat = new Material(basicInGameObject);
        rightBelly.startColor = skinColor;
        rightBelly.endColor = skinColor;
        rightBelly.Initialize();
        rightBelly.UpdateVertices();

        go = new GameObject("BellySkin");
        go.transform.SetParent(rightBelly.transform, false);
        sticher = go.AddComponent<BodyStitcher>();
        sticher.leftSide = leftBelly;
        sticher.rightSide = rightBelly;
        sticher.mat = new Material(basicInGameObject);
        sticher.startColor = skinColor;
        sticher.endColor = skinColor;
        sticher.Initialize();

        return atlasEnd + atlas.transform.forward * ribLength;
    }

    Transform CreateMiddleTorso()
    {
        return null;
    }

    Transform CreateLowerTorso()
    {
        return null;
    }
}
