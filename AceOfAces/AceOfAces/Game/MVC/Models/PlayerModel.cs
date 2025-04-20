using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class PlayerModel : GameObjectModel, ITarget
{
    #region Health
    private int _health = 100;
    public int Health => _health; // Здоровье

    public event Action<bool> OnDamaged;
    #endregion

    #region Rotation
    private readonly float _rotationSpeed = 4f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation => _rotation; // Угол поворота

    //public event Action<float> RotationChanged;
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

    #region Invulnerability
    private float _invulnerabilityTimer;

    public float InvulnerabilityTimer
    {
        get => _invulnerabilityTimer;
        set
        {
            _invulnerabilityTimer = value;
        }
    }

    public bool IsInvulnerable => _invulnerabilityTimer > 0; // Находится ли игрок в состоянии неуязвимости
    #endregion

    #region Blink
    public event Action<float> OnBlinkPhaseChanged;

    private float _blinkPhase = 0f;
    private int _firedMissileCount;

    public float BlinkPhase
    {
        get => _blinkPhase;
        set
        {
            _blinkPhase = value;
            OnBlinkPhaseChanged?.Invoke(_blinkPhase);
        }
    }
    #endregion

    #region Missile
    private readonly Vector2 PointLocalOffset = new(30, 0);

    public int MaxMissileCount => 2;

    public Vector2 MissileJointPosition => GetMissileJointPosition();

    public int FiredMissileCount
    {
        get => _firedMissileCount;
        set
        {
            if(_firedMissileCount == MaxMissileCount)
            {
                _firedMissileCount = 0;
            }
            _firedMissileCount = value;
        }
    }
    #endregion

    public event Action<Vector2> PositionChangedEvent;

    public PlayerModel(Texture2D texture, Vector2 position) : base(texture, position)
    {
        _collider = new ColliderModel(GetBounds(),GameObjectType.Player);
    }
    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
        PositionChangedEvent?.Invoke(_position);
    }

    public void SetRoration(float rotation)
    {
        _rotation += rotation;
    }

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable)
        {
            return;
        }

        _health -= damage;

        _invulnerabilityTimer = 1.5f;

        OnDamaged?.Invoke(IsInvulnerable);
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

