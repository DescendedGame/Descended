using UnityEngine;

public abstract class BodyCreator : MonoBehaviour
{
    public abstract void CreateBody(out Transform atlas, out Transform eyeTransform);
}
