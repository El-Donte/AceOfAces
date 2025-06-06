using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Models;

public class ColliderModel
{
    private readonly int _width;
    private readonly int _height;
    private readonly float _sizeKoeff;
    private readonly float _scale;

    private Rectangle _bounds;
    public Rectangle Bounds => _bounds;

    private bool _isEnable = true;
    public bool IsEnable 
    {
        get => _isEnable; 
        set => _isEnable = value; 
    }

    public ColliderModel(int width, int height, float sizeKoeff, float scale = 1.0f)
    {
        _width = width;
        _height = height;
        _sizeKoeff = sizeKoeff;
        _scale = Math.Clamp(scale, 0.1f, 2.0f);
    }

    public void UpdateBounds(Vector2 position, float rotation)
    {
        Vector2 center = new(
            position.X - (_width / _sizeKoeff) + _width / 5,
            position.Y - (_height / _sizeKoeff) + _height / 5
        );

        float scaledWidth = _width * _scale;
        float scaledHeight = _height * _scale;

        if (rotation == 0)
        {
            _bounds = new Rectangle(
                (int)(center.X - scaledWidth / 2),
                (int)(center.Y - scaledHeight / 2),
                (int)scaledWidth,
                (int)scaledHeight
            );
            return;
        }

        float cos = MathF.Cos(rotation);
        float sin = MathF.Sin(rotation);

        Vector2[] corners = [
            new (-scaledWidth / 2, -scaledHeight / 2),
            new (scaledWidth / 2, -scaledHeight / 2),
            new (scaledWidth / 2, scaledHeight / 2),
            new (-scaledWidth / 2, scaledHeight / 2),
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

