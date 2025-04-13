using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models
{
    public class EnemyModel : GameObjectModel
    {
        #region Rotation
        private readonly float _rotationSpeed = 4f;
        public float RotationSpeed => _rotationSpeed; // Скорость поворота

        private int _health = 100;

        public event Action<float> RotationChanged;
        #endregion

        public EnemyModel(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _collider = new CollisionModel(GetBounds(),GameObjectType.Enemy);
        }

        public override void Move(Vector2 direction, float deltaTime)
        {
            Velocity = direction * 100f;
            _position += Velocity * deltaTime;
        }

        public override void Rotate(Vector2 inputDirection, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }
}
