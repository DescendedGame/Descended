using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GeneratedLimb : MonoBehaviour
{
    public bool renderHalf = false;
    public bool snapToParent = false;
    public Transform target;

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
    public Material mat;

    MeshRenderer limbRenderer;
    MeshFilter limbFilter;
    Mesh limbMesh;

    [SerializeField] bool initialized = false;

    public void Initialize()
    {
        SetInitialState();
        SetUpComponents();
        CreateMesh();
    }

    void SetInitialState()
    {
        if (target != null)
        {
            length = Vector3.Distance(transform.position, target.position);
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, transform.parent.up + target.up);
            GeneratedLimb targetLimb = target.GetComponent<GeneratedLimb>();
            if(targetLimb != null)
            {
                endRadius = targetLimb.startRadius;
            }
        }

        if (snapToParent)
        {
            GeneratedLimb prevLimbPart = transform.parent?.GetComponent<GeneratedLimb>();
            if (prevLimbPart != null)
            {
                startRadius = prevLimbPart.endRadius;
                transform.localPosition = prevLimbPart.LastPoint - startRadius * Vector3.forward;
            }
        }

        if (startRadius < 0.001f) startRadius = 0.001f;
        if (endRadius < 0.001f) endRadius = 0.001f;
        if (length < 0.001f) length = 0.001f;
        prevStartRadius = startRadius;
        prevEndRadius = endRadius;
        prevLength = length;

    }

    void SetUpComponents()
    {
        if (limbRenderer == null) limbRenderer = gameObject.AddComponent<MeshRenderer>();
        if (limbFilter == null) limbFilter = gameObject.AddComponent<MeshFilter>();
        if (limbMesh == null) limbMesh = new Mesh();
    }

    void CreateMesh()
    {
        float slopeAngle = CalculateAngle();

        float upperAngle = slopeAngle + 90;
        float lowerAngle = 180 - (slopeAngle + 90);

        //int upperSegments = (int)(upperAngle / 30) + 1;
        //int lowerSegments = (int)(lowerAngle / 30) + 1;
        int upperSegments = 4;
        int lowerSegments = 4;

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

        limbMesh.vertices = verts;
        limbMesh.normals = normals;
        limbMesh.triangles = tris;
        limbMesh.uv = uvs;
        limbMesh.RecalculateBounds();

        limbFilter.mesh = limbMesh;

        Material thisOnesMat = new Material(mat);
        thisOnesMat.SetColor("_MainColor", startColor);
        thisOnesMat.SetColor("_TransitionColor", endColor);
        limbRenderer.material = thisOnesMat;
        limbRenderer.material = thisOnesMat;

        initialized = true;
    }

    /// <returns>The slope from start to end in degrees</returns>
    float CalculateAngle()
    {
        float kat1 = startRadius - endRadius;
        float l2 = Mathf.Sqrt(length * length + kat1 * kat1);

        float sinValue = kat1 / l2;
        return Mathf.Asin(sinValue) * Mathf.Rad2Deg;
    }

    public void UpdateVertices()
    {
        if (target != null)
        {
            length = Vector3.Distance(transform.position, target.position);
            transform.LookAt(target, target.up + transform.parent.up);
        }

        float slopeAngle = CalculateAngle();

        float upperAngle = slopeAngle + 90;
        float lowerAngle = 180 - (slopeAngle + 90);

        //int upperSegments = (int)(upperAngle / 30) + 1;
        //int lowerSegments = (int)(lowerAngle / 30) + 1;
        int upperSegments = 4;
        int lowerSegments = 4;

        float upperAngleDiff = upperAngle / upperSegments;
        float lowerAngleDiff = lowerAngle / lowerSegments;


        Vector3[] verts = new Vector3[12 * (upperSegments - 1) + 12 * (lowerSegments - 1) + 24 + 2];
        Vector3[] normals = new Vector3[verts.Length];
        int segmentCount = upperSegments + lowerSegments;

        int current = 0;

        verts[current] = Vector3.back * startRadius;
        FirstPoint = verts[current];
        normals[current] = Vector3.back;
        current++;

        float rotatingAngle = 0;
        float sphereAngle = upperAngleDiff;

        for (int i = 0; i < upperSegments; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if (!renderHalf)
                {
                    verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * startRadius));
                    normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                }
                else
                {
                    if (rotatingAngle <= 90 || rotatingAngle >= 270)
                    {
                        verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * startRadius));
                        normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                    }
                    else
                    {
                        verts[current] = Vector3.zero;
                        normals[current] = Vector3.up;
                    }
                }
                    
                rotatingAngle += 30;
                current++;
            }
            rotatingAngle = 0;
            sphereAngle += upperAngleDiff;
        }

        sphereAngle = upperAngle;
        rotatingAngle = 0;
        for (int i = 0; i < lowerSegments; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if(!renderHalf)
                {
                    verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * endRadius)) + Vector3.forward * length;
                    normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                }
                else
                {
                    if (rotatingAngle <= 90 || rotatingAngle >= 270)
                    {
                        verts[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * (Vector3.back * endRadius)) + Vector3.forward * length;
                        normals[current] = Quaternion.AngleAxis(rotatingAngle, Vector3.forward) * (Quaternion.AngleAxis(sphereAngle, Vector3.right) * Vector3.back);
                    }
                    else
                    {
                        verts[current] = Vector3.zero;
                        normals[current] = Vector3.up;
                    }
                }
                
                rotatingAngle += 30;
                current++;
            }
            rotatingAngle = 0;
            sphereAngle += lowerAngleDiff;
        }
        verts[current] = Vector3.forward * endRadius + Vector3.forward * length;
        LastPoint = verts[current];
        normals[current] = Vector3.forward;

        limbFilter.sharedMesh.vertices = verts;
        limbFilter.sharedMesh.normals = normals;
        limbFilter.sharedMesh.RecalculateBounds();

    }

    private void Update()
    {
        if (target != null && initialized)
        {
            UpdateVertices();
        }
        else if ((prevLength != length) || prevStartRadius != startRadius || prevEndRadius != endRadius)
        {
            HumanoidBodyCreator creator = transform.root.GetComponent<HumanoidBodyCreator>();
            creator?.RecalculateBody();
        }
        
    }
}
