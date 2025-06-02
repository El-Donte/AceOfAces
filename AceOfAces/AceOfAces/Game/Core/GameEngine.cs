using AceOfAces.Core.FSM;
using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AceOfAces.Core;

public class GameEngine : Game
{
    private readonly GraphicsDeviceManager _graphics;

    public GameEngine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();

        AssetsManager.Initialize(Content, _graphics.GraphicsDevice);
        AssetsManager.LoadContent();

        var stateComponent = new StateComponent(this);
        var gameState = new GameState(stateComponent.StateMachine);
        var gameOverState = new GameOverState(stateComponent.StateMachine);
        var menuState = new MenuState(stateComponent.StateMachine);

        stateComponent.StateMachine.Add("Menu", menuState);
        stateComponent.StateMachine.Add("Game", gameState);
        stateComponent.StateMachine.Add("GameOver", gameOverState);

        stateComponent.StateMachine.Change("Menu");

        Components.Add(stateComponent);

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}