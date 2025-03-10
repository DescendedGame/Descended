
using System;
using UnityEngine;

[Serializable]
public class PawnAttributes
{
    public Transform m_pivot;
    public Rigidbody m_physics;
    public float m_swim_force;
    public float m_swim_drag;
    public float roll_acc = 1500;
    public float roll_speed;
    public Tool[] tools;
    public int selectedToolIndex;
}