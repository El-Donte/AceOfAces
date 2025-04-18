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
    private readonly List<IView> _views = new List<IView>();
    private readonly List<IController> _controllers = new List<IController>();
    
    private readonly SpriteBatch _spriteBatch;
    private readonly Camera camera;
    private readonly Grid _grid;

    private readonly List<ColliderModel> _colliders = new List<ColliderModel>();
    private PlayerModel _playerModel;
    private MissileListModel missles;

    public static bool isDebug;

    public GameManager(GraphicsDeviceManager graphics, ContentManager contentManager)
    {
        _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

        _grid = new Grid(64, graphics);
        camera = new Camera(graphics.GraphicsDevice.Viewport);

        DebugDraw.Initialize(graphics.GraphicsDevice);

        #region Models
        _playerModel = new PlayerModel(contentManager.Load<Texture2D>("jets/jet"), 
            new(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2));

        _playerModel.PositionChangedEvent += camera.SetPosition;

        var enemyModel = new EnemyModel(contentManager.Load<Texture2D>("jets/jet"), Vector2.Zero);

        var layers = new List<BackgroundModel> {
            new BackgroundModel(contentManager.Load<Texture2D>("Clouds/Clouds1"), 0.6f, 0.5f, graphics.GraphicsDevice.Viewport),
            new BackgroundModel(contentManager.Load<Texture2D>("Clouds/Clouds2"), 0.8f, 1f, graphics.GraphicsDevice.Viewport),
            new BackgroundModel(contentManager.Load<Texture2D>("Clouds/Clouds3"), 1.1f, 1.4f, graphics.GraphicsDevice.Viewport)
        };

        MissileModel.MissleTexture = contentManager.Load<Texture2D>("missile");

        var missileCooldownTexture = contentManager.Load<Texture2D>("missileCooldown");
        //var font = _contentManager.Load<SpriteFont>("Fonts/font");

        var cooldowns = new List<MissileCooldownModel>();
        for (int i = 0; i < 2; i++)
        {
            cooldowns.Add(new MissileCooldownModel(3f));
        }

        missles = new MissileListModel(cooldowns);

        _colliders.Add(_playerModel.Collider);
        _colliders.Add(enemyModel.Collider);
        #endregion

        #region Controllers
        _controllers.Add(new PlayerController(_playerModel, missles, enemyModel));
        _controllers.Add(new BackGroundController(layers, _playerModel));
        _controllers.Add(new MissileController(missles));
        _controllers.Add(new CollisionController(_grid, _playerModel, missles, new List<EnemyModel>() { enemyModel }));
        #endregion

        #region Views
        _views.Add(new BackgroundView(layers,_playerModel) { SpriteBatch = _spriteBatch });
        _views.Add(new EnemyView(enemyModel) { SpriteBatch = _spriteBatch });
        _views.Add(new PlayerView(_playerModel, MissileModel.MissleTexture, cooldowns) {SpriteBatch = _spriteBatch});
        _views.Add(new MissilesView(missles) { SpriteBatch = _spriteBatch });
 
        for (int i = 0; i < missles.Cooldowns.Count; i++)
        {
            var screenMargin = new Vector2(20 + 60 * i, 50);

            var bar  = new CooldownBarView(model: missles.Cooldowns[i],texture: missileCooldownTexture, screenMargin: screenMargin)
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

        if (isDebug)
        {
            foreach (var collider in _colliders)
            {
                DebugDraw.DrawRectangle(_spriteBatch, collider.Bounds, Color.Red);
            }

            for (int i = 0; i < missles.Missiles.Count; i++)
            {
                DebugDraw.DrawRectangle(_spriteBatch, missles.Missiles[i].Collider.Bounds, Color.Blue);
            }

            DebugDraw.DrawGrid(_spriteBatch, _grid);
        }

        _spriteBatch.End();
    }

}

