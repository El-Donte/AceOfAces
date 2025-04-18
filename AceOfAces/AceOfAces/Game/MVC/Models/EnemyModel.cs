using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models;

public class EnemyModel : GameObjectModel, ITarget
{
    #region Rotation
    private readonly float _rotationSpeed = 4f;
    public float RotationSpeed => _rotationSpeed; // Скорость поворота

    protected float _rotation;
    public float Rotation => _rotation; // Угол поворота

    public event Action<float> RotationChanged;
    #endregion

    private Vector2 _velocity;
    public Vector2 Velocity => _velocity;

    private int _health = 100;

    public EnemyModel(Texture2D texture, Vector2 position) : base(texture,position)
    {
        _collider = new ColliderModel(GetBounds(),GameObjectType.Enemy);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}

