using UnityEngine;

[ExecuteInEditMode]
public class DynamicLimb : MonoBehaviour
{

    public Transform target;

    public float startRadius;
    public float endRadius;
    private float length;
    float prevStartRadius;
    float prevEndRadius;
    float prevLength;
    public Vector3 FirstPoint;
    public Vector3 LastPoint;
    public Color startColor;
    public Color endColor;
    [SerializeField] Material mat;

    ArmInitializer m_initializer;

    MeshRenderer armRenderer;
    MeshFilter armFilter;
    Mesh upperArm;

    int[] stitchSeams;

    [SerializeField] bool initialized;

    public void GetVertexAtIndex(out Vector3[] vertices, out Vector3[] normals, out Vector2[] uvs, Transform transformSpace)
    {
        vertices = new Vector3[stitchSeams.Length];
        normals = new Vector3[stitchSeams.Length];
        uvs = new Vector2[stitchSeams.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transformSpace.InverseTransformPoint(transform.TransformPoint(armFilter.sharedMesh.vertices[stitchSeams[i]]));
            normals[i] = transformSpace.InverseTransformDirection(transform.TransformDirection(armFilter.sharedMesh.normals[stitchSeams[i]]));
            uvs[i] = armFilter.sharedMesh.uv[stitchSeams[i]];
        }

    }

    public void Initialize(ArmInitializer initializer)
    {
        initialized = true;
        if (startRadius < 0.001f) startRadius = 0.001f;
        if (endRadius < 0.001f) endRadius = 0.001f;

        length = Vector3.Distance(transform.position, target.position);
        transform.LookAt(target, target.up);

        if (length < 0.001f) length = 0.001f;
        prevStartRadius = startRadius;
        prevEndRadius = endRadius;
        prevLength = length;

        Material thisOnesMat = new Material(mat);
        thisOnesMat.SetColor("_MainColor", startColor);
        thisOnesMat.SetColor("_TransitionColor", endColor);

        m_initializer = initializer;
        GeneratedLimb prevLimbPart = transform.parent?.GetComponent<GeneratedLimb>();
        if (prevLimbPart != null)
        {
            transform.localPosition = prevLimbPart.LastPoint - startRadius * Vector3.forward;
        }

        if (armRenderer == null) armRenderer = gameObject.GetComponent<MeshRenderer>();
        if (armFilter == null) armFilter = gameObject.GetComponent<MeshFilter>();
        if (armRenderer == null) armRenderer = gameObject.AddComponent<MeshRenderer>();
        if (armFilter == null) armFilter = gameObject.AddComponent<MeshFilter>();

        upperArm = new Mesh();


        float kat1 = startRadius - endRadius;
        float l2 = Mathf.Sqrt(length * length - kat1 * kat1);

        float sinValue = kat1 / l2;
        float sinAngle = Mathf.Asin(sinValue) * Mathf.Rad2Deg;

        float upperAngle = sinAngle + 90;



        Vector3[] verts = new Vector3[24];
        Vector3[] normals = new Vector3[verts.Length];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[(12 * 3 * 2)];


        int current = 0;

        FirstPoint = Vector3.back * startRadius;

        float rotatingAngle = 0;

        for (int j = 0; j < 12; j++)
        {
            //if (rotatingAngle % 180 == 0)
            //    debugIndices.Add(current);
            verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * (Vector3.back * startRadius));
            normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * Vector3.back);
            uvs[current] = new Vector2(rotatingAngle / 360, 0);
            rotatingAngle += 30;
            current++;
        }

        for (int j = 0; j < 12; j++)
        {
            verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * (Vector3.back * endRadius)) + Vector3.forward * length;
            normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * Vector3.back);
            uvs[current] = new Vector2(rotatingAngle / 360, 1);
            rotatingAngle += 30;
            current++;
        }

        LastPoint = Vector3.forward * endRadius + Vector3.forward * length;




        for (int i = 0; i < 11; i++)
        {
            int firstIndex = i * 6;
            tris[firstIndex] = i;
            tris[firstIndex + 1] = i + 13;
            tris[firstIndex + 2] = i + 12;
            tris[firstIndex + 3] = i;
            tris[firstIndex + 4] = i + 1;
            tris[firstIndex + 5] = i + 13;
        }
        int currentTris = 6 * 11;

        tris[currentTris] = 11;
        tris[currentTris + 1] = 12;
        tris[currentTris + 2] = 11 + 12;

        tris[currentTris + 3] = 11;
        tris[currentTris + 4] = 0;
        tris[currentTris + 5] = 12;


        upperArm.vertices = verts;
        upperArm.normals = normals;
        upperArm.triangles = tris;
        upperArm.uv = uvs;
        upperArm.RecalculateBounds();

        armFilter.mesh = upperArm;

        armRenderer.material = thisOnesMat;

        //stitchSeams = debugIndices.ToArray();
        initialized = true;
    }

    public void UpdateVertices()
    {
        length = Vector3.Distance(transform.position, target.position);
        transform.LookAt(target, target.up);

        float kat1 = startRadius - endRadius;
        float l2 = Mathf.Sqrt(length * length - kat1 * kat1);

        float sinValue = kat1 / l2;
        float sinAngle = Mathf.Asin(sinValue) * Mathf.Rad2Deg;

        float upperAngle = sinAngle + 90;

        Vector3[] verts = new Vector3[24];
        Vector3[] normals = new Vector3[verts.Length];

        int current = 0;

        FirstPoint = Vector3.back * startRadius;

        float rotatingAngle = 0;

        for (int j = 0; j < 12; j++)
        {
            //if (rotatingAngle % 180 == 0)
            //    debugIndices.Add(current);
            verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * (Vector3.back * startRadius));
            normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * Vector3.back);
            rotatingAngle += 30;
            current++;
        }

        for (int j = 0; j < 12; j++)
        {
            verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * (Vector3.back * endRadius)) + Vector3.forward * length;
            normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(upperAngle, Vector3.right) * Vector3.back);
            rotatingAngle += 30;
            current++;
        }

        LastPoint = Vector3.forward * endRadius + Vector3.forward * length;

    }

    private void Update()
    {
        if (initialized)
        {
            UpdateVertices();
        }
        if (prevLength != length || prevStartRadius != startRadius || prevEndRadius != endRadius)
        {
            Initialize(null);
            //m_initializer?.Initialize();
        }
        //if (gameObject.name == "RightRib")
        //{
        //    foreach (int index in stitchSeams)
        //    {
        //        Debug.DrawRay(transform.TransformPoint(armFilter.sharedMesh.vertices[index]),
        //            transform.TransformDirection(armFilter.sharedMesh.normals[index]), Color.white);
        //    }
        //}
    }


}
