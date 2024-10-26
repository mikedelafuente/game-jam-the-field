using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public class IdleState : IFiniteState
{
    public const string Name = "Idle";

    public Player Entity { get; init; }

    public string Key => Name;
    public FiniteStateMachine StateMachine { get; set; }

    public IdleState(Player entity)
    {
        Entity = entity;
    }

    public void Enter(IFiniteState previous = null)
    {
        // Set animation to idle
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