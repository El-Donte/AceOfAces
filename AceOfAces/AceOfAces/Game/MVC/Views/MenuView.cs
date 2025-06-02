using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class MenuView : IView
{
    private readonly MenuModel _model;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D menuTexture;

    public MenuView(MenuModel model, SpriteBatch spriteBatch)
    {
        _model = model;
        _spriteBatch = spriteBatch;
        menuTexture = AssetsManager.MenuTexture;
    }

    public void Draw()
    {
        _spriteBatch.Draw(menuTexture, new Vector2(0, 0), Color.White);
        _model.QuitButton.Draw(_spriteBatch);
        _model.PlayButton.Draw(_spriteBatch);
    }
}

