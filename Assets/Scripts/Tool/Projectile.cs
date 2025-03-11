using UnityEngine;

public class Projectile : Hazard
{
    /// <summary>
    /// Sets the velocity of the projectile to have this magnitude upon initialization.
    /// </summary>
    public float speed = 10f;

    /// <summary>
    /// How long this projectile lasts.
    /// </summary>
    public float lifetime = 3f;

    /// <summary>
    /// A reference to the ExplosionManager data item to use whatever explosions available.
    /// </summary>
    [SerializeField] ExplosionManager explosionManager;
    [SerializeField] Rigidbody physics;

    /// <summary>
    /// Call this after the prefab has been created and set at the right position.
    /// </summary>
    /// <param name="shootDirection"></param>
    public void Initialize()
    {
        explosionManager.TriggerParticles1(transform.position);
        physics.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    // Add some particle effects to each collision!
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        explosionManager.TriggerParticles1(transform.position);
    }

    // This ain't no trigger collider, so won't be needing this.
    protected override void OnTriggerEnter(Collider other) { }
}
