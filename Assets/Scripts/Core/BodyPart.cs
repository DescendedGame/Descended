using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    protected Quaternion DragBehind(Vector3 previousPosition, Quaternion previousRotation, Vector3 currentPosition, Quaternion currentRotation, Vector3 backwardsVector, float weightCenterDistance = 0.3f)
    {
        float moveDistance = Vector3.Distance(previousPosition, currentPosition);
        Vector3 lastEnd = previousPosition + previousRotation * backwardsVector * weightCenterDistance;
        Vector3 wannabeBackwards = lastEnd - currentPosition;
        Vector3 torsoBackwards = currentRotation * backwardsVector * weightCenterDistance;
        Vector3 rotationAxis = Vector3.Cross(wannabeBackwards, torsoBackwards);
        return Quaternion.AngleAxis(Mathf.Clamp(moveDistance * 90,0, Vector3.Angle(wannabeBackwards, torsoBackwards)), 
            -rotationAxis) * currentRotation;
    }

    public virtual void RememberTransform()
    {
    }

    public virtual void Idle(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {

    }

    public virtual void Prepare(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void PrepareGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Grounded(commands, pawnProperties, actionDirection);
    }

    public virtual void Attack(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void AttackGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Grounded(commands, pawnProperties, actionDirection);
    }

    public virtual void Defend(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle( commands, pawnProperties, actionDirection);
    }

    public virtual void Sprint(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void Interact(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void Toppled(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void Grounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void DefendGrounded(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }

    public virtual void Dodge(Commands commands, PawnProperties pawnProperties, ActionDirection actionDirection)
    {
        Idle(commands, pawnProperties, actionDirection);
    }
}
