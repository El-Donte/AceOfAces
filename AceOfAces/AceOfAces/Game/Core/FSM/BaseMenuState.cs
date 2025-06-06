using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Core.FSM;

public abstract class BaseMenuState : BaseState
{
    protected readonly SpriteBatch _spriteBatch;
    protected readonly RenderTarget2D _nativeRenderTarget;
    protected readonly Rectangle _actualScreenRectangle;

    protected static readonly int TargetWidth = 320;
    protected static readonly int TargetHeight = 180;

    protected static readonly int ScreenWidth = 1920;
    protected static readonly int ScreenHeight = 1080;

    protected static readonly Color ClearColor = Color.CornflowerBlue;

    public BaseMenuState(StateMachine stateMachine) : base(stateMachine)
    {
        var graphics = StateMachine.GameEngine.GraphicsDevice;
        _nativeRenderTarget = new RenderTarget2D(graphics, TargetWidth, TargetHeight);
        _actualScreenRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
        _spriteBatch = new SpriteBatch(graphics);
    }

    protected void DrawToRenderTarget(Action drawAction)
    {
        var graphics = StateMachine.GameEngine.GraphicsDevice;
        graphics.SetRenderTarget(_nativeRenderTarget);
        graphics.Clear(ClearColor);

        _spriteBatch.Begin();
        drawAction();
        _spriteBatch.End();

        graphics.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
        _spriteBatch.End();
    }
}

