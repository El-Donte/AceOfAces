using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class MenuView : IView
{
    private readonly MenuModel _model;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _font;

    public MenuView(MenuModel model, SpriteBatch spriteBatch)
    {
        _model = model;
        _spriteBatch = spriteBatch;
        _font = AssetsManager.Font;
    }

    public void Draw()
    {
        _model.QuitButton.Draw(_spriteBatch);
        _model.PlayButton.Draw(_spriteBatch);
        _spriteBatch.DrawString(_font, "Ace Of Aces", new Vector2(100, 100), Color.White);
    }
}

