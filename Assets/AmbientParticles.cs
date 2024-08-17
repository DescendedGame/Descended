using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientParticles : MonoBehaviour
{
    public ParticleSystem m_snow;
    ParticleSystem.EmitParams m_params = new ParticleSystem.EmitParams();

    Vector3 m_prevPos;
    Vector3 m_movedist;

    void Start()
    {
        m_movedist = Vector3.zero;
        m_snow.trigger.AddCollider(m_snow.GetComponent<SphereCollider>());
        m_snow.trigger.AddCollider(m_snow.GetComponent<BoxCollider>());
        m_prevPos = transform.position;
    }

    void Update()
    {
        m_movedist += Vector3.up * Time.deltaTime * 0.05f;
        m_params.position = (transform.position);

        if (m_prevPos != transform.position)
        {
            m_movedist -= (m_prevPos - transform.position);
            m_prevPos = transform.position;
        }

        while (m_movedist.magnitude > 0.005f)
        {
            Vector3 t_rand = Random.insideUnitCircle;

            Quaternion t_lookrot = Quaternion.LookRotation(m_movedist, Vector3.up);
            t_rand = t_lookrot * t_rand * 10;
            m_params.position = transform.position + m_movedist.normalized * Mathf.Sqrt(100 - t_rand.magnitude * t_rand.magnitude) + t_rand;
            m_snow.Emit(m_params, 1);
            m_movedist -= m_movedist.normalized * 0.01f;

        }

    }
}
