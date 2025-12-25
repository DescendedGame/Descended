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

    public virtual void Idle(PawnProperties pawnProperties, ActionDirection actionDirection)
    {

    }

    public virtual void Prepare(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void PrepareGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Grounded(pawnProperties, actionDirection);
    }

    public virtual void Attack(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void AttackGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Grounded(pawnProperties, actionDirection);
    }

    public virtual void Defend(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void Sprint(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void Interact(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void Toppled(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void Grounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void DefendGrounded(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }

    public virtual void Dodge(PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(pawnProperties, actionDirection);
    }
}
