using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class EnemyView : IView
{
    private readonly EnemyModel _model;
    public SpriteBatch SpriteBatch { get; set; }

    public EnemyView(EnemyModel model)
    {
        _model = model;
    }

    public void Draw()
    {
        SpriteBatch.Draw(
            _model.Texture,
            _model.Position,
            null,
            Color.White,
            _model.Rotation,
            _model.Origin,
            1,
            SpriteEffects.None,
            0f);
    }
}

