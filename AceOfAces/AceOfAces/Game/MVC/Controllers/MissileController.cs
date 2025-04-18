using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Controllers;

public class MissileController : IController
{
    private MissileListModel _missilesList;

    public MissileController(MissileListModel missiles)
    {
        _missilesList = missiles;
    }

    public void Update(float deltaTime)
    {
        UpdateCooldowns(deltaTime);
        UpdatePostion(deltaTime);
    }

    private void UpdatePostion(float deltaTime)
    {
        for (int i = 0; i < _missilesList.Missiles.Count; i++)
        {
            var missile = _missilesList.Missiles[i];
            if (missile.IsDestroyed || missile.Target == null)
            {
                continue;
            }

            Vector2 predictedPos = missile.Target.Position +
                    missile.Target.Velocity * missile.PredictedTime;

            Vector2 desiredDirection = predictedPos - missile.Position;
            float distance = desiredDirection.Length();

            if (distance > 0.01f)
            {
                desiredDirection = Vector2.Normalize(desiredDirection) * missile.Speed;

                if (distance < missile.ArrivalTreshold)
                {
                    desiredDirection *= (distance / missile.ArrivalTreshold);
                }
            }

            Vector2 velocity = ((desiredDirection - missile.Velocity) * missile.RotationSpeed) * deltaTime;
            missile.SetVelocity(velocity);

            float rotation = 0f; 
            if (missile.Velocity.LengthSquared() > 0.1f)
            {
                rotation = (float)Math.Atan2(missile.Velocity.Y, missile.Velocity.X);
            }
            missile.SetRotation(rotation);

            var position = missile.Velocity * deltaTime;
            missile.SetPosition(position);

            missile.ReduceLifespan(deltaTime);
        }
    }

    private void UpdateCooldowns(float deltaTime)
    {
        foreach (var cooldown in _missilesList.Cooldowns)
        {
            if (cooldown.AvailableToFire) continue;

            cooldown.Timer -= deltaTime;

            if (cooldown.Timer <= 0)
            {
                cooldown.AvailableToFire = true;
            }
        }
    }
}