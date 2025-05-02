using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class EnemyView : IView
{
    private readonly List<EnemyModel> _model;
    public SpriteBatch SpriteBatch { get; set; }

    public EnemyView(List<EnemyModel> model)
    {
        _model = model;
    }

    public void Draw()
    {
        for (int i = 0; i < _model.Count; i++)
        {
            var model = _model[i];
            SpriteBatch.Draw(
                model.Texture,
                model.Position,
                null,
                Color.White,
                model.Rotation + MathHelper.PiOver2,
                model.Origin,
                1,
                SpriteEffects.None,
                0f);
        }
    }
}

