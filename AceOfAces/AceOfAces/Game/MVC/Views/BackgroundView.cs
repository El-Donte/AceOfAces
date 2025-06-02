using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

    public BackgroundView(List<LayerModel> layers, SpriteBatch spriteBatch)
    {
        _layers = layers;
        _player = null;
        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        var position = _player == null ? new Vector2(960, 540): _player.Position;
        foreach(var layer in _layers)
        {
            _spriteBatch.Draw(
                layer.Texture, 
                position,
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

