using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class BackgroundView : IView
{
    private readonly List<LayerModel> _layers;
    private readonly PlayerModel _player;

    public SpriteBatch SpriteBatch { get; set; }

    public BackgroundView(List<LayerModel> layers, PlayerModel player)
    {
        _layers = layers;
        _player = player;
    }

    public void Draw()
    {
        foreach(var layer in _layers)
        {
            SpriteBatch.Draw(
                layer.Texture, 
                _player.Position,
                layer.Bounds, 
                Color.White,
                0,
                layer.Origin,
                layer.Zoom,
                SpriteEffects.None,
                1
            );
        }
    }
}

