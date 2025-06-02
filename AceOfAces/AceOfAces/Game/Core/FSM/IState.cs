namespace AceOfAces.Core.FSM;

public interface IState
{
    void Update(float deltaTime);

    void Draw();

    void Enter(params object[] args);

    void Exit();
}

