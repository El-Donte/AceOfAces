using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Models;

public class ColliderModel
{
    private readonly int _width;
    private readonly int _height;
    private readonly float _koeff;

    private Rectangle _bounds;
    public Rectangle Bounds => _bounds;

    public ColliderModel(int width, int height, float koeff)
    {
        _width = width;
        _height = height;
        _koeff = koeff;
    }

    public void UpdateBounds(Vector2 position, float rotation)
    {
        Vector2 center = new(
            position.X - (_width / _koeff) + _width / 5,
            position.Y - (_height / _koeff) + _height / 5
        );

        if (rotation == 0)
        {
            _bounds = new Rectangle(
                (int)(center.X - _width / 2),
                (int)(center.Y - _height / 2),
                _width,
                _height
            );
            return;
        }

        float cos = MathF.Cos(rotation);
        float sin = MathF.Sin(rotation);

        Vector2[] corners = [
            new (-_width / 2, -_height / 2),
            new (_width / 2, -_height / 2),
            new (_width / 2, _height / 2),
            new (-_width / 2, _height / 2),
        ];

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (Vector2 corner in corners)
        {
            Vector2 rotated = new Vector2(
                corner.X * cos - corner.Y * sin,
                corner.X * sin + corner.Y * cos
            ) + center;

            minX = Math.Min(minX, rotated.X);
            maxX = Math.Max(maxX, rotated.X);
            minY = Math.Min(minY, rotated.Y);
            maxY = Math.Max(maxY, rotated.Y);
        }

        _bounds = new Rectangle(
            (int)minX,
            (int)minY,
            (int)(maxX - minX),
            (int)(maxY - minY)
        );
    }
}

