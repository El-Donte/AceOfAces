using Microsoft.Xna.Framework;
using System;


namespace AceOfAces.Core;

public static class GameEvents
{
    public static event Action<Vector2> OnExplosionEvent;
    public static event Action<Vector2> OnBulletTrailEvent;
    public static event Action OnGameOverEvent;

    public static void TriggerExplosion(Vector2 position)
    {
        OnExplosionEvent?.Invoke(position);
    }

    public static void TriggerBulletTrail(Vector2 position)
    {
        OnBulletTrailEvent?.Invoke(position);
    }

    public static void TriggerGameOver()
    {
        OnGameOverEvent?.Invoke();
    }
}

