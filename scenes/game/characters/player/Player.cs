using System.Collections.Generic;
using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public partial class Player : CharacterBody2D
{
    public const float Speed = 32.0f;

    private readonly List<string> _pressedKeys = new();
    private Vector2 _currentFacingDirection = Vector2.Down;

    internal AnimationTree AnimationTree;

    internal Vector2 LastFacingDirection;

    internal Vector2 CurrentFacingDirection
    {
        get => _currentFacingDirection;
        set
        {
            LastFacingDirection = _currentFacingDirection;
            _currentFacingDirection = value;
        }
    }

    public FiniteStateMachine StateMachine { get; } = new();

    public Player()
    {
        LastFacingDirection = Vector2.Down;
        CurrentFacingDirection = Vector2.Down;
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.ExecuteStatePhysics((float)delta);
    }

    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("Sprite2D/AnimationPlayer/AnimationTree");
        AnimationTree.Active = true;

        // Add states to the FSM and initialize the starting state
        StateMachine.Add(new IdleState(this));
        StateMachine.Add(new WalkState(this));
        StateMachine.InitialiseState(IdleState.Name); // Start in Idle state
    }

    public Vector2 GetDirectionFromKeys()
    {
        // Determine direction based on the last key pressed in the list
        if (_pressedKeys.Count == 0)
            return Vector2.Zero;

        var lastKey = _pressedKeys[^1]; // Get the last element in the list
        return lastKey switch
        {
            "ui_left" => Vector2.Left,
            "ui_right" => Vector2.Right,
            "ui_up" => Vector2.Up,
            "ui_down" => Vector2.Down,
            _ => Vector2.Zero
        };
    }

    public Vector2 GetFacingDirection(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.X) > Mathf.Abs(inputDirection.Y))
            return inputDirection.X > 0 ? Vector2.Right : Vector2.Left;
        return inputDirection.Y > 0 ? Vector2.Down : Vector2.Up;
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

    public void UpdatePressedKeys()
    {
        // Track individual key presses and their order
        TrackKey("ui_left");
        TrackKey("ui_right");
        TrackKey("ui_up");
        TrackKey("ui_down");
    }
}