using AceOfAces.Models;
using System;

namespace AceOfAces.Controllers;

public class GameOverController : IController
{
    private readonly GameOverModel _model;

    public GameOverController(GameOverModel model) 
    { 
        _model = model;
        _model.MenuButton.Click += QuitButtonClicked;
        _model.PlayAgainButton.Click += PlayAgainButtonClicked;
    }

    private void PlayAgainButtonClicked(object sender, EventArgs e)
    {
        _model.PlayAgain = true;
    }

    private void QuitButtonClicked(object sender, EventArgs e)
    {
        _model.Menu = true;
    }

    public void Update(float deltaTime)
    {
        _model.MenuButton.Update();
        _model.PlayAgainButton.Update();
    }
}

