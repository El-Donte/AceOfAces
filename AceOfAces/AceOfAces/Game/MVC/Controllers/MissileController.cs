using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Controllers;

public class MissileController : IController
{
    private const float MAX_SPEED = 800f;
    private const float STEERING_FORCE = 8f;
    private const float ARRIVAL_THRESHOLD = 100f;
    private const float PREDICTION_TIME = 0.2f;

    private MissileList _missilesList;

    public MissileController(MissileList missiles)
    {
        _missilesList = missiles;
    }

    public void Update(float deltaTime)
    {
        for(int i = 0; i < _missilesList.Missiles.Count; i++)
        {
            var missile = _missilesList.Missiles[i];
            if (missile.IsDestroyed || missile.Target == null)
                continue;

            // 1. Предсказание позиции цели
            Vector2 predictedPos = missile.Target.Position +
                                 missile.Target.Velocity * PREDICTION_TIME;

            // 2. Желаемое направление
            Vector2 desiredDirection = predictedPos - missile.Position;
            float distance = desiredDirection.Length();

            // 3. Нормализация и масштабирование скорости
            if (distance > 0.01f)
            {
                desiredDirection = Vector2.Normalize(desiredDirection) * MAX_SPEED;

                // 4. Режим замедления у цели
                if (distance < ARRIVAL_THRESHOLD)
                {
                    desiredDirection *= (distance / ARRIVAL_THRESHOLD);
                }
            }

            // 5. Вычисление силы руления
            Vector2 steering = (desiredDirection - missile.Velocity) * STEERING_FORCE;

            // 6. Применение силы
            missile.Velocity += steering * deltaTime;

            // 7. Ограничение скорости
            if (missile.Velocity.Length() > MAX_SPEED)
            {
                missile.Velocity = Vector2.Normalize(missile.Velocity) * MAX_SPEED;
            }

            // 8. Движение
            missile.Position += missile.Velocity * deltaTime;

            // 9. Поворот
            if (missile.Velocity.LengthSquared() > 0.1f)
            {
                missile.Rotation = (float)Math.Atan2(missile.Velocity.Y, missile.Velocity.X);
            }

            missile.UpdateLifeSpan(deltaTime);
        }
    }
}