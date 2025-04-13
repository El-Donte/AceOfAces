using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Models
{
    public class MissileModel : GameObjectModel
    {
        private readonly int _damage = 50;
        private float _lifespan = 5f;
        private float _speed = 500f;
        private float _rotationSpeed = 3f;
        private GameObjectModel _target;
        private GameObjectType _source;

        public Vector2 Direction { get; set; }
        public float Lifespan => _lifespan;
        
        public int Damage => _damage;
        public GameObjectModel Target => _target;
        public GameObjectType Source => GameObjectType.Player;

        public MissileModel(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _collider = new CollisionModel(GetBounds(), GameObjectType.Missile);
        }

        public override void Move(Vector2 direction, float deltaTime)
        {
            _position += direction * _speed * deltaTime;

            _collider.UpdateBounds(GetBounds());
        }

        public override void Rotate(Vector2 inputDirection, float deltaTime)
        {
            // Нормализуем углы
            float difference = inputDirection.X - _rotation;
            while (difference < -Math.PI) difference += MathHelper.TwoPi;
            while (difference > Math.PI) difference -= MathHelper.TwoPi;

            // Плавное изменение угла
            _rotation += MathHelper.Clamp(difference, -_rotationSpeed * deltaTime, _rotationSpeed * deltaTime);
        }

        public void UpdateLifeSpan(float deltaTime)
        {
            //_lifespan -= deltaTime;
            //if (_lifespan < 0f)
            //{
            //    Dispose();
            //}
        }

        public void SetTarget(GameObjectModel target)
        {
            _target = target;
        }
    }
}
