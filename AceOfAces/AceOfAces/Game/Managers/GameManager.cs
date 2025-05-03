using AceOfAces.Controllers;
using AceOfAces.Core;
using AceOfAces.Models;
using AceOfAces.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace AceOfAces.Managers;

public class GameManager
{
    private readonly List<IView> _views = [];
    private readonly List<IController> _controllers = [];
    
    private readonly SpriteBatch _spriteBatch;
    private readonly Camera camera;
    private readonly Grid _grid;

    public static bool IsDebugMode { get; set; } = false;

    public GameManager(GraphicsDeviceManager graphics, ContentManager contentManager)
    {
        _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

        _grid = new Grid(64, graphics);
        camera = new Camera(graphics.GraphicsDevice.Viewport);

        #region Models
        var missles = new MissileListModel();
        var startPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

        var playerModel = new PlayerModel(contentManager.Load<Texture2D>("jets/jet"), startPos);

        playerModel.PositionChangedEvent += camera.SetPosition;
        playerModel.PositionChangedEvent += _grid.UpdateGridPosition;

        var enemyModel = new EnemyModel(contentManager.Load<Texture2D>("jets/jet"), startPos + new Vector2(-500, -500));
        var enemyModel2 = new EnemyModel(contentManager.Load<Texture2D>("jets/jet"), startPos + new Vector2(500, 500));
        var listEnemies = new List<EnemyModel>() { enemyModel, enemyModel2 };

        var layers = new List<LayerModel> {
            new(contentManager.Load<Texture2D>("Clouds/Clouds1"), 0.6f, 0.5f, graphics.GraphicsDevice.Viewport),
            new(contentManager.Load<Texture2D>("Clouds/Clouds2"), 0.8f, 1f, graphics.GraphicsDevice.Viewport),
            new(contentManager.Load<Texture2D>("Clouds/Clouds3"), 1.1f, 1.4f, graphics.GraphicsDevice.Viewport)
        };
        #endregion

        #region Controllers
        _controllers.Add(new PlayerController(playerModel, missles, listEnemies));
        _controllers.Add(new EnemyController(listEnemies, playerModel, missles));
        _controllers.Add(new MissileController(missles));
        _controllers.Add(new CollisionController(_grid));
        _controllers.Add(new GridController(_grid, playerModel, missles,listEnemies));
        _controllers.Add(new BackGroundController(layers, playerModel));
        #endregion

        #region Views
        var missileCooldownTexture = contentManager.Load<Texture2D>("missileCooldown");
        var missileTexture = contentManager.Load<Texture2D>("missile");
        MissileModel.SetTerxture(missileTexture);

        _views.Add(new BackgroundView(layers, playerModel) { SpriteBatch = _spriteBatch });
        _views.Add(new PlayerView(playerModel, missileTexture) {SpriteBatch = _spriteBatch});
        _views.Add(new EnemyView(listEnemies, missileTexture) { SpriteBatch = _spriteBatch });
        _views.Add(new MissilesView(missles) { SpriteBatch = _spriteBatch });
        _views.Add(new DebugView(graphics, _grid, playerModel, listEnemies, missles) { SpriteBatch = _spriteBatch });
 
        for (int i = 0; i < playerModel.Cooldowns.Count; i++)
        {
            var screenMargin = new Vector2(20 + 60 * i, 50);

            var bar  = new CooldownBarView(missles.Cooldowns[i], missileCooldownTexture, screenMargin)
            { Camera = camera, GraphicsDevice = graphics.GraphicsDevice, SpriteBatch = _spriteBatch };

            _views.Add(bar);
        }
        #endregion
    }

    public void Update(GameTime gt)
    {
        float gameTime = (float)gt.ElapsedGameTime.TotalSeconds;

        foreach (var controller in _controllers)
        {
            controller.Update(gameTime);
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, transformMatrix: camera.TransformMatrix);

        foreach (var view in _views)
        {
            view.Draw();
        }

        _spriteBatch.End();
    }
}

