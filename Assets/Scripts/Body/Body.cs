using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] protected Brain m_brain;
    [SerializeField] protected Rigidbody m_physics;
    [SerializeField] protected Transform m_pivot;
    [SerializeField] protected float m_swim_force;
    [SerializeField] protected float m_swim_drag;
    [SerializeField] ParticleSystem m_glitter;

    protected virtual void Awake()
    {
    }

    protected void Update()
    {
        m_brain.UpdateCommands();
        UpdateRotation();
        var emission = m_glitter.emission;
        emission.rateOverTime = m_physics.velocity.magnitude * 5;
    }

    protected virtual void UpdateRotation()
    {
    }

    protected virtual void FixedUpdate()
    {
    }
}
