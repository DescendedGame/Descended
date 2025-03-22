using UnityEngine;

[ExecuteInEditMode]
public class BodyStitcher : MonoBehaviour
{
    [SerializeField] GeneratedLimb rightSide;
    [SerializeField] GeneratedLimb leftSide;

    MeshRenderer skinRenderer;
    MeshFilter skinFilter;
    Mesh upperSkin;
    public Color startColor;
    public Color endColor;
    [SerializeField] Material mat;

    public void Initialize()
    {
        if (skinRenderer == null) skinRenderer = gameObject.GetComponent<MeshRenderer>();
        if (skinFilter == null) skinFilter = gameObject.GetComponent<MeshFilter>();
        if (skinRenderer == null) skinRenderer = gameObject.AddComponent<MeshRenderer>();
        if (skinFilter == null) skinFilter = gameObject.AddComponent<MeshFilter>();

        Vector3[] rightVertices;
        Vector3[] rightNormals;
        Vector2[] rightUvs;
        rightSide.GetVertexAtIndex(out rightVertices, out rightNormals, out rightUvs, transform);

        Vector3[] leftVertices;
        Vector3[] leftNormals;
        Vector2[] leftUvs;
        leftSide.GetVertexAtIndex(out leftVertices, out leftNormals, out leftUvs, transform);

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

        for(int i = 0; i < (segments)-2; i++)
        {
            if(i %2 == 0)
            {
                triangles[i * 6] = i;
                triangles[i * 6 + 1] = i + 2 + segments;
                triangles[i * 6 + 2] = i + segments;
                
                triangles[i * 6+3] = i;
                triangles[i * 6 + 4] = i + 2 ;
                triangles[i * 6 + 5] = i + segments + 2;
            }
            else
            {
                triangles[i * 6] = i;
                triangles[i * 6 + 2] = i + segments;
                if(i == 1)triangles[i * 6 + 1] = i - 1 + segments;
                else triangles[i * 6 + 1] = i - 2 + segments;

                triangles[i * 6 + 3] = i;
                if (i == 1)
                {
                    triangles[i * 6 + 4] = i - 1;
                    triangles[i * 6 + 5] = i - 1 + segments;
                }
                else
                {
                    triangles[i * 6 + 4] = i - 2;
                    triangles[i * 6 + 5] = i - 2 + segments;
                }
            }
            //triangles[i * 6 + 3] = i;
            //triangles[i * 6 + 3] = i+1+segments;
            //triangles[i * 6 + 3] = i+segments;
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

    // Update is called once per frame
    void Update()
    {

    }
}
