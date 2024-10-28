using Godot;
using TheField.Common;

namespace TheField.Player;

public class WalkState : IFiniteState
{
    public const string Name = "Walk";
    public TheField.Player.Player Entity { get; init; }
    public string Key => Name;
    public FiniteStateMachine StateMachine { get; set; }

    public WalkState(TheField.Player.Player entity)
    {
        Entity = entity;
    }

    public void Enter(IFiniteState previous = null)
    {
        // Set animation to walk
        ((AnimationNodeStateMachinePlayback)Entity.AnimationTree.Get("parameters/playback")).Travel(Name);
        Entity.DustEmitter.Emitting = true;
    }

    public void Exit()
    {
        // Clear velocity when exiting Walk state
        //Entity.Velocity = Vector2.Zero;
    }

    public void PhysicsProcess(float delta)
    {
        Entity.UpdatePressedKeys();
        var inputDirection = Entity.GetDirectionFromKeys();

        if (inputDirection == Vector2.Zero)
        {
            Entity.StateMachine.ChangeState(IdleState.Name);
        }
        else
        {
            // Update facing direction and movement
            Entity.CurrentFacingDirection = Entity.GetFacingDirection(inputDirection);
            Entity.Velocity = inputDirection * TheField.Player.Player.Speed;

            if (Entity.DustEmitter.ProcessMaterial is ParticleProcessMaterial particleProcessMaterial)
            {
                var gravity = Entity.CurrentFacingDirection * 5;
                particleProcessMaterial.Gravity = new Vector3(gravity.X, gravity.Y, 0);
                
                // -2. -6
                Vector2 emitPoint = Vector2.Zero;
                
                if (Entity.CurrentFacingDirection == Vector2.Left)
                { 
                    emitPoint = new Vector2(6f, -2f);
                } else if (Entity.CurrentFacingDirection == Vector2.Right)
                { 
                    emitPoint = new Vector2(-6f, -2f);
                } else if (Entity.CurrentFacingDirection == Vector2.Down)
                { 
                    emitPoint = new Vector2(0f, -10f);
                }else if (Entity.CurrentFacingDirection == Vector2.Up)
                { 
                    emitPoint = new Vector2(0f, -2f);
                }
                Entity.DustEmitter.SetPosition(new Vector2(emitPoint.X, emitPoint.Y));

            }
            
            // Update animation blend positions
            Entity.AnimationTree.Set($"parameters/{Name}/blend_position", inputDirection);
            Entity.MoveAndSlide();
        }
    }

    public void Process(float delta)
    {
    }
}