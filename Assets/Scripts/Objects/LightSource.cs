using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float m_luminance;
    public Color m_color;

    [SerializeField] Light m_light_source;
    [SerializeField] Transform m_light_dome;
    [SerializeField] MeshRenderer m_light_orb;

    private void OnValidate()
    {
        m_light_source.color = m_color;
        if (Application.isPlaying) m_light_orb.material.SetColor("Luminescence", m_color);
        m_light_source.range = m_luminance;
        m_light_dome.localScale = new Vector3(m_luminance, m_luminance, m_luminance);
    }

    void SetLuminance(float luminance)
    {

    }

}
