using UnityEngine;

public abstract class BodyCreator : MonoBehaviour
{
    public abstract void CreateBody(out Transform atlasTransform, out Transform eyeTransform);
    public abstract void RecalculateBody();
}
