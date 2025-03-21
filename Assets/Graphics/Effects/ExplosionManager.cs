
using UnityEngine;

/// <summary>
/// Particle systems in the game scene should report in to this, then anything that needs to use particles can easily do so through this data item.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ParticleExplosion", order = 1)]
public class ExplosionManager : ScriptableObject
{
    ParticleSystem particles1;

    public void SetParticles1(ParticleSystem p_particles1)
    {
        particles1 = p_particles1;
    }

    public void TriggerParticles1(Vector3 position)
    {
        if(particles1 != null)
        {
            particles1.transform.position = position;
            particles1.Emit(100);
        }
    }

}