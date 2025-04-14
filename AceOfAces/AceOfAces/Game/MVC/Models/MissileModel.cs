using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace AceOfAces.Models;

public class MissileModel : GameObjectModel
{
    private float _lifespan = 5f;
    public readonly float ArrivalTreshold = 100f;
    public readonly float PredictedTime = 0.2f;

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


    public MissileModel(Texture2D texture, Vector2 position) : base(texture, position)
    {
        _collider = new CollisionModel(GetBounds(), GameObjectType.Missile);
    }

    public override void Move(Vector2 direction, float deltaTime)
    {
        SetVelocity(direction, deltaTime);
        _position += _velocity * deltaTime;

        _collider.UpdateBounds(GetBounds());
    }

    public override void Rotate(Vector2 inputDirection, float deltaTime)
    {
        if (_velocity.LengthSquared() > 0.1f)
        {
            _rotation = (float)Math.Atan2(_velocity.Y, _velocity.X);
        }
    }

    private void SetVelocity(Vector2 direction, float deltaTime)
    {
        _velocity += direction * deltaTime;
        if (_velocity.Length() > _speed)
        {
            _velocity = Vector2.Normalize(_velocity) * _speed;
        }
    }

    public void UpdateLifeSpan(float deltaTime)
    {
        _lifespan -= deltaTime;
        if (_lifespan < 0f)
        {
            Dispose();
        }
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }
}

