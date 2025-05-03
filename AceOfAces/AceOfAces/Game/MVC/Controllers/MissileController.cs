using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Controllers;

public class MissileController(MissileListModel _missilesList) : IController
{
    public void Update(float deltaTime)
    {
        UpdateCooldowns(deltaTime);
        UpdatePosition(deltaTime);
    }

    private void UpdatePosition(float deltaTime)
    {
        for (int i = 0; i < _missilesList.Missiles.Count; i++)
        {
            var missile = _missilesList.Missiles[i];

            if (missile.IsDestroyed || missile.Target == null) continue;

            Vector2 predictedPos = GetPredictedPosition(missile);
            Vector2 desiredDirection = GetDesiredDirection(missile, predictedPos);

            UpdateMissileVelocity(missile, desiredDirection, deltaTime);
            UpdateMissileRotation(missile);

            missile.SetPosition(missile.Velocity * deltaTime);
            missile.Lifespan -= deltaTime;
        }
    }

    private Vector2 GetPredictedPosition(MissileModel missile)
    {
        return missile.Target.Position + missile.Target.Velocity * missile.PredictedTime;
    }

    private Vector2 GetDesiredDirection(MissileModel missile, Vector2 predictedPos)
    {
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

        return desiredDirection;
    }

    private void UpdateMissileVelocity(MissileModel missile, Vector2 desiredDirection, float deltaTime)
    {
        missile.Velocity += (desiredDirection - missile.Velocity) * missile.RotationSpeed * deltaTime;
    }

    private void UpdateMissileRotation(MissileModel missile)
    {
        if (missile.Velocity.LengthSquared() > 0.1f)
        {
            missile.Rotation = (float)Math.Atan2(missile.Velocity.Y, missile.Velocity.X);
        }
    }

    private void UpdateCooldowns(float deltaTime)
    {
        foreach (var cooldown in _missilesList.Cooldowns)
        {
            if (cooldown.AvailableToFire) continue;

            cooldown.Timer -= deltaTime;
        }
    }
}