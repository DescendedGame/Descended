using UnityEngine;

public class BodyLinkage : BodyPart
{
    public float GetIKAngle(float length1, float length2, float targetLength)
    {
        if (length1 + length2 < targetLength) return 0;
        if (Mathf.Abs(length1 - length2) > targetLength) return 150;
        float angle = (length1 * length1 + length2 * length2 - targetLength * targetLength) / (2 * length1 * length2);
        return 180- Mathf.Acos(angle) * Mathf.Rad2Deg;
    }
}
