using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Core;

public static class GameEvents
{
    public static event Action<Vector2> ExplosionEvent;
    public static event Action<Vector2> BulletTrailEvent;
    public static event Action<int> ChangeTragetEvent;

    public static void TriggerExplosion(Vector2 position)
    {
        ExplosionEvent?.Invoke(position);
    }

    public static void TriggerBulletTrail(Vector2 position)
    {
        BulletTrailEvent?.Invoke(position);
    }

    public static void ChangeTarget(int targetIndex)
    {
        ChangeTragetEvent?.Invoke(targetIndex);
    }
}

