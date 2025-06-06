using AceOfAces.Managers;
using AceOfAces.Core;
using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class MissileModel : GameObjectModel
{
    #region Lifespan
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

    private readonly float _arrivalTreshold = 100f;
    public float ArrivalTreshold => _arrivalTreshold;

    private readonly float _predictedTime = 0.2f;
    public float PredictedTime => _predictedTime;
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

    public MissileModel(Vector2 position) : base(position)
    {
        _collider = new ColliderModel(AssetsManager.MissileTexture.Height, AssetsManager.MissileTexture.Width, 4f, 0.8f);
    }
}