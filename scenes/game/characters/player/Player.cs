using System.Collections.Generic;
using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public partial class Player : CharacterBody2D
{
    enum PlayerState
    {
        Idle,
        Walk
    }

    enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private AnimationTree _animationTree;
    private AnimationPlayer _animationPlayer;
    private Vector2 _lastMoveDirection = Vector2.Zero;
    private FacingDirection _currentDirection = FacingDirection.Down;
    PlayerState _currentState = PlayerState.Idle;
    private FiniteStateMachine _fsm = new();
    public const float Speed = 32.0f;
    private List<string> _pressedKeys = new();


    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>("Sprite2D/AnimationPlayer/AnimationTree");
        _animationTree.Active = true;
        _animationPlayer = GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer");
        _fsm.Add("Idle", new IdleState(this));
        _fsm.Add("Walk", new WalkState(this));
    }
    
    public override void _PhysicsProcess(double delta)
    {
        UpdatePressedKeys();

        // Determine the latest direction pressed
        Vector2 inputDirection = GetDirectionFromKeys();
        
        if (inputDirection != Vector2.Zero)
        {
            _currentState = PlayerState.Walk;
            _currentDirection = GetFacingDirection(inputDirection);
            _lastMoveDirection = inputDirection;
        }
        else
        {
            _currentState = PlayerState.Idle;
        }

     
        // Update AnimationTree parameters
        ((AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback")).Travel(_currentState.ToString());
        _animationTree.Set("parameters/Idle/blend_position", _lastMoveDirection);
        _animationTree.Set("parameters/Walk/blend_position", inputDirection);

        // Handle movement
        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }

    private void UpdatePressedKeys()
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

    private Vector2 GetDirectionFromKeys()
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

    private FacingDirection GetFacingDirection(Vector2 inputDirection)
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