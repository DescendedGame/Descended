using UnityEngine;

public class WaveVariables : MonoBehaviour
{
    public static float sinTime = 0;
    public static float sinTime4 = 0;
    public static float sinTime8 = 0;
    public static float sinTimeRushQuarter = 0;
    public static float sinTimeRushQuarter4 = 0;
    public static float sinTimeRushQuarter8 = 0;
    public static float sinTimeDragQuarter = 0;
    public static float sinTimeDragQuarter4 = 0;
    public static float sinTimeDragQuarter8 = 0;

    void Update()
    {
        sinTime = Mathf.Sin(Time.time);
        sinTime4 = Mathf.Sin(Time.time*4);
        sinTime8 = Mathf.Sin(Time.time * 8);
        sinTimeRushQuarter = Mathf.Sin(Time.time + Mathf.PI / 2);
        sinTimeRushQuarter4 = Mathf.Sin(Time.time*4 + Mathf.PI / 2);
        sinTimeRushQuarter8 = Mathf.Sin(Time.time * 8 + Mathf.PI / 2);
        sinTimeDragQuarter = Mathf.Sin(Time.time - Mathf.PI / 2);
        sinTimeDragQuarter4 = Mathf.Sin(Time.time*4 - Mathf.PI / 2);
        sinTimeDragQuarter8 = Mathf.Sin(Time.time * 8 - Mathf.PI / 2);
    }
}
