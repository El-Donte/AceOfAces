using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class EnemyModel : GameObjectModel, ITarget
{
    private int _health = 100;

    #region Rotation
    //public event Action<float> RotationChanged;

    private readonly float _rotationSpeed = 8f;
    public float RotationSpeed => _rotationSpeed; // Скорость поворота

    protected float _rotation;
    public float Rotation => _rotation; // Угол поворота
    #endregion

    #region Speed
    private float _currentSpeed = 60f;
    public float CurrentSpeed => _currentSpeed; // Текущая скорость

    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    private readonly float _minSpeed = 60f;
    public float MinSpeed => _minSpeed;

    private readonly float _maxSpeed = 500f;
    public float MaxSpeed => _maxSpeed;

    private readonly float _acceleration = 400f;
    public float Acceleration => _acceleration;

    private readonly float _decceleration = 100f;
    public float Decceleration => _decceleration;
    #endregion

    #region Missile
    private readonly Vector2 PointLocalOffset = new(30, 0);

    public int MaxMissileCount => 2;

    public Vector2 MissileJointPosition => GetMissileJointPosition();

    private int _firedMissileCount;
    public int FiredMissileCount
    {
        get => _firedMissileCount;
        set
        {
            if (_firedMissileCount == MaxMissileCount)
            {
                _firedMissileCount = 0;
            }
            _firedMissileCount = value;
        }
    }
    #endregion

    public EnemyModel(Texture2D texture, Vector2 position) : base(texture,position)
    {
        _collider = new ColliderModel(GetBounds());
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

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity += velocity;
        if (_velocity.Length() > _maxSpeed)
        {
            _velocity = Vector2.Normalize(_velocity) * _maxSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }

    private Vector2 GetMissileJointPosition()
    {
        Vector2 rotatedOffset = new(
            PointLocalOffset.X * (float)Math.Cos(Rotation) - PointLocalOffset.Y * (float)Math.Sin(Rotation),
            PointLocalOffset.X * (float)Math.Sin(Rotation) + PointLocalOffset.Y * (float)Math.Cos(Rotation)
        );

        var offset = FiredMissileCount % 2 == 0 ? -rotatedOffset : rotatedOffset;
        return offset;
    }
}

