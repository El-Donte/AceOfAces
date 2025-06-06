using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class EnemyView : IView
{
    private readonly Texture2D _missileTexture = AssetsManager.MissileTexture;
    private readonly Vector2 _missileOrigin;
    private readonly Texture2D _pixelTexture = AssetsManager.PixelTexture;

    private readonly Texture2D _enemyTexture = AssetsManager.EnemyTexture;
    private readonly Vector2 _enemyOrigin;
    private readonly List<EnemyModel> _enemies;

    private readonly SpriteBatch _spriteBatch;

    public EnemyView(SpawnerModel spawner, SpriteBatch spriteBatch)
    {
        _enemies = spawner.Enemies;
        _enemyOrigin = new Vector2(_enemyTexture.Width / 2f, _enemyTexture.Height / 2f + 10);

        _missileOrigin = new Vector2(_missileTexture.Width / 2f, _missileTexture.Height / 2f);
        _spriteBatch = spriteBatch;
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
        _spriteBatch.Draw(
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
        _spriteBatch.Draw(
            _enemyTexture,
            enemy.Position,
            null,
            Color.White,
            enemy.Rotation + MathHelper.PiOver2,
            _enemyOrigin,
            1,
            SpriteEffects.None,
            0f);

        if (enemy.IsTargeted)
        {
            Rectangle squareRect = new Rectangle(
                (int)(enemy.Position.X - 10),
                (int)(enemy.Position.Y - 10),
                20,
                20
            );
            DrawRectangleOutline(squareRect, Color.DarkOliveGreen);
        }
    }

    private void DrawRectangleOutline(Rectangle bounds,Color color,int thickness = 5)
    {
        _spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X - thickness, bounds.Y - thickness, bounds.Width + 2 * thickness, thickness), color);

        _spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X - thickness, bounds.Y + bounds.Height, bounds.Width + 2 * thickness, thickness), color);

        _spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X - thickness, bounds.Y, thickness, bounds.Height), color);

        _spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X + bounds.Width, bounds.Y,thickness, bounds.Height), color);
    }
}

