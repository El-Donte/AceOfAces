using AceOfAces.Models;
using AceOfAces.Controllers;
using AceOfAces.Views;

namespace AceOfAces.Core.FSM;

public class GameOverState : BaseMenuState
{
    private readonly GameOverModel _model;
    private readonly GameOverView _view;
    private readonly GameOverController _controller;

    public GameOverState(StateMachine stateMachine)
        : base(stateMachine)
    {
        _model = new GameOverModel();
        _view = new GameOverView(_model, _spriteBatch);
        _controller = new GameOverController(_model);
    }

    public override void Enter()
    {
        _model.Menu = false;
        _model.PlayAgain = false;
    }

    public override void Update(float deltaTime)
    {
        _controller.Update(deltaTime);

        if (_model.PlayAgain)
        {
            StateMachine.Change("Game");
        }

        if(_model.Menu)
        {
            StateMachine.Change("Menu");
        }
    }

    public override void Draw()
    {
        DrawToRenderTarget(_view.Draw);
    }

    public override void Exit()
    {

    }
}

