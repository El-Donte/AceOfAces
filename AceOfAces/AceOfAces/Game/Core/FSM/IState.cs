namespace AceOfAces.Core.FSM;

public interface IState
{
    void Enter();

    void Update(float deltaTime);

    void Draw();

    void Exit();
}

