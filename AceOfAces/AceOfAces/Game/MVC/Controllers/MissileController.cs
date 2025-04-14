using AceOfAces.Models;
using Microsoft.Xna.Framework;

namespace AceOfAces.Controllers;

public class MissileController : IController
{
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

                if (distance <  missile.ArrivalTreshold)
                {
                    desiredDirection *= (distance / missile.ArrivalTreshold);
                }
            }

            Vector2 steering = (desiredDirection - missile.Velocity) * missile.RotationSpeed;

            missile.Move(steering, deltaTime);

            missile.Rotate(Vector2.Zero, deltaTime);

            missile.UpdateLifeSpan(deltaTime);
        }
    }
}