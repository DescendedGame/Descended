using UnityEngine;

/// <summary>
/// Destroys the particle engine when it is certainly finished.
/// </summary>
public class ParticleExplosion : MonoBehaviour
{
    float duration;

    private void Awake()
    {
        duration = GetComponent<ParticleSystem>().main.startLifetime.constantMax;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0) Destroy(gameObject);
    }
}
