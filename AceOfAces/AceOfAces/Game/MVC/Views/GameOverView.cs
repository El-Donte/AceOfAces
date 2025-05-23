using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class GameOverView : IView
{
    private readonly SpriteBatch _spriteBatch;
    private readonly GameOverModel _model;
    private readonly SpriteFont _font;

    public GameOverView(GameOverModel model, SpriteBatch spriteBatch)
    {
        _model = model;
        _spriteBatch = spriteBatch;
        _font = AssetsManager.Font;
    }

    public void Draw()
    {
        _model.MenuButton.Draw(_spriteBatch);
        _model.PlayAgainButton.Draw(_spriteBatch);
        _spriteBatch.DrawString(_font, "Game Over", new Vector2(100, 100), Color.White);
    }
}

