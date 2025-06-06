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
    private readonly Viewport _viewport;

    private readonly int _width;
    private readonly int _height;

    private readonly SpriteBatch _spriteBatch;
    private readonly Camera _camera;

    private Vector2 _screenPosition;

    public CooldownBarView(MissileCooldownModel cooldown,Vector2 screenMargin, SpriteBatch spriteBatch, Viewport viewport, Camera camera)
    {
        _cooldown = cooldown;
        _width = _cooldownTexture.Width;
        _height = _cooldownTexture.Height;
        _spriteBatch = spriteBatch;

        _screenMargin = screenMargin;

        _camera = camera;
        _viewport = viewport;
    }

    public void Draw()
    {
        _screenPosition = new(
            _viewport.Width - _width - _screenMargin.X,
            _viewport.Height - _height - _screenMargin.Y
        );

        Matrix invertedMatrix = Matrix.Invert(_camera.TransformMatrix);
        _screenPosition = Vector2.Transform(_screenPosition, invertedMatrix);

        _spriteBatch.Draw(_cooldownTexture, _screenPosition, null, Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None,0f);

        int fillHeight = (int)(_height * _cooldown.Progress);
        Rectangle sourceRect = new(
            0,
            _height - fillHeight, 
            _width,
            fillHeight
        );

        Vector2 fillPosition = new(
            _screenPosition.X,
            _screenPosition.Y + (_height - fillHeight)
        );

        _spriteBatch.Draw(_cooldownTexture, fillPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}
