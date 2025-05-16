using Microsoft.Xna.Framework;
using System;


namespace AceOfAces.Core
{
    public static class GameEvents
    {
        public static event Action<Vector2> OnExplosion;
        public static event Action<Vector2> OnBulletTrail;

        public static void TriggerExplosion(Vector2 position)
        {
            OnExplosion?.Invoke(position);
        }

        public static void TriggerBulletTrail(Vector2 position)
        {
            OnBulletTrail?.Invoke(position);
        }
    }
}
