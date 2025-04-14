using AceOfAces.Core;
using AceOfAces.Controllers;
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
    private readonly List<IView> _views = new List<IView>();
    private readonly List<IController> _controllers = new List<IController>();
    private readonly List<GameObjectModel> _models = new List<GameObjectModel>();

    private readonly Camera2d camera2D;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphicsManager;
    private readonly ContentManager _contentManager;
    private readonly Viewport _viewport;
    private readonly Grid _grid;

    private PlayerModel _playerModel;
    private EnemyModel enemyModel;
    private MissileList missles;
    public static bool isDebug;

    public GameManager(GraphicsDeviceManager graphics, ContentManager content)
    {
        _graphicsManager = graphics;
        _contentManager = content;
        _spriteBatch = new SpriteBatch(_graphicsManager.GraphicsDevice);
        _viewport = _graphicsManager.GraphicsDevice.Viewport;

        _grid = new Grid(64,_graphicsManager);
        camera2D = new Camera2d(_graphicsManager.GraphicsDevice.Viewport);
        DebugDraw.Initialize(_graphicsManager.GraphicsDevice);

        #region Models
        _playerModel = new PlayerModel(_contentManager.Load<Texture2D>("jets/jet"), new(_viewport.Width / 2, _viewport.Height / 2));
        enemyModel = new EnemyModel(_contentManager.Load<Texture2D>("jets/jet"), Vector2.Zero);
        var layers = new List<BackgroundModel> {
            new BackgroundModel(_contentManager.Load<Texture2D>($"Clouds/Clouds1"), 0.6f, 0.5f, _viewport),
            new BackgroundModel(_contentManager.Load<Texture2D>($"Clouds/Clouds2"), 0.8f, 1f, _viewport),
            new BackgroundModel(_contentManager.Load<Texture2D>($"Clouds/Clouds3"), 1.1f, 1.4f, _viewport)
        };

        missles = new MissileList(_contentManager.Load<Texture2D>("missile"),_playerModel);
        _models.Add(_playerModel);
        _models.Add(enemyModel);
        #endregion

        #region Controllers
        var _collisionController = new CollisionController(_grid,_playerModel,missles,new List<EnemyModel>() { enemyModel });

        _controllers.Add(new PlayerController(_playerModel, missles));
        _controllers.Add(new BackGroundController(layers, _playerModel));
        _controllers.Add(new MissileController(missles));
        _controllers.Add(_collisionController);
        #endregion

        #region Views
        _views.Add(new BackgroundView(layers,_playerModel) { SpriteBatch = _spriteBatch });
        _views.Add(new EnemyView(enemyModel) { SpriteBatch = _spriteBatch });
        _views.Add(new PlayerView(_playerModel) {SpriteBatch = _spriteBatch});
        _views.Add(new MissilesView(missles) { SpriteBatch = _spriteBatch });
        #endregion
    }

    public void Update(GameTime gt)
    {
        float gameTime = (float)gt.ElapsedGameTime.TotalSeconds;

        foreach (var controller in _controllers)
        {
            controller.Update(gameTime);
            enemyModel.Move(new Vector2(1, 0), gameTime);
        }

        camera2D.Position = _playerModel.Position;
    }

    public void Draw()
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, transformMatrix: camera2D.Transform);

        foreach (var view in _views)
        {
            view.Draw();
        }

        if (isDebug)
        {
            foreach (var model in _models)
            {
                DebugDraw.DrawRectangle(_spriteBatch, model.Collider.Bounds, Color.Red);
            }

            for(int i = 0; i < missles.Missiles.Count; i++)
            {
                DebugDraw.DrawRectangle(_spriteBatch, missles.Missiles[i].Collider.Bounds, Color.Blue);
            }

            DebugDraw.DrawGrid(_spriteBatch, _grid);
        }

        _spriteBatch.End();
    }
}

