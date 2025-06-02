using AceOfAces.Controllers;
using AceOfAces.Models;
using AceOfAces.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core.FSM;

public class MenuState : BaseState
{
    private SpriteBatch _spriteBatch;
    private readonly MenuModel _model;
    private readonly MenuView _view;
    private readonly MenuController _controller;


    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;

    public MenuState(StateMachine stateMachine) : base(stateMachine)
    {
        _nativeRenderTarget = new RenderTarget2D(StateMachine.GameEngine.GraphicsDevice, 320, 180);
        _actualScreenRectangle = new Rectangle(0, 0, 1920, 1080);

        _spriteBatch = new SpriteBatch(StateMachine.GameEngine.GraphicsDevice);
        _model = new MenuModel();
        _view = new MenuView(_model, _spriteBatch);
        _controller = new MenuController(_model);
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
        _model.Quit = false;
        _model.Play = false;
    }

    public override void Exit()
    {

    }

    public override void Update(float deltaTime)
    {
        _controller.Update(deltaTime);

        if (_model.Play)
        {
            StateMachine.Change("Game");
        }

        if (_model.Quit)
        {
            StateMachine.GameEngine.Exit();
        }
    }
}