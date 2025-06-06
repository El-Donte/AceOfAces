using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class BackgroundView : IView
{
    private readonly SpriteBatch _spriteBatch;

    private readonly List<LayerModel> _layers;
    private readonly PlayerModel _player;

    public BackgroundView(List<LayerModel> layers, PlayerModel player, SpriteBatch spriteBatch)
    {
        _layers = layers;
        _player = player;
        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {;
        foreach(var layer in _layers)
        {
            _spriteBatch.Draw(
                layer.Texture,
                _player.Position,
                layer.Bounds, 
                Color.White,
                0f,
                layer.Origin,
                layer.Zoom,
                SpriteEffects.None,
                1f
            );
        }
    }
}

