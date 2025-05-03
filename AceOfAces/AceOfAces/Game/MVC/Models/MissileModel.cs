using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class MissileModel : GameObjectModel
{
    private static Texture2D missileTexture;

    private float _lifespan = 3f;
    public float Lifespan
    {
        get => _lifespan;
        set
        {
            _lifespan = value;
            if (_lifespan <= 0)
            {
                Dispose();
            }
        }
    }

    #region Speed
    private readonly float _speed = 800f;
    public float Speed => _speed;

    private Vector2 _velocity;
    public Vector2 Velocity
    {
        get => _velocity;
        set
        {
            _velocity += value;
            if (_velocity.Length() > _speed)
            {
                _velocity = Vector2.Normalize(_velocity) * _speed;
            }
        }
    }

    public readonly float ArrivalTreshold = 100f;
    public readonly float PredictedTime = 0.2f;
    #endregion

    #region Rotation
    private readonly float _rotationSpeed = 8f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    #endregion

    #region Damage
    private readonly int _damage = 1;
    public int Damage => _damage;
    #endregion

    #region Target
    private ITarget _target;
    public ITarget Target
    {
        get => _target;
        set => _target = value;
    }

    private GameObjectType _source;
    public GameObjectType Source
    {
        get => _source;
        set => _source = value;
    }
        
    #endregion

    public MissileModel(Vector2 position) : base(missileTexture, position)
    {
        _collider = new ColliderModel(GetBounds());
    }

    public static void SetTerxture(Texture2D texture) => missileTexture = texture;

    public void SetPosition(Vector2 position)
    {
        _position += position;
        _collider.UpdateBounds(GetBounds());
    }

    protected override Rectangle GetBounds()
    {
        int width = (int)(_texture.Width / 1.1f);
        int height =(int)(_texture.Height / 1.1f);
        Vector2 center = new (
            _position.X - (_texture.Width / 2) + width / 2 + 5,
            _position.Y - (_texture.Height / 2) + height / 2
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

        Vector2[] corners = [
            new (-width / 2, -height / 2),
            new (width / 2, -height / 2),
            new (width / 2, height / 2),
            new (-width / 2, height / 2),
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

