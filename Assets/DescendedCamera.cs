using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DescendedCamera : MonoBehaviour
{
    public Camera m_fogCam;
    public Camera m_sonarCam;
    int m_screenresy;
    int m_screenresx;

    private void Start()
    {
        Shader.SetGlobalFloat("Sonar", 0);
        Shader.SetGlobalVector("WaterAttenuation", new Vector4(5, 10, 25, 1));
        Shader.SetGlobalVector("FogColor", new Vector4(1f, 1f, 1f, 1f));
        m_screenresx = (int)((float)Screen.width * 0.75f);
        m_screenresy = (int)((float)Screen.height * 0.75f);
        UpdateRenderTexCam(m_fogCam, "FogTexture");
        UpdateRenderTexCam(m_sonarCam, "SonarTexture");
    }

    public void UpdateRenderTexCam(Camera p_cam, string p_propertyName)
    {
        m_screenresx = (int)((float)Screen.width * 0.75f);
        m_screenresy = (int)((float)Screen.height * 0.75f);
        p_cam.targetTexture.Release();
        RenderTexture t_rendertex = new RenderTexture(p_cam.targetTexture);
        t_rendertex.filterMode = FilterMode.Point;
        t_rendertex.width = m_screenresx;
        t_rendertex.height = m_screenresy;
        t_rendertex.Create();
        p_cam.targetTexture = t_rendertex;
        p_cam.targetTexture.filterMode = FilterMode.Point;
        Shader.SetGlobalTexture(p_propertyName, t_rendertex);
    }
}
