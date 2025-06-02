using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class GameOverView : IView
{
    private readonly SpriteBatch _spriteBatch;
    private readonly GameOverModel _model;
    private readonly Texture2D _gameOverTexture;

    public GameOverView(GameOverModel model, SpriteBatch spriteBatch)
    {
        _model = model;
        _spriteBatch = spriteBatch;
        _gameOverTexture = AssetsManager.GameOverTexture;
    }

    public void Draw()
    {
        _spriteBatch.Draw(_gameOverTexture, new Vector2(0, 0), Color.White);
        _model.MenuButton.Draw(_spriteBatch);
        _model.PlayAgainButton.Draw(_spriteBatch);
    }
}

