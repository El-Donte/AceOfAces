using AceOfAces.Core;
using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class PlayerModel : GameObjectModel, ITarget
{
    #region Health
    private int _health = 6;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
            {
                PlayerDeadEvent.Invoke();
            }
        }
    }
    public event Action PlayerDeadEvent;

    public event Action<bool> OnDamagedEvent;
    #endregion

    #region Rotation
    private readonly float _rotationSpeed = 5f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    #endregion

    #region Position
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            _collider.UpdateBounds(_position, _rotation);
            PositionChangedEvent?.Invoke(_position);
        }
    }

    public event Action<Vector2> PositionChangedEvent;
    #endregion

    #region Speed
    private float _currentSpeed = 150f;
    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed =  MathHelper.Clamp(value, _minSpeed, _maxSpeed);
    }

    private Vector2 _velocity;
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private readonly float _acceleration = 200f;
    public float Acceleration => _acceleration;

    private readonly float _decceleration = 150f;
    public float Decceleration => _decceleration;

    private readonly float _minSpeed = 100f;
    private readonly float _maxSpeed = 300f;
    #endregion

    #region Invulnerability
    private float _invulnerabilityTimer;

    public float InvulnerabilityTimer
    {
        get => _invulnerabilityTimer;
        set
        {
            _invulnerabilityTimer = value;
            if (_invulnerabilityTimer <= 0) 
            { 
                _collider.IsEnable = true;
            }

        }
    }

    public bool IsInvulnerable => _invulnerabilityTimer > 0;
    #endregion

    #region Blink
    private float _blinkPhase = 0f;

    public float BlinkPhase
    {
        get => _blinkPhase;
        set
        {
            _blinkPhase = value;
            BlinkPhaseChangedEvent?.Invoke(_blinkPhase);
        }
    }

    public event Action<float> BlinkPhaseChangedEvent;
    #endregion

    #region Missile
    private int _targetIndex = 0;
    public int TargetIndex
    {
        get => _targetIndex;
        set => _targetIndex = value;
    } 

    private readonly Vector2 _pointLocalOffset = new(30, 0);
    public Vector2 MissileJointPosition => GetMissileJointPosition();

    public List<MissileCooldownModel> Cooldowns { get; } = [];

    public GameObjectType Type => GameObjectType.Player;

    private readonly int _maxMissileCount = 2;
    public int MaxMissileCount => _maxMissileCount;

    private int _firedMissileCount;
    public int FiredMissileCount
    {
        get => _firedMissileCount;
        set
        {
            if(_firedMissileCount == _maxMissileCount)
            {
                _firedMissileCount = 0;
            }
            _firedMissileCount = value;
        }
    }
    #endregion

    public PlayerModel(Vector2 position) : base(position)
    {
        _collider = new ColliderModel(AssetsManager.PlayerTexture.Width, AssetsManager.PlayerTexture.Height, 5f, 0.5f);

        for (int i = 0; i < MaxMissileCount; i++)
        {
            Cooldowns.Add(new MissileCooldownModel(3f));
        }
    }

    public void SetTargetIndex(int index)
    { 
        _targetIndex = index; 
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable)
        {
            return;
        }

        Health -= damage;

        _collider.IsEnable = false;

        _invulnerabilityTimer = 1.5f;

        OnDamagedEvent?.Invoke(IsInvulnerable);
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

