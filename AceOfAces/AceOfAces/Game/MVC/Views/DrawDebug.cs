using AceOfAces.Core;
using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class DebugView : IView
{
    private readonly Texture2D _pixelTexture = AssetsManager.PixelTexture;

    private readonly List<EnemyModel> _enemies;
    private readonly MissileListModel _missileList;
    private readonly PlayerModel _player;
    private readonly Grid _grid;

    private readonly SpriteBatch _spriteBatch;

    public DebugView(Grid grid, PlayerModel player, SpawnerModel spawner, MissileListModel missileList, SpriteBatch spriteBatch)
    {
        _grid = grid;
        _enemies = spawner.Enemies;
        _missileList = missileList;
        _player = player;
        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        if (GameManager.IsDebugMode == false) return;

        DrawGrid();

        DrawRectangle(_player.Collider.Bounds, Color.Green);

        foreach (var enemy in _enemies)
        {
            DrawRectangle(enemy.Collider.Bounds, Color.Red);
        }

        foreach (var missile in _missileList.Missiles)
        {
            DrawRectangle(missile.Collider.Bounds, Color.Blue);
        }
    }

    private void DrawGrid()
    {
        Vector2 startPos = _grid.GridWorldPosition;

        for (int x = 0; x <= _grid.Width; x++)
        {
            Vector2 linePos = startPos + new Vector2(x * _grid.CellSize, 0);
            _spriteBatch.Draw(
                _pixelTexture,
                new Rectangle(
                    (int)linePos.X,
                    (int)linePos.Y,
                    1,
                    _grid.Height * _grid.CellSize
                ),
                Color.DarkViolet
            );
        }

        for (int y = 0; y <= _grid.Height; y++)
        {
            Vector2 linePos = startPos + new Vector2(0, y * _grid.CellSize);
            _spriteBatch.Draw(
                _pixelTexture,
                new Rectangle(
                    (int)linePos.X,
                    (int)linePos.Y,
                        _grid.Width * _grid.CellSize,
                    1
                ),
               Color.DarkViolet
            );
        }
    }

    private void DrawRectangle(Rectangle rect, Color color, int lineWidth = 1)
    {
        _spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, lineWidth), color);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - lineWidth, rect.Width, lineWidth), color);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, lineWidth, rect.Height), color);
        _spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X + rect.Width - lineWidth, rect.Y, lineWidth, rect.Height), color);
    }
}

