using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        Shader.SetGlobalFloat("Sonar", 0);
        Shader.SetGlobalVector("WaterAttenuation", new Vector4(5, 10, 25, 1));
        Shader.SetGlobalColor("FogColor", new Vector4(1f, 1f, 1f, 1f));
        m_screen_x = (int)((float)Screen.width * 0.75f);
        m_screen_y = (int)((float)Screen.height * 0.75f);

        m_fog_camera.targetTexture = m_fog_texture;
        m_sonar_camera.targetTexture = m_sonar_texture;
        UpdateRenderTexCam(m_fog_camera, "FogTexture");
        UpdateRenderTexCam(m_sonar_camera, "SonarTexture");
    }

    public void UpdateRenderTexCam(Camera camera, string texture_name)
    {
        m_screen_x = (int)((float)Screen.width * 0.75f);
        m_screen_y = (int)((float)Screen.height * 0.75f);
        camera.targetTexture?.Release();
        RenderTexture t_rendertex = new RenderTexture(camera.targetTexture);
        t_rendertex.width = m_screen_x;
        t_rendertex.height = m_screen_y;
        t_rendertex.Create();
        camera.targetTexture = t_rendertex;
        Shader.SetGlobalTexture(texture_name, t_rendertex);
    }

    public void SetFieldOfView(float field_of_view)
    {
        m_object_camera.fieldOfView = field_of_view;
        m_fog_camera.fieldOfView = field_of_view;
        m_sonar_camera.fieldOfView = field_of_view;
    }

}
