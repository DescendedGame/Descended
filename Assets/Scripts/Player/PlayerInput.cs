using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Brain
{
    public KeyCode k_forwards = KeyCode.W;
    public KeyCode k_rightwards = KeyCode.D;
    public KeyCode k_backwards = KeyCode.S;
    public KeyCode k_leftwards = KeyCode.A;
    public KeyCode k_upwards = KeyCode.Space;
    public KeyCode k_downwards = KeyCode.LeftAlt;
    public KeyCode k_rollClockwise = KeyCode.E;
    public KeyCode k_rollCounterClockwise = KeyCode.Q;
    public KeyCode k_1 = KeyCode.Alpha1;
    public KeyCode k_2 = KeyCode.Alpha2;
    public KeyCode k_3 = KeyCode.Alpha3;
    public KeyCode k_4 = KeyCode.Alpha4;
    public KeyCode k_5 = KeyCode.Alpha5;
    public KeyCode k_primary = KeyCode.Mouse0;
    public KeyCode k_secondary = KeyCode.Mouse1;
    public KeyCode k_tertiary = KeyCode.Mouse4;
    public KeyCode k_sprint = KeyCode.LeftShift;
    public KeyCode k_balance = KeyCode.LeftControl;

    // Update is called once per frame
    public override void UpdateCommands()
    {
        SetMovementDirections();
        SetRotationDirections();
        SetToolActions();
    }

    void SetMovementDirections()
    {
        commands.forwards = 0;
        if (Input.GetKey(k_forwards)) commands.forwards += 1;
        if (Input.GetKey(k_backwards)) commands.forwards -= 1;

        commands.rightwards = 0;
        if (Input.GetKey(k_rightwards)) commands.rightwards += 1;
        if (Input.GetKey(k_leftwards)) commands.rightwards -= 1;

        commands.upwards = 0;
        if (Input.GetKey(k_upwards)) commands.upwards += 1;
        if (Input.GetKey(k_downwards)) commands.upwards -= 1;
    }

    void SetRotationDirections()
    {
        commands.roll = 0;
        if (Input.GetKey(k_rollClockwise)) commands.roll += 1;
        if (Input.GetKey(k_rollClockwise)) commands.roll -= 1;

        commands.look = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
    }

    void SetToolActions()
    {
        if (Input.GetKey(k_1)) commands.selected = 1;
        if (Input.GetKey(k_2)) commands.selected = 2;
        if (Input.GetKey(k_3)) commands.selected = 3;
        if (Input.GetKey(k_4)) commands.selected = 4;
        if (Input.GetKey(k_5)) commands.selected = 5;

        commands.primary = Input.GetKey(k_primary);
        commands.secondary = Input.GetKey(k_secondary);
        commands.tertiary = Input.GetKey(k_tertiary);
    }
}
