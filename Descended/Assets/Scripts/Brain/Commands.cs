using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Commands
{
    /// <summary>
    /// Should only be -1, 0 and 1.
    /// </summary>
    public sbyte forwards, rightwards, upwards, roll;
    
    public Vector2 look;
    public bool sprint;
    public bool balance;

    /// <summary>
    /// For toolbar selection.
    /// </summary>
    public byte selected;
    /// <summary>
    /// Each tool can have up to three actions.
    /// </summary>
    public bool primary, secondary, tertiary;
}
