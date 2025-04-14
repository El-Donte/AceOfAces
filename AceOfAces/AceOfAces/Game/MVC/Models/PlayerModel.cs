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
      
    public event Action<float> SpeedChanged;
    #endregion

    #region Invulnerability
    private float _invulnerabilityTimer;
    public bool IsInvulnerable => _invulnerabilityTimer > 0; // Находится ли игрок в состоянии неуязвимости

    public event Action<bool> OnInvulnerabilityEnded;
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

    #region Events
    public event Action<Vector2> PositionChanged;
    public event Action<Vector2> MovementDirectionChanged;
    #endregion

    public PlayerModel(Texture2D texture, Vector2 position) : base(texture, position)
    {
        _collider = new CollisionModel(GetBounds(),GameObjectType.Player);
    }

    public override void Move(Vector2 direction, float deltaTime)
    {
        _velocity = direction * _currentSpeed;
        _position +=  Velocity * deltaTime;

        _collider.UpdateBounds(GetBounds());

        PositionChanged?.Invoke(_position);
        MovementDirectionChanged?.Invoke(direction);
    }

    public override void Rotate(Vector2 inputDirection, float deltaTime)
    {
        _rotation += inputDirection.X * _rotationSpeed * deltaTime;
        RotationChanged?.Invoke(_rotation);
    }

    public void ChangeSpeed(int k, bool isBreaking, float deltaTime)
    {
        if (isBreaking)
        {
            _currentSpeed -= k * _decceleration * deltaTime;
        }
        else
        {
            _currentSpeed += k * _acceleration * deltaTime;
        }

        _currentSpeed = MathHelper.Clamp(_currentSpeed, _minSpeed, _maxSpeed);
        SpeedChanged?.Invoke(_currentSpeed);
    }

    public void Invulnerable(float deltaTime)
    {
        if (!IsInvulnerable)
        {
            return;
        }

        _invulnerabilityTimer -= deltaTime;
    }

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = MathHelper.Clamp(speed, _minSpeed, _maxSpeed);
        SpeedChanged?.Invoke(_currentSpeed);
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

