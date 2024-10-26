using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public class IdleState : IFiniteState
{
    public const string Name = "Idle";
    
    public string Key => Name;
    
    public Player Entity { get; init; }
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

    public void Process(float delta) { }

    public void PhysicsProcess(float delta)
    {
        Entity.UpdatePressedKeys();
        Vector2 inputDirection = Entity.GetDirectionFromKeys();

        if (inputDirection != Vector2.Zero)
        {
            Entity.StateMachine.ChangeState(WalkState.Name);
        }
    }
}