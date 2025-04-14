using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Views;

public class PlayerView : IView
{
    private readonly PlayerModel _model;
    private float _alpha = 1f;
    public SpriteBatch SpriteBatch { get; set; }

    public PlayerView(PlayerModel playerModel)
    {
        _model = playerModel;
        _model.OnBlinkPhaseChanged += SetAlpha; 
    }

    public void Draw()
    {
        Color color = Color.White * _alpha;

        SpriteBatch.Draw(
            _model.Texture, 
            _model.Position, 
            null, 
            color,
            _model.Rotation, 
            _model.Origin, 
            1, 
            SpriteEffects.None, 
            0f);
    }

    private void SetAlpha(float phase)
    {
        _alpha = 0.5f + 0.5f * (float)Math.Sin(phase);
    }
}

