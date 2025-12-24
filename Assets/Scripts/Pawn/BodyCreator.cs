using UnityEngine;

public abstract class BodyCreator : MonoBehaviour
{
    public abstract void CreateBody(out Transform atlasTransform, out Transform eyeTransform, out Transform actionPoint);
    public abstract void RecalculateBody();

    public abstract float GetLength();
}
