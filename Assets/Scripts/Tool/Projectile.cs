using UnityEngine;

public class Projectile : Hazard
{
    public float speed = 10f;
    public float lifetime = 3f;
    [SerializeField] ExplosionManager explosionManager;  // Reference to the explosion effect prefab
    [SerializeField] Rigidbody physics;

    public void Initialize()
    {
        explosionManager.TriggerParticles1(transform.position);
        physics.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    override protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        explosionManager.TriggerParticles1(transform.position);
    }

    override protected void OnTriggerEnter(Collider other) { }
}
