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
    private readonly Camera _camera;
    private readonly Grid _grid;

    public static bool IsDebugMode { get; set; } = false;

    public GameManager(GraphicsDeviceManager graphics, ContentManager contentManager)
    {
        _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

        _grid = new Grid(64, graphics);
        _camera = new Camera(graphics.GraphicsDevice.Viewport);

        AssetsManager.ContentManager = contentManager;
        AssetsManager.Graphics = graphics.GraphicsDevice;
        AssetsManager.LoadContent();

        #region Models
        var missles = new MissileListModel();
        var startPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

        var playerModel = new PlayerModel(startPos);
        playerModel.PositionChangedEvent += _camera.SetPosition;
        playerModel.PositionChangedEvent += _grid.UpdateGridPosition;

        var spawner = new SpawnerModel();

        var layers = new List<LayerModel> {
            new(AssetsManager.CloudTextures[0], 0.6f, 0.5f, graphics.GraphicsDevice.Viewport),
            new(AssetsManager.CloudTextures[1], 0.8f, 1f, graphics.GraphicsDevice.Viewport),
            new(AssetsManager.CloudTextures[2], 1.1f, 1.4f, graphics.GraphicsDevice.Viewport)
        };
        #endregion

        #region Controllers
        _controllers.Add(new PlayerController(playerModel, missles, spawner));
        _controllers.Add(new SpawnerController(spawner,graphics));
        _controllers.Add(new EnemyController(spawner, playerModel, missles));
        _controllers.Add(new MissileController(missles));
        _controllers.Add(new CollisionController(_grid));
        _controllers.Add(new GridController(_grid, playerModel, missles, spawner));
        _controllers.Add(new BackGroundController(layers, playerModel));
        #endregion

        #region Views
        _views.Add(new BackgroundView(layers, playerModel, _spriteBatch));
        _views.Add(new PlayerView(playerModel,_spriteBatch));
        _views.Add(new EnemyView(spawner, _spriteBatch));
        _views.Add(new MissilesView(missles,_spriteBatch));
        _views.Add(new DebugView(_grid, playerModel, spawner, missles, _spriteBatch));
 
        for (int i = 0; i < playerModel.Cooldowns.Count; i++)
        {
            var screenMargin = new Vector2(20 + 60 * i, 50);

            var bar  = new CooldownBarView(missles.Cooldowns[i], screenMargin, _spriteBatch)
            { Camera = _camera, GraphicsDevice = graphics.GraphicsDevice };

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
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, transformMatrix: _camera.TransformMatrix);

        foreach (var view in _views)
        {
            view.Draw();
        }

        _spriteBatch.End();
    }
}

