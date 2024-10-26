namespace TheField.Common;

public interface IFiniteState
{
    /// <summary>
    /// The Key is the Name 
    /// </summary>
    public string Key { get; }
    FiniteStateMachine StateMachine { get; set; }
    public void Enter(IFiniteState previous = null);
    public void Exit();
    public void Process(float delta);
    public void PhysicsProcess(float delta);

}