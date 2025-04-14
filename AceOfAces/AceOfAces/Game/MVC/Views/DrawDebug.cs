using AceOfAces.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public static class DebugDraw
{
    private static Texture2D _pixelTexture;
    private static Color _gridColor = Color.Violet;

    public static void Initialize(GraphicsDevice graphicsDevice)
    {
        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    public static void DrawGrid(SpriteBatch spriteBatch, Grid grid)
    {
        Vector2 gridCenter = grid.GridWorldPosition;
        Vector2 startPos = grid.GridWorldPosition;

        for (int x = 0; x <= grid.Width; x++)
        {
            Vector2 linePos = startPos + new Vector2(x * grid.CellSize, 0);
            spriteBatch.Draw(
                _pixelTexture,
                new Rectangle(
                    (int)linePos.X,
                    (int)linePos.Y,
                    1,
                    grid.Height * grid.CellSize
                ),
                _gridColor
            );
        }

        for (int y = 0; y <= grid.Height; y++)
        {
            Vector2 linePos = startPos + new Vector2(0, y * grid.CellSize);
            spriteBatch.Draw(
                _pixelTexture,
                new Rectangle(
                    (int)linePos.X,
                    (int)linePos.Y,
                        grid.Width * grid.CellSize,
                    1
                ),
                _gridColor
            );
        }
    }

    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color, int lineWidth = 1)
    {
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, lineWidth), color);
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - lineWidth, rect.Width, lineWidth), color);
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, lineWidth, rect.Height), color);
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X + rect.Width - lineWidth, rect.Y, lineWidth, rect.Height), color);
    }
}

