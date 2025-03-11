using UnityEngine;

/// <summary>
/// Contains input and GUI logic for the player pawn.
/// </summary>
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

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void UpdateCommands()
    {
        base.UpdateCommands();
        SetMovementDirections();
        SetRotationDirections();
        SetActions();
        SetToolActions();
    }

    void SetMovementDirections()
    {
        //Expects the ZeroCommands() to have been called before this.

        if (Input.GetKey(k_forwards)) commands.forwards += 1;
        if (Input.GetKey(k_backwards)) commands.forwards -= 1;

        if (Input.GetKey(k_rightwards)) commands.rightwards += 1;
        if (Input.GetKey(k_leftwards)) commands.rightwards -= 1;

        if (Input.GetKey(k_upwards)) commands.upwards += 1;
        if (Input.GetKey(k_downwards)) commands.upwards -= 1;
    }

    void SetRotationDirections()
    {
        //Expects the ZeroCommands() to have been called before this.

        if (Input.GetKey(k_rollClockwise)) commands.roll -= 1;
        if (Input.GetKey(k_rollCounterClockwise)) commands.roll += 1;

        commands.look = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
    }

    /// <summary>
    /// Listens for action bar selection input, and calls to action.
    /// </summary>
    void SetToolActions()
    {
        if (Input.GetKeyDown(k_1)) commands.selected = 0;
        if (Input.GetKeyDown(k_2)) commands.selected = 1;
        if (Input.GetKeyDown(k_3)) commands.selected = 2;
        if (Input.GetKeyDown(k_4)) commands.selected = 3;
        if (Input.GetKeyDown(k_5)) commands.selected = 4;

        if ((byte)(m_properties.tools.Length - 1) < commands.selected) commands.selected = (byte)(m_properties.tools.Length - 1);

        commands.primary = Input.GetKeyDown(k_primary);
        commands.secondary = Input.GetKeyDown(k_secondary);
        commands.tertiary = Input.GetKeyDown(k_tertiary);
        commands.primaryHold = Input.GetKey(k_primary);
        commands.secondaryHold = Input.GetKey(k_secondary);
        commands.tertiaryHold = Input.GetKey(k_tertiary);
    }

    void SetActions()
    {
        commands.sprint = Input.GetKey(k_sprint);
    }

    public override void OnDamaged(Hazard damage)
    {
        // Update healthbar not implemented
    }
}
