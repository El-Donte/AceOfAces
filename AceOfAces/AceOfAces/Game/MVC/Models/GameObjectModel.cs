using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Models;

public class GameObjectModel : IDisposable
{
    #region Collisions
    protected ColliderModel _collider;
    public ColliderModel Collider => _collider;
    #endregion

    #region Destroyed
    private bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;

    public event Action<GameObjectModel> DestroyedEvent;
    #endregion

    #region Position
    protected Vector2 _position = Vector2.Zero;
    public Vector2 Position => _position;
    #endregion

    public GameObjectModel(Vector2 position)
    {
        _position = position;
    }

    public void Dispose()
    {
        if (IsDestroyed) return;

        _isDestroyed = true;

        DestroyedEvent?.Invoke(this);

        DestroyedEvent = null;
    }
}

