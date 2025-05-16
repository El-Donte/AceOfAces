using AceOfAces.Managers;
using AceOfAces.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class EnemyModel : GameObjectModel, ITarget
{
    #region Health
    private int _health = 2;
    public int Health => _health;
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
        }
    }
    #endregion

    #region Speed
    private readonly float _evasionKoeff = 1.2f;

    private float _currentSpeed = 400f;
    public float CurrentSpeed => _currentSpeed;

    private readonly float _acceleration = 100f;
    public float Acceleration => _acceleration;

    private readonly float _minSpeed = 450f;
    private readonly float _maxSpeed = 600f;

    private Vector2 _velocity = Vector2.Zero;
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    #endregion

    #region Target
    private bool _isTargeted = false;
    public bool IsTargeted
    {
        get => _isTargeted;
        set => _isTargeted = value;
    }

    private readonly float _pursuitRadius = 400f;
    public float PursuitRadius => _pursuitRadius;

    private Vector2 _targetPosition;
    public Vector2 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }

    private bool _isPursuingPlayer = false;
    public bool IsPursuingPlayer
    {
        get => _isPursuingPlayer;
        set => _isPursuingPlayer = value;
    }

    private readonly float _fieldOfViewAngle = MathHelper.ToRadians(60);
    public float FieldOfViewAngle => _fieldOfViewAngle;

    public GameObjectType Type => GameObjectType.Enemy;
    #endregion

    #region Missile
    private readonly List<MissileCooldownModel> _cooldowns = [];
    public List<MissileCooldownModel> Cooldowns => _cooldowns;

    private readonly Vector2 _pointLocalOffset = new(0, 30);
    public Vector2 MissileJointPosition => GetMissileJointPosition();

    public int MaxMissileCount => 2;

    private int _firedMissileCount;
    public int FiredMissileCount
    {
        get => _firedMissileCount;
        set
        {
            _firedMissileCount = value;
            if (_firedMissileCount == MaxMissileCount)
            {
                _firedMissileCount = 0;
            }
        }
    }

    private float _fireDelay = 2f;
    public float FireDelayTimer
    {
        get => _fireDelay;
        set
        {
            if(_fireDelay <= 0)
            {
                _fireDelay = 0;
            }
            _fireDelay = value;
        }
    }

    public bool CanFire => _fireDelay <= 0;
    #endregion

    public EnemyModel(Vector2 position) : base(position)
    {
        _collider = new ColliderModel(AssetsManager.EnemyTexture.Height, AssetsManager.EnemyTexture.Width, 5f, 0.8f);
        for (int i = 0; i < MaxMissileCount; i++)
        {
            _cooldowns.Add(new MissileCooldownModel(6f));
        }
    }

    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(_position,_rotation);
    }

    public void SetCurrentSpeed(float speed, bool isEvaiding)
    {
        var koeff = isEvaiding ? _evasionKoeff : 1;
        _currentSpeed = Math.Clamp(_currentSpeed + speed * koeff, _minSpeed, _maxSpeed * koeff);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            IsTargeted = false;
            Dispose();
        }
    }

    private Vector2 GetMissileJointPosition()
    {
        Vector2 rotatedOffset = new(
            _pointLocalOffset.X * (float)Math.Cos(_rotation) - _pointLocalOffset.Y * (float)Math.Sin(_rotation),
            _pointLocalOffset.X * (float)Math.Sin(_rotation) + _pointLocalOffset.Y * (float)Math.Cos(_rotation)
        );

        var offset = _firedMissileCount % 2 == 0 ? -rotatedOffset : rotatedOffset;
        return offset;
    }
}

