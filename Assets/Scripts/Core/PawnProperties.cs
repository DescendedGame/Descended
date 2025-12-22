using System;
using UnityEngine;

/// <summary>
/// Contains important properties of the pawn, which can be passed around as a reference together.
/// </summary>
[Serializable]
public class PawnProperties
{
    /// <summary>
    /// The part of the pawn that rotates, check its forward vector to know which direction it faces.
    /// </summary>
    public Transform m_pivot;

    /// <summary>
    /// The pawn's rigid body.
    /// </summary>
    public Rigidbody m_physics;

    /// <summary>
    /// The pawn's movement acceleration (in regards to physics).
    /// </summary>
    public float m_swim_force;
    public float m_swim_drag;

    /// <summary>
    /// How fast the pawn accelerates its "roll".
    /// </summary>
    public float roll_acc = 1500;

    /// <summary>
    /// The current roll speed.
    /// </summary>
    public float roll_speed;

    /// <summary>
    /// Should maybe be kept elsewhere. If this runs out, the pawn will be toppled.
    /// </summary>
    public float m_balance;

    /// <summary>
    /// Used by tools to know where to act.
    /// </summary>
    public Transform actionPoint;

    /// <summary>
    /// A data item that contains all the available tools in the game.
    /// </summary>
    public AllTools toolStorage;

    /// <summary>
    /// The tool types that should be readied by the pawn, can be 5.
    /// </summary>
    public ToolType[] toolTypes;

    /// <summary>
    /// The readied tools for the pawn, can be 5.
    /// </summary>
    public Tool[] tools;
    public int selectedToolIndex;
    public Tool selectedTool
    {
        get { return tools[selectedToolIndex]; }
        private set { }
    }

    /// <summary>
    /// All the body parts of this pawn. Should maybe be kept elsewhere.
    /// </summary>
    public BodyPart[] bodyParts;

    /// <summary>
    /// Where to place camera for player, from where to raycast vision for npc
    /// </summary>
    public Transform eyeTransform;
}

/// <summary>
/// A pawn is a state machine that can have these state types.
/// </summary>
public enum PawnStateType
{
    Idle,
    Prepare,
    Attack,
    Defend,
    Sprint,
    Interact,
    Toppled,
    Grounded,
}

/// <summary>
/// A pawn can perform actions in different directions.
/// </summary>
public enum ActionDirection
{
    Up,
    Down,
    Left,
    Right,
}