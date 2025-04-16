using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Models;

public class MissileModel : GameObjectModel
{
    public static Texture2D MissleTexture { get; set; }

    public readonly float ArrivalTreshold = 100f;
    public readonly float PredictedTime = 0.2f;
    private float _lifespan = 5f;

    #region Speed
    private float _speed = 800f;
    public float Speed => _speed;

    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;
    #endregion

    #region Rotation
    private float _rotationSpeed = 8f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation => _rotation; // Угол поворота
    #endregion

    #region Damage
    private readonly int _damage = 50;
    public int Damage => _damage;
    #endregion

    #region Target
    private ITarget _target;
    public ITarget Target => _target;

    private GameObjectType _source;
    public GameObjectType Source => GameObjectType.Player;
    #endregion

    public MissileModel(Vector2 position) : base(MissleTexture, position)
    {
        _collider = new CollisionModel(GetBounds(), GameObjectType.Missile);
    }

    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
    }

    public void SetRotation(float rotation)
    {
        _rotation = rotation;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity += velocity;
        if (_velocity.Length() > _speed)
        {
            _velocity = Vector2.Normalize(_velocity) * _speed;
        }
    }

    public void ReduceLifespan(float deltaTime)
    {
        _lifespan -= deltaTime;
        if (_lifespan <= 0)
        {
            Dispose();
        }
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }
}

