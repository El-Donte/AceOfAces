using AceOfAces.Models;
using AceOfAces.Controllers;
using AceOfAces.Views;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core.FSM;

public class GameOverState : BaseState
{
    private SpriteBatch _spriteBatch;
    private readonly GameOverModel _model;
    private readonly GameOverView _view;
    private readonly GameOverController _controller;

    public GameOverState(StateMachine stateMachine)
        : base(stateMachine)
    {
        _spriteBatch = new SpriteBatch(StateMachine.GameEngine.GraphicsDevice);
        _model = new GameOverModel();
        _view = new GameOverView(_model, _spriteBatch);
        _controller = new GameOverController(_model);
    }

    public override void Draw()
    {
        _spriteBatch.Begin();
        _view.Draw();
        _spriteBatch.End();
    }

    public override void Enter(params object[] args)
    {
        _model.Menu = false;
        _model.PlayAgain = false;
    }

    public override void Exit()
    {

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
}

