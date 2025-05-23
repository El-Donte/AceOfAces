using AceOfAces.Core;
using AceOfAces.Managers;
using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class GameOverModel
{
    private readonly Button _menuButton;
    private readonly Button _playAgainButton;
    public Button MenuButton { get => _menuButton; }
    public Button PlayAgainButton { get => _playAgainButton; }

    public bool PlayAgain { get; set; }
    public bool Menu { get; set; }

    public GameOverModel()
    {
        PlayAgain = false;
        Menu = false;

        _menuButton = new Button(AssetsManager.ButtonTexture, AssetsManager.Font)
        {
            Position = new Vector2(300, 300),
            Text = "Menu",
        };

        _playAgainButton = new Button(AssetsManager.ButtonTexture, AssetsManager.Font)
        {
            Position = new Vector2(300, 200),
            Text = "Play again",
        };
    }
}

