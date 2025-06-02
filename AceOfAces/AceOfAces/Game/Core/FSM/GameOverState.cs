using AceOfAces.Models;
using AceOfAces.Controllers;
using AceOfAces.Views;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AceOfAces.Core.FSM;

public class GameOverState : BaseState
{
    private SpriteBatch _spriteBatch;
    private readonly GameOverModel _model;
    private readonly GameOverView _view;
    private readonly GameOverController _controller;


    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;

    public GameOverState(StateMachine stateMachine)
        : base(stateMachine)
    {
        _nativeRenderTarget = new RenderTarget2D(StateMachine.GameEngine.GraphicsDevice, 320, 180);
        _actualScreenRectangle = new Rectangle(0, 0, 1920, 1080);

        _spriteBatch = new SpriteBatch(StateMachine.GameEngine.GraphicsDevice);
        _model = new GameOverModel();
        _view = new GameOverView(_model, _spriteBatch);
        _controller = new GameOverController(_model);
    }

    public override void Draw()
    {
        var graphics = StateMachine.GameEngine.GraphicsDevice;
        graphics.SetRenderTarget(_nativeRenderTarget);
        graphics.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _view.Draw();
        _spriteBatch.End();

        graphics.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
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

