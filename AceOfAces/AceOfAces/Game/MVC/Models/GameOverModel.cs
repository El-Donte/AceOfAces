using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class GameOverModel
{
    private readonly Button _menuButton;
    private readonly Button _playAgainButton;
    public Button MenuButton => _menuButton;
    public Button PlayAgainButton => _playAgainButton;

    public bool PlayAgain { get; set; }
    public bool Menu { get; set; }

    public GameOverModel()
    {
        PlayAgain = false;
        Menu = false;

        _playAgainButton = new Button(63, 26)
        {
            Position = new Vector2(128, 80),
        };

        _menuButton = new Button(63,26)
        {
            Position = new Vector2(128, 111),
        };
    }
}

