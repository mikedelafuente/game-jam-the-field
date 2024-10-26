using TheField.Common;

namespace TheField.Scenes.Game.Characters.Player;

public class WalkState : IFiniteState
{
    public FiniteStateMachine StateMachine { get; set; }
    public Player Entity { get; init; }

    public WalkState(Player entity)
    {
        Entity = entity;
    }
    public void Enter(IFiniteState previous = null)
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Process(float delta)
    {
        throw new System.NotImplementedException();
    }

    public void PhysicsProcess(float delta)
    {
        throw new System.NotImplementedException();
    }
}