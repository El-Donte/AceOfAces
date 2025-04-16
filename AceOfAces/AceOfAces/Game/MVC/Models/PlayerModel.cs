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

    public event Action<float> RotationChanged;
    #endregion

    #region Speed
    private float _currentSpeed = 60f;
    public float CurrentSpeed => _currentSpeed; // Текущая скорость

    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    private readonly float _minSpeed = 60f;
    private readonly float _maxSpeed = 500f;
    private readonly float _acceleration = 400f;
    private readonly float _decceleration = 100f;

    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public float Decceleration => _decceleration;
    public float Acceleration => _acceleration;
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

    public PlayerModel(Texture2D texture, Vector2 position) : base(texture, position)
    {
        _collider = new CollisionModel(GetBounds(),GameObjectType.Player);
    }
    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
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
}

