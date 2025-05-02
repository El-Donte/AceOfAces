using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class PlayerModel : GameObjectModel, ITarget
{
    #region Health
    public event Action<bool> OnDamagedEvent;

    private int _health = 4;
    public int Health => _health; // Здоровье
    #endregion

    #region Rotation
    //public event Action<float> RotationChanged;

    private readonly float _rotationSpeed = 5f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
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

    private readonly float _minSpeed = 100f;
    public float MinSpeed => _minSpeed;

    private readonly float _maxSpeed = 300f;
    public float MaxSpeed => _maxSpeed;

    private readonly float _acceleration = 200f;
    public float Acceleration => _acceleration;

    private readonly float _decceleration = 150f;
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
    public event Action<float> OnBlinkPhaseChangedEvent;

    private float _blinkPhase = 0f;

    public float BlinkPhase
    {
        get => _blinkPhase;
        set
        {
            _blinkPhase = value;
            OnBlinkPhaseChangedEvent?.Invoke(_blinkPhase);
        }
    }
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
        _collider = new ColliderModel(GetBounds());
    }

    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
        PositionChangedEvent?.Invoke(_position);
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable)
        {
            return;
        }

        _health -= damage;

        _invulnerabilityTimer = 1.5f;

        OnDamagedEvent?.Invoke(IsInvulnerable);
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

