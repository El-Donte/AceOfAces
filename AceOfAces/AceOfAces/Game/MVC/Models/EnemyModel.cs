using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public float MinSpeed => _minSpeed;

    private readonly float _maxSpeed = 600f;
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

    private Vector2 _targetPosition;
    public Vector2 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }

    public bool IsPursuingPlayer { get; set; }

    private readonly float _fieldOfViewAngle = MathHelper.ToRadians(60);
    public float FieldOfViewAngle => _fieldOfViewAngle;
    #endregion

    #region Missile
    public List<MissileCooldownModel> Cooldowns { get; } = [];

    private readonly Vector2 PointLocalOffset = new(0, 30);
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
    #endregion

    public GameObjectType Type => GameObjectType.Enemy;

    public EnemyModel(Texture2D texture, Vector2 position) : base(texture,position)
    {
        _collider = new ColliderModel(GetBounds());
        for (int i = 0; i < MaxMissileCount; i++)
        {
            Cooldowns.Add(new MissileCooldownModel(6f));
        }
    }

    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
    }

    public void SetCurrentSpeed(float speed, bool isEvaiding)
    {
        _currentSpeed = Math.Clamp(_currentSpeed + speed * (isEvaiding ? _evasionKoeff : 1), 
            _minSpeed, _maxSpeed * (isEvaiding ? _evasionKoeff : 1));
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

    protected override Rectangle GetBounds()
    {
        int width = (int)(_texture.Width / 1.5f);
        int height = (int)(_texture.Height / 1.5f);
        Vector2 center = new(
            _position.X - (_texture.Width / 3.2f) + width / 2,
            _position.Y - (_texture.Height / 3.2f) + height / 2
        );

        if (_rotation == 0)
        {
            return new Rectangle(
                (int)(center.X - width / 2),
                (int)(center.Y - height / 2),
                width,
                height
            );
        }

        float cos = MathF.Cos(_rotation + MathHelper.PiOver2);
        float sin = MathF.Sin(_rotation + MathHelper.PiOver2);

        Vector2[] corners = 
        [
            new (-width / 2, -height / 2),
            new (width / 2, -height / 2),
            new (width / 2, height / 2),
            new (-width / 2, height / 2)
        ];

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

        return new Rectangle(
            (int)minX,
            (int)minY,
            (int)(maxX - minX),
            (int)(maxY - minY)
        );
    }
}

