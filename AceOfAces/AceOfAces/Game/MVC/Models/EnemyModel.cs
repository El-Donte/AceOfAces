using AceOfAces.Managers;
using AceOfAces.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class EnemyModel : GameObjectModel, ITarget
{
    private readonly Random _random = new Random();

    #region Health
    private readonly int _id;
    public int Id => _id;

    private int _health = 1;
    #endregion

    #region Rotation
    private readonly float _evasionradius = 300f;
    public float EvasionRadius => _evasionradius;

    private readonly float _rotationSpeed = 1.6f;
    public float RotationSpeed => _rotationSpeed;

    private float _rotation;
    public float Rotation 
    { 
        get => _rotation; 
        set => _rotation = value; 
    }
    #endregion

    #region Speed
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            _collider.UpdateBounds(_position, _rotation);
        }
    }

    private readonly float _evasionKoeff = 2f;
    public float EvasionKoeff => _evasionKoeff;

    private float _speed;
    public float Speed 
    { 
        get => _speed;
        set => _speed = Math.Clamp(value, _minSpeed, _maxSpeed); 
    }

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

    private bool _isEvading = false;
    public bool IsEvading 
    { 
        get => _isEvading; 
        set => _isEvading = value; 
    }

    private readonly float _fieldOfViewAngle = MathHelper.ToRadians(30);
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

    private readonly float _minFireDelay = 2.5f;
    private readonly float _maxFireDelay = 5f;

    private float _fireDelay;
    public float FireDelayTimer
    {
        get => _fireDelay;
        set
        {
            _fireDelay = value;
            if (_fireDelay <= 0)
            {
                _fireDelay = 0;
            }
        }
    }

    public bool CanFire => _fireDelay <= 0;
    #endregion

    public EnemyModel(int id, Vector2 position) : base(position)
    {
        _id = id;

        _collider = new ColliderModel(AssetsManager.EnemyTexture.Height, AssetsManager.EnemyTexture.Width, 5f, 0.8f);
        for (int i = 0; i < MaxMissileCount; i++)
        {
            _cooldowns.Add(new MissileCooldownModel(10f));
        }

        _speed = _minSpeed;
    }

    public float ResetRandomFireDelay()
    {
        return (float)(_random.NextDouble() * (_maxFireDelay - _minFireDelay) + _minFireDelay);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
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

