using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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

    public List<MissileCooldownModel> Cooldowns { get; } = [];

    public GameObjectType Type => GameObjectType.Player;

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

        for (int i = 0; i < MaxMissileCount; i++)
        {
            Cooldowns.Add(new MissileCooldownModel(3f));
        }
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

    protected override Rectangle GetBounds()
    {
        // Получаем исходные размеры и позицию
        int width = (int)(_texture.Width / 1.5f);
        int height = (int)(_texture.Height / 1.5f);
        Vector2 center = new(
            _position.X - (_texture.Width / 3.2f) + width / 2,
            _position.Y - (_texture.Height / 3.2f) + height / 2
        );

        // Если угол поворота равен нулю, возвращаем обычный прямоугольник
        if (_rotation == 0)
            return new Rectangle(
                (int)(center.X - width / 2),
                (int)(center.Y - height / 2),
                width,
                height
            );

        // Вычисляем повёрнутые углы прямоугольника
        float cos = MathF.Cos(_rotation);
        float sin = MathF.Sin(_rotation);

        Vector2[] corners = [
             new (-width / 2, -height / 2),
            new (width / 2, -height / 2),
            new (width / 2, height / 2),
            new (-width / 2, height / 2),
        ];

        // Поворачиваем углы и находим границы
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (Vector2 corner in corners)
        {
            Vector2 rotated = new Vector2(
                corner.X * cos - corner.Y * sin,
                corner.X * sin + corner.Y * cos
            ) + center;

            minX = Math.Min(minX, rotated.X);
            maxX = Math.Max(maxX, rotated.X);
            minY = Math.Min(minY, rotated.Y);
            maxY = Math.Max(maxY, rotated.Y);
        }

        // Создаём новый прямоугольник, содержащий повёрнутый коллайдер
        return new Rectangle(
            (int)minX,
            (int)minY,
            (int)(maxX - minX),
            (int)(maxY - minY)
        );
    }
}

