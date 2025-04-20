using AceOfAces.Core;
using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class DebugDraw : IView
{
    private readonly Texture2D _pixelTexture;

    private readonly Grid _grid;
    private readonly List<ColliderModel> _colliders;
    private readonly MissileListModel _missileList;

    public SpriteBatch SpriteBatch { get; set; }

    public DebugDraw(Texture2D pixelTexture, Grid grid, List<ColliderModel> colliders, MissileListModel missileList)
    {
        _pixelTexture = pixelTexture;
        _grid = grid;
        _colliders = colliders;
        _missileList = missileList;
    }

    public void Draw()
    {
        if (GameManager.IsDebugMode == false) return;

        DrawGrid();

        foreach (var collider in _colliders)
        {
            DrawRectangle(collider.Bounds, Color.Red);
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
            SpriteBatch.Draw(
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
            SpriteBatch.Draw(
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
        SpriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, lineWidth), color);
        SpriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - lineWidth, rect.Width, lineWidth), color);
        SpriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, lineWidth, rect.Height), color);
        SpriteBatch.Draw(_pixelTexture, new Rectangle(rect.X + rect.Width - lineWidth, rect.Y, lineWidth, rect.Height), color);
    }
}

