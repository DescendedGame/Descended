using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    public virtual void Idle()
    {

    }

    public virtual void Prepare()
    {
        Idle();
    }

    public virtual void Attack()
    {
        Idle();
    }

    public virtual void Defend()
    {
        Idle();
    }

    public virtual void Sprint()
    {
        Idle();
    }

    public virtual void Interact()
    {
        Idle();
    }

    public virtual void Toppled()
    {
        Idle();
    }

}
