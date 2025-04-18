using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class BackgroundView : IView
{
    private readonly List<BackgroundModel> _models;
    private readonly PlayerModel _playerModel;

    public SpriteBatch SpriteBatch { get; set; }

    public BackgroundView(List<BackgroundModel> models, PlayerModel playerModel)
    {
        _models = models;
        _playerModel = playerModel;
    }

    public void Draw()
    {
        foreach(var model in _models)
        {
            SpriteBatch.Draw(
                model.Texture, 
                _playerModel.Position,
                model.Rectangle, 
                Color.White,
                0,
                model.Origin,
                model.Zoom,
                SpriteEffects.None,
                1
            );
        }
    }
}

