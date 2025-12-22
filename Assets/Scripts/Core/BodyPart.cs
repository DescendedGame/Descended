using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    Quaternion unalteredRotation;
    Vector3 unalteredPosition;

    protected Quaternion FollowParentSmoothly(Vector3 unalteredTransformPosition, Quaternion unalteredTransformRotation, Vector3 alteredPosition, Vector3 downVector, float followSpeed)
    {
        Vector3 t_vectorToLast = unalteredTransformPosition + unalteredTransformRotation * Vector3.down/followSpeed - alteredPosition;
        Vector3 t_vectorToCurrent = unalteredTransformRotation * downVector;
        return Quaternion.FromToRotation(t_vectorToCurrent, t_vectorToLast) * unalteredTransformRotation;
    }

    public virtual void RememberTransform()
    {
        unalteredPosition = transform.position;
        unalteredRotation = transform.rotation;
    }

    public virtual void Idle(Vector3 movementDirection, ActionDirection actionDirection)
    {

    }

    public virtual void Prepare(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Attack(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Defend(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Sprint(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Interact(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Toppled(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }

    public virtual void Grounded(Vector3 movementDirection, ActionDirection actionDirection)
    {
        Idle(movementDirection, actionDirection);
    }
}
