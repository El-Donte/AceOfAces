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

    private Rectangle _gridBounds;

    public Grid(int cellSize, GraphicsDeviceManager graphics )
    {
        _cellSize = cellSize;
        _graphics = graphics;

        _width = (int)Math.Ceiling((float)_graphics.GraphicsDevice.Viewport.Width / cellSize) + 2;
        _height =(int)Math.Ceiling((float)_graphics.GraphicsDevice.Viewport.Height / cellSize) + 2;

        _cells = new GameObjectModel[_width, _height];
        _gridBounds = new Rectangle(0, 0, _width * _cellSize, _height * _cellSize);
    }

    public void UpdateGridPosition(Vector2 centerPosition)
    {
        _gridWorldPosition = new Vector2(
            centerPosition.X - (_width * _cellSize) / 2,
            centerPosition.Y - (_height * _cellSize) / 2
        );

        _gridBounds.X = (int)_gridWorldPosition.X;
        _gridBounds.Y = (int)_gridWorldPosition.Y;
    }

    public void AddObject(GameObjectModel obj)
    {
        if (!_gridBounds.Contains(obj.Position))
        {
            return;
        }

        Vector2 gridRelativePos = obj.Position - _gridWorldPosition;
        int x = (int)(gridRelativePos.X / _cellSize);
        int y = (int)(gridRelativePos.Y / _cellSize);

        var isInGrid = x >= 0 && x < _width && y >= 0 && y < _height;
        if (isInGrid)
        {
            _cells[x, y] = obj;
        }
    }

    public List<GameObjectModel> GetNearbyObjects(Vector2 position, int radiusInCells = 2)
    {
        var result = new List<GameObjectModel>();

        if (!_gridBounds.Contains(position))
        {
            return result;
        }

        var (minX, maxX, minY, maxY) = GetBounds(position, radiusInCells);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (_cells[x, y] != null)
                {
                    result.Add(_cells[x, y]);
                }
            }
        }

        return result;
    }

    private (int minX, int maxX, int minY, int maxY) GetBounds(Vector2 position, int radiusInCells = 2)
    {
        Vector2 gridRelativePos = position - _gridWorldPosition;
        int centerX = (int)(gridRelativePos.X / _cellSize);
        int centerY = (int)(gridRelativePos.Y / _cellSize);

        int minX = Math.Max(centerX - radiusInCells, 0);
        int maxX = Math.Min(centerX + radiusInCells, _width - 1);
        int minY = Math.Max(centerY - radiusInCells, 0);
        int maxY = Math.Min(centerY + radiusInCells, _height - 1);

        return (minX, maxX, minY, maxY);
    }
}
