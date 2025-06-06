using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AceOfAces.Core;

public record Point(int x, int y);

public class Grid
{
    private readonly GraphicsDevice _graphics;
    private readonly List<GameObjectModel>[,] _cells;
    private Rectangle _gridBounds;

    private Vector2 _gridWorldPosition;
    public Vector2 GridWorldPosition => _gridWorldPosition;

    private readonly int _cellSize;
    public int CellSize => _cellSize;

    private readonly int _width;
    public int Width => _width;

    private readonly int _height;
    public int Height => _height;

    private readonly Dictionary<GameObjectModel, Point> _objectPositions = new();
    public List<GameObjectModel> ActiveObjects => _objectPositions.Keys.ToList();

    public Grid(int cellSize, GraphicsDevice graphics)
    {
        _cellSize = cellSize;
        _graphics = graphics;

        _width = (int)Math.Ceiling((float)_graphics.Viewport.Width / cellSize) + 2;
        _height = (int)Math.Ceiling((float)_graphics.Viewport.Height / cellSize) + 2;

        _cells = new List<GameObjectModel>[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _cells[x, y] = [];
            }
        }

        _gridBounds = new Rectangle(0, 0, _width * _cellSize, _height * _cellSize);
    }

    public void UpdateGridPosition(Vector2 centerPosition)
    {
        _gridWorldPosition = new Vector2(
            centerPosition.X - (_width * _cellSize) / 2,
            centerPosition.Y - (_height * _cellSize) / 2);

        _gridBounds.X = (int)_gridWorldPosition.X;
        _gridBounds.Y = (int)_gridWorldPosition.Y;
    }

    public void AddObject(GameObjectModel obj)
    {
        if (obj.IsDestroyed || !_gridBounds.Contains(obj.Position))
        {
            return;
        }

        RemoveObject(obj);

        var pos = GetCellCoordinates(obj.Position);
        if (IsInGrid(pos.x, pos.y))
        {
            _cells[pos.x, pos.y].Add(obj);
            _objectPositions[obj] = pos;
        }
    }

    public void RemoveObject(GameObjectModel obj)
    {
        if (_objectPositions.TryGetValue(obj, out var coords))
        {
            _cells[coords.x, coords.y].Remove(obj);
            _objectPositions.Remove(obj);
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
                result.AddRange(_cells[x, y]);
            }
        }

        return result;
    }

    private Point GetCellCoordinates(Vector2 worldPos)
    {
        Vector2 gridRelativePos = worldPos - _gridWorldPosition;
        int x = (int)(gridRelativePos.X / _cellSize);
        int y = (int)(gridRelativePos.Y / _cellSize);
        return new Point(x, y);
    }

    private bool IsInGrid(int x, int y) =>
        x >= 0 && x < _width && y >= 0 && y < _height;

    private (int minX, int maxX, int minY, int maxY) GetBounds(Vector2 position, int radiusInCells)
    {
        var coords = GetCellCoordinates(position);
        int centerX = coords.x;
        int centerY = coords.y;

        int minX = Math.Max(centerX - radiusInCells, 0);
        int maxX = Math.Min(centerX + radiusInCells, _width - 1);
        int minY = Math.Max(centerY - radiusInCells, 0);
        int maxY = Math.Min(centerY + radiusInCells, _height - 1);

        return (minX, maxX, minY, maxY);
    }

    public void Clear()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _cells[x, y].Clear();
            }
        }

        _objectPositions.Clear();
    }
}