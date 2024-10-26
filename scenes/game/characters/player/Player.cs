using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private AnimationTree animationTree;
    private AnimationPlayer animationPlayer;
    private Vector2 _lastMoveDirection = Vector2.Zero;

    public enum PlayerState
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

    [Export] public string CurrentPlayerState = "walk";

    public PlayerState State = PlayerState.Idle;
    private FacingDirection currentDirection = FacingDirection.Down;
    public const float Speed = 32.0f;

    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>("Sprite2D/AnimationPlayer/AnimationTree");
        animationTree.Active = true;
        animationPlayer = GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();

        if (inputDirection != Vector2.Zero)
        {
            State = PlayerState.Walk;
            currentDirection = GetFacingDirection(inputDirection);
            _lastMoveDirection = inputDirection;
        }
        else
        {
            State = PlayerState.Idle;
        }

        if (currentDirection is FacingDirection.Up or FacingDirection.Down)
        {
            inputDirection = new Vector2(0, inputDirection.Y);
        }
        else if (currentDirection is FacingDirection.Left or FacingDirection.Right)
        {
            inputDirection = new Vector2(inputDirection.X, 0);
        }
        // Update AnimationTree parameters

        ((AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback")).Travel(State.ToString());
        animationTree.Set("parameters/Idle/blend_position", _lastMoveDirection);
        animationTree.Set("parameters/Walk/blend_position", inputDirection);

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
}