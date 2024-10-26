﻿using Godot;
using TheField.Common;

namespace TheField.Characters.Player;

public class WalkState : IFiniteState
{
    public const string Name = "Walk";
    public TheField.Characters.Player.Player Entity { get; init; }
    public string Key => Name;
    public FiniteStateMachine StateMachine { get; set; }

    public WalkState(TheField.Characters.Player.Player entity)
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
            Entity.Velocity = inputDirection * TheField.Characters.Player.Player.Speed;

            // Update animation blend positions
            Entity.AnimationTree.Set($"parameters/{Name}/blend_position", inputDirection);
            Entity.MoveAndSlide();
        }
    }

    public void Process(float delta)
    {
    }
}