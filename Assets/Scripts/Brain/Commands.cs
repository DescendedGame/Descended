using UnityEngine;

public struct Commands
{
    /// <summary>
    /// Should only be -1, 0 and 1.
    /// </summary>
    public sbyte forwards, rightwards, upwards, roll;
    
    /// <summary>
    /// Which way the camera should be tilted.
    /// </summary>
    public Vector2 look;

    /// <summary>
    /// For toolbar selection.
    /// </summary>
    public byte selected;
    /// <summary>
    /// Each tool can have up to three actions (more if how they react in different pawn states are counted).
    /// Only returns true the frame it was pressed.
    /// </summary>
    public bool primary, secondary, tertiary;

    /// <summary>
    /// Each tool can have up to three actions (more if how they react in different pawn states are counted).
    /// </summary>
    public bool primaryHold, secondaryHold, tertiaryHold;

    public bool sprint;

    /// <summary>
    /// If this is toggled, the steering becomes less accurate and less confusing.
    /// </summary>
    public bool balance;
}
