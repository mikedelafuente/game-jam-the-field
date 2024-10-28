using Godot;
using TheField.Common;

namespace TheField.Player;

public class IdleState : IFiniteState
{
    public const string Name = "Idle";

    public TheField.Player.Player Entity { get; init; }

    public string Key => Name;
    public FiniteStateMachine StateMachine { get; set; }

    public IdleState(TheField.Player.Player entity)
    {
        Entity = entity;
    }

    public void Enter(IFiniteState previous = null)
    {
        // Set animation to idle
        Entity.DustEmitter.Emitting = false;
        Entity.Velocity = Vector2.Zero;
        Entity.AnimationTree.Set($"parameters/{Name}/blend_position", Entity.LastFacingDirection);
        ((AnimationNodeStateMachinePlayback)Entity.AnimationTree.Get("parameters/playback")).Travel(Name);
    }

    public void Exit()
    {
        // No specific exit logic needed for Idle
    }

    public void PhysicsProcess(float delta)
    {
        Entity.UpdatePressedKeys();
        var inputDirection = Entity.GetDirectionFromKeys();

        if (inputDirection != Vector2.Zero) Entity.StateMachine.ChangeState(WalkState.Name);
    }

    public void Process(float delta)
    {
    }
}