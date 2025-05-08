using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core;

public class Camera(Viewport viewport)
{
    private Vector2 _position = Vector2.Zero;

    public Matrix TransformMatrix => GetTranformation();

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    private Matrix GetTranformation()
    {
        return Matrix.CreateTranslation(
                new Vector3(-_position.X, -_position.Y, 0)) *
                Matrix.CreateScale(new Vector3(1f, 1f, 1f)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0)
            );
    }
}