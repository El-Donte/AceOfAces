using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class MenuModel
{
    private readonly Button _quitButton;
    public Button QuitButton => _quitButton; 

    private readonly Button _playButton;
    public Button PlayButton => _playButton;

    public bool Play { get; set; }
    public bool Quit { get; set; }

    public MenuModel()
    {
        _playButton = new Button(61,24)
        {
            Position = new Vector2(240, 72),
        };

        _quitButton = new Button(61, 24)
        {
            Position = new Vector2(240, 135),
        };
    }
}

