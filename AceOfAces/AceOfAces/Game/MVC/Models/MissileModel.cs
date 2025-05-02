using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Models;

public class MissileModel : GameObjectModel
{
    private static Texture2D missileTexture;

    private float _lifespan = 3f;

    #region Speed
    private float _speed = 800f;
    public float Speed => _speed;

    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    public readonly float ArrivalTreshold = 100f;
    public readonly float PredictedTime = 0.2f;
    #endregion

    #region Rotation
    private float _rotationSpeed = 8f;
    public float RotationSpeed => _rotationSpeed;

    protected float _rotation;
    public float Rotation => _rotation; // Угол поворота
    #endregion

    #region Damage
    private readonly int _damage = 1;
    public int Damage => _damage;
    #endregion

    #region Target
    private ITarget _target;
    public ITarget Target => _target;

    private GameObjectType _source;
    public GameObjectType Source => _source;
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

    public void SetRotation(float rotation)
    {
        _rotation = rotation;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity += velocity;
        if (_velocity.Length() > _speed)
        {
            _velocity = Vector2.Normalize(_velocity) * _speed;
        }
    }

    public void ReduceLifespan(float deltaTime)
    {
        _lifespan -= deltaTime;
        if (_lifespan <= 0)
        {
            Dispose();
        }
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }

    public void SetSource(GameObjectType source)
    {
        _source = source;
    }

    protected override Rectangle GetBounds()
    {
        return new Rectangle(
            (int)(_position.X - _texture.Width / 2),
            (int)(_position.Y - _texture.Height / 2),
            _texture.Width,
            _texture.Height
        );
    }
}

