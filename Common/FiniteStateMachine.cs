using System.Collections.Generic;

namespace TheField.Common;

public class FiniteStateMachine
{
    protected readonly Dictionary<string, IFiniteState> States = new();
    public IFiniteState CurrentState { get; private set; }
    public string CurrentStateName { get; private set; }
    public string PreviousStateName { get; set; }

    public void Add(IFiniteState state)
    {
        States[state.Key] = state;
        state.StateMachine = this;
    }

    public void ChangeState(string newState, IFiniteState previous = null)
    {
        CurrentState.Exit();
        CurrentState = States[newState];
        CurrentStateName = newState;
        CurrentState.Enter(previous);
    }

    public void ExecuteProcess(float delta)
    {
        CurrentState.Process(delta);
    }

    public void ExecuteStatePhysics(float delta)
    {
        CurrentState.PhysicsProcess(delta);
    }

    public void InitialiseState(string newState)
    {
        CurrentState = States[newState];
        CurrentStateName = newState;
        CurrentState.Enter();
    }
}