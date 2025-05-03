using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class EnemyView : IView
{
    private readonly Texture2D _missileTexture;
    private readonly Vector2 _missileOrigin;

    private readonly List<EnemyModel> _enemies;
    public SpriteBatch SpriteBatch { get; set; }

    public EnemyView(List<EnemyModel> enemies, Texture2D missileTexture)
    {
        _enemies = enemies;

        _missileTexture = missileTexture;
        _missileOrigin = new Vector2(_missileTexture.Width / 2, _missileTexture.Height / 2);
    }

    public void Draw()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            if (enemy.Cooldowns[0].AvailableToFire)
            {
                DrawMissile(enemy,-enemy.MissileJointPosition);
            }

            if (enemy.Cooldowns[1].AvailableToFire)
            {
                DrawMissile(enemy,enemy.MissileJointPosition);
            }

            DrawEnemy(enemy);
        }
    }

    private void DrawMissile(EnemyModel enemy,Vector2 offset)
    {
        SpriteBatch.Draw(
            _missileTexture,
            enemy.Position + offset,
            null,
            Color.White,
            enemy.Rotation + MathHelper.PiOver2,
            _missileOrigin,
            1f,
            SpriteEffects.None,
            0f
        );
    }

    public void DrawEnemy(EnemyModel enemy)
    {
        SpriteBatch.Draw(
            enemy.Texture,
            enemy.Position,
            null,
            Color.White,
            enemy.Rotation + MathHelper.PiOver2,
            enemy.Origin,
            1,
            SpriteEffects.None,
            0f);
    }
}

