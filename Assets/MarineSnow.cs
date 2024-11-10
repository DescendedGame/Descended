using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles ambient particles that are supposed to resemble marine snow. It only works properly in play mode.
/// </summary>
public class MarineSnow : MonoBehaviour
{
    public ParticleSystem m_snow;
    float maxDistance;
    float maxDistanceSquared;
    float fall_speed;

    Vector3 m_prevPos;
    Vector3 m_move_direction;

    void Awake()
    {
        //The particle system should be set to reach its own maximum count withing one second of play.
        m_move_direction = Vector3.zero;
        m_prevPos = transform.position;
        maxDistance = m_snow.shape.radius;
        maxDistanceSquared = maxDistance * maxDistance;
        fall_speed = -m_snow.velocityOverLifetime.yMultiplier;
    }

    void LateUpdate()
    {
        RelocateSnow();
    }

    /// <summary>
    ///When a particle exits the sphere, it is teleported to a position where a new particle would enter the sphere.
    /// </summary>
    void RelocateSnow()
    {

        GetMoveDirection();

        ParticleSystem.Particle[] snow = new ParticleSystem.Particle[m_snow.particleCount];
        m_snow.GetParticles(snow);
        for (int i = snow.Length - 1; i >= 0; i--)
        {
            float distance = Vector3.Distance(snow[i].position, m_snow.transform.position);
            if (distance > maxDistance + 0.1f) //Seems particles move out of range instantly upon spawning in lower regions, so increased relocate distance slightly.
            {
                Vector3 t_rand = Random.insideUnitCircle;
                Quaternion t_lookrot = Quaternion.LookRotation(m_move_direction, Vector3.up);
                t_rand = t_lookrot * t_rand * maxDistance;
                snow[i].position = transform.position + m_move_direction.normalized * Mathf.Sqrt(maxDistanceSquared - t_rand.magnitude * t_rand.magnitude) + t_rand;
            }
        }
        m_snow.SetParticles(snow);
    }

    /// <summary>
    /// It is required for knowing in what direction to randomize a new snow flake position.
    /// </summary>
    void GetMoveDirection()
    {
        m_move_direction = Vector3.up * Time.deltaTime * fall_speed;

        if (m_prevPos != transform.position)
        {
            m_move_direction -= (m_prevPos - transform.position);
            m_prevPos = transform.position;
        }
    }

}
