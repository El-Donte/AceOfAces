using AceOfAces.Core;
using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class CooldownBarView : IView
{
    private readonly MissileCooldownModel _cooldown;
    private readonly Texture2D _cooldownTexture = AssetsManager.CooldownTexture;

    private readonly Vector2 _screenMargin;
    private readonly int _width;
    private readonly int _height;

    private readonly SpriteBatch _spriteBatch;

    public Camera Camera { get; set; }
    public GraphicsDevice GraphicsDevice { get; set; }

    public CooldownBarView(MissileCooldownModel cooldown,Vector2 screenMargin, SpriteBatch spriteBatch)
    {
        _cooldown = cooldown;
        _width = _cooldownTexture.Width;
        _height = _cooldownTexture.Height;
        _screenMargin = screenMargin;
        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        Vector2 screenPosition = new(
            GraphicsDevice.Viewport.Width - _width - _screenMargin.X,
            GraphicsDevice.Viewport.Height - _height - _screenMargin.Y
        );

        Matrix invertedMatrix = Matrix.Invert(Camera.TransformMatrix);
        screenPosition = Vector2.Transform(screenPosition, invertedMatrix);

        _spriteBatch.Draw(_cooldownTexture, screenPosition, null, Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None,0f);

        int fillHeight = (int)(_height * _cooldown.Progress);
        Rectangle sourceRect = new(
            0,
            _height - fillHeight, 
            _width,
            fillHeight
        );

        Vector2 fillPosition = new(
            screenPosition.X,
            screenPosition.Y + (_height - fillHeight)
        );

        _spriteBatch.Draw(_cooldownTexture, fillPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}
