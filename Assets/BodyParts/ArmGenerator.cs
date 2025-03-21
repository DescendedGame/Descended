using UnityEngine;

[ExecuteInEditMode]
public class ArmGenerator : MonoBehaviour
{
    public float startRadius;
    public float endRadius;
    public float length;
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
    

    public void Initialize(ArmInitializer initializer)
    {
        if (startRadius < 0.001f) startRadius = 0.001f;
        if (endRadius < 0.001f) endRadius = 0.001f;
        if (length < 0.001f) length = 0.001f;
        prevStartRadius = startRadius;
        prevEndRadius = endRadius;
        prevLength = length;

        Material thisOnesMat = new Material(mat);
        thisOnesMat.SetColor("_MainColor", startColor);
        thisOnesMat.SetColor("_TransitionColor", endColor);

        m_initializer = initializer;
        ArmGenerator prevLimbPart = transform.parent?.GetComponent<ArmGenerator>();
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
        float l2 = Mathf.Sqrt(length*length - kat1*kat1);

        //float hypo = Mathf.Sqrt(kat1 * kat1 + length * length);
        float sinValue = kat1 / l2;
        float sinAngle = Mathf.Asin(sinValue) * Mathf.Rad2Deg;

        float upperAngle = sinAngle + 90;
        float lowerAngle = 180 - (sinAngle + 90);

        int upperSegments = (int)(upperAngle / 30) + 1;
        int lowerSegments = (int)(lowerAngle / 30) + 1;

        float upperAngleDiff = upperAngle / upperSegments;
        float lowerAngleDiff = lowerAngle / lowerSegments;


        Vector3[] verts = new Vector3[12 * (upperSegments - 1) + 12 * (lowerSegments - 1) + 24 + 2];
        Vector3[] normals = new Vector3[verts.Length];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[(12 * 3 * 2)/*top and bottom*/+ (verts.Length - 2 - 12) * 6/*all 12 fillers*/];
        int segmentCount = upperSegments + lowerSegments;

        int current = 0;

        verts[current] = Vector3.back * startRadius;
        FirstPoint = verts[current];
        uvs[current] = Vector2.zero;
        normals[current] = Vector3.back;
        current++;

        float rotatingAngle = 0;
        float sphereAngle = upperAngleDiff;

        for (int i = 0; i < upperSegments; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * startRadius));
                normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                uvs[current] = new Vector2(rotatingAngle / 360, 0);
                rotatingAngle += 30;
                current++;
            }
            sphereAngle += upperAngleDiff;
        }
        
        sphereAngle = upperAngle;
        rotatingAngle = 0;
        for (int i = 0; i < lowerSegments; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * endRadius)) + Vector3.forward * length;
                normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                uvs[current] = new Vector2(rotatingAngle / 360, 1);
                rotatingAngle += 30;
                current++;
            }
            sphereAngle += lowerAngleDiff;
        }
        verts[current] = Vector3.forward * endRadius + Vector3.forward * length;
        LastPoint = verts[current]; 
        uvs[current] = new Vector2(1, 1);
        normals[current] = Vector3.forward;

        for (int i = 1; i < 12; i++)
        {
            tris[(i - 1) * 3] = 0;
            tris[(i - 1) * 3 + 1] = i + 1;
            tris[(i - 1) * 3 + 2] = i;
        }
        tris[33] = 0;
        tris[33 + 1] = 1;
        tris[33 + 2] = 12;

        int currentTris = 36;


        for (int i = 0; i < segmentCount - 1; i++)
        {
            int firstIndex;
            for (int j = 0; j < 11; j++)
            {
                firstIndex = 1 + j + i * 12;
                tris[currentTris] = firstIndex;
                tris[currentTris + 1] = firstIndex + 13;
                tris[currentTris + 2] = firstIndex + 12;
                tris[currentTris + 3] = firstIndex;
                tris[currentTris + 4] = firstIndex + 1;
                tris[currentTris + 5] = firstIndex + 13;
                currentTris += 6;
            }
            firstIndex = 1 + 11 + i * 12;
            tris[currentTris] = firstIndex;
            tris[currentTris + 1] = firstIndex + 1;
            tris[currentTris + 2] = firstIndex + 12;
            tris[currentTris + 3] = firstIndex;
            tris[currentTris + 4] = firstIndex - 11;
            tris[currentTris + 5] = firstIndex + 1;
            currentTris += 6;
        }

        current = verts.Length - 1 - 12;

        for (int i = 0; i < 11; i++)
        {
            tris[currentTris] = current + i;
            tris[currentTris + 1] = current + i + 1;
            tris[currentTris + 2] = verts.Length - 1;
            currentTris += 3;
        }

        tris[currentTris] = current + 11;
        tris[currentTris + 1] = current;
        tris[currentTris + 2] = verts.Length - 1;

        upperArm.vertices = verts;
        upperArm.normals = normals;
        upperArm.triangles = tris;
        upperArm.uv = uvs;
        upperArm.RecalculateBounds();

        armFilter.mesh = upperArm;

        armRenderer.material = thisOnesMat;


    }

    private void Update()
    {
        if (prevLength != length || prevStartRadius != startRadius || prevEndRadius != endRadius)
        {
            m_initializer?.InitializeRecursively(m_initializer.transform);
        }
    }
}
