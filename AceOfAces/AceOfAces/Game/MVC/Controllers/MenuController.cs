using AceOfAces.Models;

namespace AceOfAces.Controllers;

public class MenuController : IController
{
    private readonly MenuModel _model;

    public MenuController(MenuModel model)
    {
        _model = model;
        _model.PlayButton.Click += PlayButtonClicked;
        _model.QuitButton.Click += QuitButtonClicked;
    }

    private void QuitButtonClicked()
    {
        _model.Quit = true;
    }

    private void PlayButtonClicked()
    {
        _model.Play = true;
    }

    public void Update(float deltaTime)
    {
        _model.QuitButton.Update();
        _model.PlayButton.Update();
    }
}

