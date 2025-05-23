using AceOfAces.Core;
using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceOfAces.Models;

public class MenuModel
{
    private readonly Button _quitButton;
    private readonly Button _playButton;
    public Button QuitButton { get => _quitButton; }
    public Button PlayButton { get => _playButton; }

    public bool Play { get; set; }
    public bool Quit { get; set; }

    public MenuModel()
    {
        _playButton = new Button(AssetsManager.ButtonTexture, AssetsManager.Font)
        {
            Position = new Vector2(300, 200),
            Text = "Play",
        };

        _quitButton = new Button(AssetsManager.ButtonTexture, AssetsManager.Font)
        {
            Position = new Vector2(300, 300),
            Text = "Quit",
        };
    }
}

