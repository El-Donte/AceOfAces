using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core;

public class Camera2d
{
    private Viewport viewport;
    public float Zoom { get; set; }
    public float Rotation { get; set; }
    public Vector2 Position { get; set; }
    public Matrix TransformMatrix => GetTranformation();

    public Camera2d(Viewport viewport)
    {
        Zoom = 1.0f;
        Rotation = 0.0f;
        Position = Vector2.Zero;
        this.viewport = viewport;
    }

    private Matrix GetTranformation()
    {
        return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                        Matrix.CreateRotationZ(Rotation) *
                                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                        Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
    }
}

