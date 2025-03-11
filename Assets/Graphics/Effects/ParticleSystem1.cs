using UnityEngine;

/// <summary>
/// A particle system that should be accessible in the ExplosionManager must report in.
/// </summary>
public class ParticleSystem1 : MonoBehaviour
{
    [SerializeField] ExplosionManager particleManager; 
    private void Awake()
    {
        particleManager.SetParticles1(GetComponent<ParticleSystem>());
    }
}
