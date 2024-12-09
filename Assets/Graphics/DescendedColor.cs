using UnityEngine;

public class DescendedColor : Object
{
    [SerializeField]
    public static Color color = new Color(1,0,0);



    public static Color Red()
    {
        return new Color(1, 0.1f, 0.1f);
    }
}