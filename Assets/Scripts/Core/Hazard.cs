using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float cut;
    public float temperature;
    public float suffocation;
    public float impact;
    public float stun;
    public float unbalance;
    [SerializeField] float push;
    [HideInInspector] public Vector3 pushForce;

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
