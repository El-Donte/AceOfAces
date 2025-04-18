using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Core;

public class Grid
{
    private readonly GraphicsDeviceManager _graphics;
    private GameObjectModel[,] _cells;

    private Vector2 _gridWorldPosition;
    public Vector2 GridWorldPosition => _gridWorldPosition;

    private readonly int _cellSize;
    public int CellSize => _cellSize;

    private int _width;
    public int Width => _width;

    private int _height;
    public int Height => _height;

    public Grid(int cellSize, GraphicsDeviceManager graphics)
    {
        _cellSize = cellSize;
        _graphics = graphics;
    }

    private void UpdateGridWidthHeight()
    {
        _width = (int)Math.Ceiling((float)_graphics.GraphicsDevice.Viewport.Width / _cellSize) + 1;
        _height = (int)Math.Ceiling((float)_graphics.GraphicsDevice.Viewport.Height / _cellSize) + 1;
        _cells = new GameObjectModel[_width, _height];
    }

    public void UpdateGridPosition(Vector2 playerPosition)
    {
        UpdateGridWidthHeight();
        _gridWorldPosition = new Vector2(
            playerPosition.X - (_width * _cellSize) / 2f,
            playerPosition.Y - (_height * _cellSize) / 2f
        );
    }

    public void AddObject(GameObjectModel obj)
    {
        Vector2 relativePos = obj.Position - _gridWorldPosition;

        int x = (int)(relativePos.X / _cellSize);
        int y = (int)(relativePos.Y / _cellSize);

        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            _cells[x, y] = obj;
        }
    }

    public List<GameObjectModel> GetNearbyObjects(Vector2 objectPosition)
    {
        var result = new List<GameObjectModel>();
        
        int centerX = _width / 2 + 1;
        int centerY = _height / 2 + 1;
        int radius = Math.Max(centerX, centerY);

        for (int x = centerX - radius; x <= centerX + radius; x++)
        {
            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height)
                {
                    if (_cells[x, y] != null)
                    {
                        result.Add(_cells[x, y]);
                    }
                }
            }
        }

        return result;
    }
}
