using UnityEngine;

public class ParticleSystem1 : MonoBehaviour
{
    [SerializeField] ExplosionManager particleManager; 
    private void Awake()
    {
        particleManager.SetParticles1(GetComponent<ParticleSystem>());
    }
}
