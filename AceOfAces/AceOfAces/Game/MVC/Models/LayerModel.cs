using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Models;

public class LayerModel
{
    private readonly Viewport _viewport;

    #region Position
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }
    #endregion

    #region Speed
    private readonly float _layerSpeed;
    public float LayerSpeed => _layerSpeed;
    #endregion

    #region Zoom
    private readonly float _zoom;
    public float Zoom => _zoom;
    #endregion

    #region Texture
    private readonly Texture2D _texture;
    public Texture2D Texture => _texture;

    public Vector2 Origin => new (Bounds.Width / 2, Bounds.Height / 2);
    #endregion

    #region Bounds
    public Rectangle Bounds
    {
        get
        {
            return new Rectangle((int)_position.X, (int)_position.Y,
                                (int)(_viewport.Width / _zoom),
                                (int)(_viewport.Height / _zoom));
        }
    }
    #endregion

    public LayerModel(Texture2D texture,float zoom, float speedKoeff, Viewport viewport)
    {
        _zoom = zoom;
        _layerSpeed = speedKoeff;
        _viewport = viewport;
        _texture = texture;
        _position = Vector2.Zero;
    }
}

