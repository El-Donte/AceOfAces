using AceOfAces.Controllers;
using AceOfAces.Models;
using AceOfAces.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core.FSM;

public class MenuState : BaseMenuState
{
    private readonly MenuModel _model;
    private readonly MenuView _view;
    private readonly MenuController _controller;


    public MenuState(StateMachine stateMachine) : base(stateMachine)
    {
        _model = new MenuModel();
        _view = new MenuView(_model, _spriteBatch);
        _controller = new MenuController(_model);
    }

    public override void Enter()
    {
        _model.Quit = false;
        _model.Play = false;
    }

    public override void Update(float deltaTime)
    {
        _controller.Update(deltaTime);

        if (_model.Play)
        {
            StateMachine.Change("Game");
        }

        if (_model.Quit)
        {
            StateMachine.GameEngine.Exit();
        }
    }

    public override void Draw()
    {
        DrawToRenderTarget(_view.Draw);
    }

    public override void Exit()
    {

    }
}