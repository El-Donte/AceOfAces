using AceOfAces.Models;

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

    private void PlayAgainButtonClicked()
    {
        _model.PlayAgain = true;
    }

    private void QuitButtonClicked()
    {
        _model.Menu = true;
    }

    public void Update(float deltaTime)
    {
        _model.MenuButton.Update();
        _model.PlayAgainButton.Update();
    }
}