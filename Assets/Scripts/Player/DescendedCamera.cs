using UnityEngine;
[ExecuteInEditMode]
public class DescendedCamera : MonoBehaviour
{
    public Camera m_object_camera;
    public Camera m_fog_camera;
    public Camera m_sonar_camera;

    [SerializeField] RenderTexture m_fog_texture;
    [SerializeField] RenderTexture m_sonar_texture;


    [Range(0, 180f)]
    public float m_field_of_view = 120;
    int m_screen_y;
    int m_screen_x;

    private void OnEnable()
    {
        Shader.SetGlobalFloat("Sonar", 0);
        Shader.SetGlobalVector("WaterAttenuation", new Vector4(5, 10, 25, 1));
        Shader.SetGlobalColor("FogColor", new Vector4(1f, 1f, 1f, 1f));
        m_screen_x = (int)((float)Screen.width * 0.75f);
        m_screen_y = (int)((float)Screen.height * 0.75f);

        UpdateRenderTexCam(m_fog_camera, "FogTexture", m_fog_texture);
        UpdateRenderTexCam(m_sonar_camera, "SonarTexture", m_sonar_texture);
    }

    void UpdateRenderTexture()
    {
        m_screen_x = (int)((float)Screen.width * 0.75f);
        m_screen_y = (int)((float)Screen.height * 0.75f);

        UpdateRenderTexCam(m_fog_camera, "FogTexture", m_fog_texture);
        UpdateRenderTexCam(m_sonar_camera, "SonarTexture", m_sonar_texture);
    }

    private void Update()
    {
        if(m_screen_x != (int)((float)Screen.width * 0.75f) || m_screen_y != (int)((float)Screen.height * 0.75f))
        {
            UpdateRenderTexture();
        }
    }

    public void UpdateRenderTexCam(Camera camera, string texture_name, RenderTexture targetTexture)
    {
        RenderTexture t_rendertex = new RenderTexture(targetTexture);
        t_rendertex.width = m_screen_x;
        t_rendertex.height = m_screen_y;
        t_rendertex.Create();
        if (camera.targetTexture != null)
        {
            RenderTexture previous = camera.targetTexture;
            camera.targetTexture = t_rendertex;
            previous.Release();
            DestroyImmediate(previous);
        }
        else camera.targetTexture = t_rendertex;
        Shader.SetGlobalTexture(texture_name, t_rendertex);
    }

    public void SetFieldOfView(float field_of_view)
    {
        m_object_camera.fieldOfView = field_of_view;
        m_fog_camera.fieldOfView = field_of_view;
        m_sonar_camera.fieldOfView = field_of_view;
    }

}
