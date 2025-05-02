using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class EnemyModel : GameObjectModel, ITarget
{
    #region Health
    private int _health = 2;
    public int Health => _health; // Здоровье
    #endregion

    #region Rotation
    private readonly float _evasionAngle = MathHelper.ToRadians(60);
    public float EvasionAngle => _evasionAngle;

    private readonly float _rotationSpeed = 1.3f;
    public float RotationSpeed => _rotationSpeed;

    private float _rotation;
    public float Rotation 
    { 
        get => _rotation;
        set
        {
            _rotation = value;
            //_collider.UpdateBounds(GetBounds());
        }
    }
    #endregion

    #region Speed
    private float _currentSpeed = 400f;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = MathHelper.Clamp(_currentSpeed + value, MinSpeed, MaxSpeed);
    }

    private readonly float _acceleration = 100f;
    public float Acceleration => _acceleration;

    private readonly float _minSpeed = 400f;
    public float MinSpeed => _minSpeed;

    private readonly float _maxSpeed = 650f;
    public float MaxSpeed => _maxSpeed;

    private Vector2 _velocity = Vector2.Zero;
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    #endregion

    #region Target
    private readonly float _pursuitRadius = 400f;
    public float PursuitRadius => _pursuitRadius;

    private readonly float _targetUpdateTime = 2f;
    private float _targetTimer = 0f;
    public float TargetTimer
    {
        get => _targetTimer;
        set
        {
            if (_targetTimer >= _targetUpdateTime)
            {
                _targetTimer = 0;
            }
            else
            {
                _targetTimer = value;
            }
        }
    }

    private Vector2 _targetPosition;
    public Vector2 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }

    public bool IsPursuingPlayer { get; set; }
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

    public float NoTargetTimer { get; set; }
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

