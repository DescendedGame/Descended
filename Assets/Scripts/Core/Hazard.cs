using UnityEngine;

/// <summary>
/// Inherit from this to create an object that can deal damage.
/// By default on collision, but can be overridden.
/// How something reacts upon the received damage it decided in its Attackable component.
/// </summary>
public class Hazard : MonoBehaviour
{
    /// <summary>
    /// A cut can cause high damage and inflict bleed, which causes suffocation damage!
    /// </summary>
    public float cut;

    /// <summary>
    /// Increases/decreases the temperature of a target, and extreme temperatures deal damage.
    /// </summary>
    public float temperature;

    /// <summary>
    /// Lack of oxygen deals damage and can cause sluggish movement.
    /// </summary>
    public float suffocation;

    /// <summary>
    /// Plain ol' instant damage. Goes straight on the target's balance too!
    /// </summary>
    public float impact;

    /// <summary>
    /// Lightning can cause something to get stuck in its current pose completely.
    /// </summary>
    public float stun;

    /// <summary>
    /// Doesn't take a lot of impact at the right place to cause something to trip.
    /// This deals no damage but impacts balance a lot.
    /// </summary>
    public float unbalance;

    /// <summary>
    /// Adds direct force to the target and sends them flying!
    /// </summary>
    [SerializeField] float push;

    /// <summary>
    /// Recalculate the push to a force vector to be used by the target that is hit.
    /// </summary>
    [HideInInspector] public Vector3 pushForce;


    //some default behaviour below, feel free to override.

    virtual protected void OnCollisionEnter(Collision collision)
    {
        pushForce = (collision.GetContact(0).point - transform.position).normalized * push;

        collision.collider?.GetComponent<Attackable>()?.Hit(this);
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        other?.GetComponent<Attackable>()?.Hit(this);
    }
}
