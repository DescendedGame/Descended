using UnityEngine;

public class WaveVariables : MonoBehaviour
{
    public static float sinTime = 0;
    public static float sinTimeRushQuarter = 0;
    public static float sinTimeDragQuarter = 0;

    void Update()
    {
        sinTime = Mathf.Sin(Time.time);
        sinTimeRushQuarter = Mathf.Sin(Time.time + Mathf.PI / 2);
        sinTimeDragQuarter = Mathf.Sin(Time.time - Mathf.PI / 2);
    }
}
