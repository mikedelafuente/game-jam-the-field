using System.Collections.Generic;
using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public partial class Player : CharacterBody2D
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    internal AnimationTree AnimationTree;
    internal Vector2 LastMoveDirection = Vector2.Zero;
    internal FacingDirection CurrentDirection = FacingDirection.Down;
    public FiniteStateMachine FSM { get; private set; } = new();
    public const float Speed = 32.0f;
    private List<string> _pressedKeys = new();

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("Sprite2D/AnimationPlayer/AnimationTree");
        AnimationTree.Active = true;
     
        // Add states to the FSM and initialize the starting state
        FSM.Add("Idle", new IdleState(this));
        FSM.Add("Walk", new WalkState(this));
        FSM.InitialiseState("Idle"); // Start in Idle state
    }

    public override void _PhysicsProcess(double delta)
    {
        FSM.ExecuteStatePhysics((float)delta);
    }

    public void UpdatePressedKeys()
    {
        // Track individual key presses and their order
        TrackKey("ui_left");
        TrackKey("ui_right");
        TrackKey("ui_up");
        TrackKey("ui_down");
    }

    private void TrackKey(string action)
    {
        if (Input.IsActionJustPressed(action))
        {
            if (!_pressedKeys.Contains(action))
                _pressedKeys.Add(action);
        }
        else if (Input.IsActionJustReleased(action))
        {
            _pressedKeys.Remove(action);
        }
    }

    public Vector2 GetDirectionFromKeys()
    {
        // Determine direction based on the last key pressed in the list
        if (_pressedKeys.Count == 0)
            return Vector2.Zero;

        string lastKey = _pressedKeys[^1];  // Get the last element in the list
        return lastKey switch
        {
            "ui_left" => Vector2.Left,
            "ui_right" => Vector2.Right,
            "ui_up" => Vector2.Up,
            "ui_down" => Vector2.Down,
            _ => Vector2.Zero
        };
    }

    public FacingDirection GetFacingDirection(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.X) > Mathf.Abs(inputDirection.Y))
        {
            return inputDirection.X > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            return inputDirection.Y > 0 ? FacingDirection.Down : FacingDirection.Up;
        }
    }
}
