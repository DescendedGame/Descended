using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class BodyStitcher : MonoBehaviour
{
    public GeneratedLimb rightSide;
    public GeneratedLimb leftSide;

    MeshRenderer skinRenderer;
    MeshFilter skinFilter;
    Mesh upperSkin;
    public Color startColor;
    public Color endColor;
    public Material mat;

    public Transform rightStart;
    public Transform rightEnd;

    public Transform leftStart;
    public Transform leftEnd;

    public void Initialize()
    {
        if (skinRenderer == null) skinRenderer = gameObject.GetComponent<MeshRenderer>();
        if (skinFilter == null) skinFilter = gameObject.GetComponent<MeshFilter>();
        if (skinRenderer == null) skinRenderer = gameObject.AddComponent<MeshRenderer>();
        if (skinFilter == null) skinFilter = gameObject.AddComponent<MeshFilter>();

        Vector3[] rightVertices = new Vector3[13];
        Vector3[] rightNormals = new Vector3[13];
        Vector2[] rightUvs = new Vector2[13];
        rightStart = rightSide.transform;

        if(rightSide.target == null && rightEnd == null)
        {
            rightEnd = new GameObject("RightEndCheat").transform;
            rightEnd.SetParent(rightStart, false);
            rightEnd.localPosition = new Vector3(0, 0, rightSide.length);
        }
        else if (leftSide.target != null)
        {
            rightEnd = rightSide.target;
        }

        Vector3 rightStartToEnd = rightEnd.position - rightStart.position;
        rightStartToEnd = transform.InverseTransformVector(rightStartToEnd);

        Vector3[] leftVertices = new Vector3[13];
        Vector3[] leftNormals = new Vector3[13];
        Vector2[] leftUvs = new Vector2[13];
        leftStart = leftSide.transform;
        
        if (leftSide.target == null && leftEnd == null)
        {
            leftEnd = new GameObject("LeftEndCheat").transform;
            leftEnd.SetParent(leftStart, false);
            leftEnd.localPosition = new Vector3(0, 0, leftSide.length);
        }
        else if(leftSide.target != null)
        {
            leftEnd = leftSide.target;
        }

        Vector3 leftStartToEnd = leftEnd.position - leftStart.position;
        leftStartToEnd = transform.InverseTransformVector(leftStartToEnd);

        float rightAngle = CalculateAngle(rightSide.startRadius, rightSide.endRadius, rightSide.length);
        float leftAngle = CalculateAngle(leftSide.startRadius, leftSide.endRadius, leftSide.length);

        Vector3 rotationVector = transform.InverseTransformVector(leftSide.transform.position - rightSide.transform.position);
        float rightDegreeInterval = (90 + rightAngle) / 4;
        float leftDegreeInterval = (90 + leftAngle) / 4;
        for (int i = 0; i<4;i++)
        {
            rightNormals[i] = Quaternion.AngleAxis(-rightDegreeInterval * i, rotationVector) * Quaternion.AngleAxis(90, rotationVector) * Vector3.Cross(rotationVector, rightStartToEnd).normalized;
            rightVertices[i] = rightNormals[i]*rightSide.startRadius;
            rightUvs[i] = Vector2.one;

            leftNormals[i] =  Quaternion.AngleAxis(-leftDegreeInterval * i, rotationVector) * Quaternion.AngleAxis(90, rotationVector) * Vector3.Cross(rotationVector, rightStartToEnd).normalized;
            leftVertices[i] = transform.InverseTransformVector(leftSide.transform.position - rightSide.transform.position) + leftNormals[i]*leftSide.startRadius;
            leftUvs[i] = Vector2.one;
        }

        for (int i = 4; i<9; i++)
        {
            rightVertices[i] = rightStartToEnd * ((i-4) / 4f);
            leftVertices[i] = transform.InverseTransformVector(leftSide.transform.position - rightSide.transform.position) + leftStartToEnd * ((i - 4) / 4f);

            rightNormals[i] =Quaternion.AngleAxis(-rightAngle, leftVertices[i] - rightVertices[i]) * Vector3.Cross(leftVertices[i] - rightVertices[i], rightStartToEnd).normalized;
            leftNormals[i] = Quaternion.AngleAxis(-leftAngle, leftVertices[i] - rightVertices[i]) * Vector3.Cross(leftVertices[i] - rightVertices[i], leftStartToEnd).normalized;

            rightVertices[i] += rightNormals[i] * (rightSide.startRadius+ ((i - 4) / 4f)*(rightSide.endRadius-rightSide.startRadius));
            leftVertices[i] += leftNormals[i] * (rightSide.startRadius + ((i - 4) / 4f) * (rightSide.endRadius - rightSide.startRadius));

            rightUvs[i] = Vector2.one;
            leftUvs[i] = Vector2.one;
        }

        rotationVector = transform.InverseTransformVector(leftEnd.position - rightEnd.position);
        rightDegreeInterval = (90 - rightAngle) / 4;
        leftDegreeInterval = (90 - leftAngle) / 4;
        for (int i = 9; i < 13; i++)
        {
            rightNormals[i] = Quaternion.AngleAxis(-rightDegreeInterval * (i-9), rotationVector) * Quaternion.AngleAxis(rightAngle, rotationVector) * Vector3.Cross(rotationVector, rightStartToEnd).normalized;
            rightVertices[i] = transform.InverseTransformPoint(rightEnd.position) + rightNormals[i] * rightSide.endRadius;
            rightUvs[i] = Vector2.one;

            leftNormals[i] = Quaternion.AngleAxis(-leftDegreeInterval * (i - 9), rotationVector) * Quaternion.AngleAxis(leftAngle, rotationVector) * Vector3.Cross(rotationVector, rightStartToEnd).normalized;
            leftVertices[i] = transform.InverseTransformPoint(leftEnd.position) + leftNormals[i] * leftSide.endRadius;
            leftUvs[i] = Vector2.one;
        }


        //leftSide.GetVertexAtIndex(out leftVertices, out leftNormals, out leftUvs, transform);

        int segments = rightVertices.Length;

        Vector3[] allVertices = new Vector3[segments * 2];
        Vector3[] allNormals = new Vector3[segments * 2];
        Vector2[] allUvs = new Vector2[segments * 2];

        for (int i = 0; i < segments; i++)
        {
            allVertices[i] = rightVertices[i];
            allNormals[i] = rightNormals[i];
            allUvs[i] = rightUvs[i];

            allVertices[i + segments] = leftVertices[i];
            allNormals[i + segments] = leftNormals[i];
            allUvs[i + segments] = leftUvs[i];
        }

        int[] triangles = new int[6*segments];

        for(int i = 0; i < (segments)-1; i++)
        {
            {
                triangles[i * 6] = i + segments;
                triangles[i * 6 + 1] = i + segments + 1;
                triangles[i * 6 + 2] = i;

                triangles[i * 6 + 3] = i + segments + 1;
                triangles[i * 6 + 4] = i + 1;
                triangles[i * 6 + 5] = i;
            }
        }

        upperSkin = new Mesh();
        upperSkin.vertices = allVertices;
        upperSkin.normals = allNormals;
        upperSkin.uv = allUvs;
        upperSkin.triangles = triangles;
        
        Material thisOnesMat = new Material(mat);
        thisOnesMat.SetColor("_MainColor", startColor);
        thisOnesMat.SetColor("_TransitionColor", endColor);

        skinFilter.mesh = upperSkin;
        skinRenderer.material = thisOnesMat;
    }

    float CalculateAngle(float startRadius, float endRadius, float length)
    {
        float kat1 = startRadius - endRadius;
        float l2 = Mathf.Sqrt(length * length - kat1 * kat1);

        float sinValue = kat1 / l2;
        return Mathf.Asin(sinValue) * Mathf.Rad2Deg;
    }

    void Update()
    {
        Initialize();
    }
}
