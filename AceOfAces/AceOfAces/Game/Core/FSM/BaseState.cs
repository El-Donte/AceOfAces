namespace AceOfAces.Core.FSM;

public abstract class BaseState : IState
{
    public StateMachine StateMachine { get; }

    public BaseState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public abstract void Update(float deltaTime);

    public abstract void Draw();

    public abstract void Enter(params object[] args);

    public abstract void Exit();
}
