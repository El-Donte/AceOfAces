using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Models;

public class BackgroundModel
{
    private readonly Viewport _viewport;
    private Vector2 _position;

    #region Speed
    private readonly float _speedKoeff;
    public float SpeedKoeff => _speedKoeff;
    #endregion

    #region Zoom
    private float _zoom;
    public float Zoom => _zoom;
    #endregion

    #region Texture
    private readonly Texture2D _texture;
    public Texture2D Texture => _texture;
    #endregion

    #region Rectangle
    public Rectangle Rectangle
    {
        get
        {
            return new Rectangle((int)_position.X, (int)_position.Y,
                                (int)(_viewport.Width / Zoom),
                                (int)(_viewport.Height / Zoom));
        }
    }
    public Vector2 Origin => new Vector2(Rectangle.Width / 2, Rectangle.Height / 2);
    #endregion

    public BackgroundModel(Texture2D texture,float zoom, float speedKoeff, Viewport viewport)
    {
        _zoom = zoom;
        _speedKoeff = speedKoeff;
        _viewport = viewport;
        _texture = texture;
        _position = Vector2.Zero;
    }

    public void Move(Vector2 direction, float deltaTime)
    {
        _position += direction * SpeedKoeff * deltaTime;
    }
}

