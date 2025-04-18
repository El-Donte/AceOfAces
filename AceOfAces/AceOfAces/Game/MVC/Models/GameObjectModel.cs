using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public abstract class GameObjectModel : IDisposable
{
    #region Texture
    protected readonly Texture2D _texture;
    public Texture2D Texture => _texture; //текстура
    #endregion

    #region Collisions
    protected ColliderModel _collider;
    public ColliderModel Collider => _collider;
    #endregion

    #region Destroyed
    private bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;
    public event Action<GameObjectModel> Destroyed;
    #endregion

    #region Position
    protected Vector2 _position = Vector2.Zero;
    public Vector2 Position => _position;
    #endregion

    #region Origin
    private Vector2 _origin;
    public Vector2 Origin => _origin;
    #endregion

    public GameObjectModel(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
        _origin = new Vector2(texture.Width / 2, texture.Height / 2 + 10);
    }

    public void Dispose()
    {
        if (IsDestroyed) return;

        _isDestroyed = true;

        Destroyed?.Invoke(this);

        Destroyed = null;
    }

    protected Rectangle GetBounds()
    {
        return new Rectangle(
            (int)_position.X - _texture.Width / 4,
            (int)_position.Y - _texture.Height / 4,
            _texture.Width / 2,
            _texture.Height / 2);
    }
}

