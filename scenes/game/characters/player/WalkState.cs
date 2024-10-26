using Godot;
using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public class WalkState : IFiniteState
{
    public const string Name = "Walk";
    public string Key => Name;
    public FiniteStateMachine StateMachine { get; set; }
    public Player Entity { get; init; }

    public WalkState(Player entity)
    {
        Entity = entity;
    }

    public void Enter(IFiniteState previous = null)
    {
        // Set animation to walk
        ((AnimationNodeStateMachinePlayback)Entity.AnimationTree.Get("parameters/playback")).Travel(Name);
    }

    public void Exit()
    {
        // Clear velocity when exiting Walk state
        //Entity.Velocity = Vector2.Zero;
    }

    public void Process(float delta) { }

    public void PhysicsProcess(float delta)
    {
        Entity.UpdatePressedKeys();
        Vector2 inputDirection = Entity.GetDirectionFromKeys();

        if (inputDirection == Vector2.Zero)
        {
            Entity.StateMachine.ChangeState(IdleState.Name);
        }
        else
        {
            // Update facing direction and movement
            Entity.CurrentFacingDirection = Entity.GetFacingDirection(inputDirection); 
            Entity.Velocity = inputDirection * Player.Speed;

            // Update animation blend positions
            Entity.AnimationTree.Set($"parameters/{Name}/blend_position", inputDirection);
            Entity.MoveAndSlide();
        }
    }
}