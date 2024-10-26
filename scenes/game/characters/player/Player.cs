using Godot;
using System;
using TheField.Common;

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

    public const float Speed = 32.0f;

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>("Sprite2D/AnimationPlayer/AnimationTree");
        _animationTree.Active = true;
        _animationPlayer = GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();

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

        if (_currentDirection is FacingDirection.Up or FacingDirection.Down)
        {
            inputDirection = new Vector2(0, inputDirection.Y);
        }
        else if (_currentDirection is FacingDirection.Left or FacingDirection.Right)
        {
            inputDirection = new Vector2(inputDirection.X, 0);
        }
        // Update AnimationTree parameters
        ((AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback")).Travel(_currentState.ToString());
        _animationTree.Set("parameters/Idle/blend_position", _lastMoveDirection);
        _animationTree.Set("parameters/Walk/blend_position", inputDirection);

        // Handle movement
        Velocity = inputDirection * Speed;
        MoveAndSlide();
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

    public FiniteStateMachine StateMachine { get; set; }
    public void Enter(IFiniteState previous = null)
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }
    
}