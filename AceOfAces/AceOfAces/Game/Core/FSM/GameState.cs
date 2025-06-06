using AceOfAces.Controllers;
using AceOfAces.Core.Particles;
using AceOfAces.Managers;
using AceOfAces.Models;
using AceOfAces.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace AceOfAces.Core.FSM;

public class GameState : BaseState
{
    private readonly List<IView> _views = [];
    private readonly List<IController> _controllers = [];
    
    private readonly SpriteBatch _spriteBatch;
    private readonly Camera _camera;
    private readonly Grid _grid;

    public static bool IsDebugMode { get; set; } = false;

    public GameState(StateMachine stateMachine)
        : base(stateMachine)
    {
        var graphics = StateMachine.GameEngine.GraphicsDevice;
        _spriteBatch = new SpriteBatch(graphics);
        _grid = new Grid(64, graphics);
        _camera = new Camera(graphics.Viewport);
    }

    public override void Update(float deltaTime)
    {
        for(int i = 0; i < _controllers.Count; i++)
        {
            var controller = _controllers[i];
            controller.Update(deltaTime);
        }
    }

    public override void Draw()
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, transformMatrix: _camera.TransformMatrix);

        for(int i = 0; i < _views.Count; i++)
        {
            var view = _views[i];
            view.Draw();
        }

        _spriteBatch.End();
    }

    public override void Enter()
    {
        _camera.SetPosition(new Vector2(
            StateMachine.GameEngine.GraphicsDevice.Viewport.Width / 2,
            StateMachine.GameEngine.GraphicsDevice.Viewport.Height / 2));
        _grid.Clear();
        ParticleEmitter.Initialize();

        var player = CreatePlayer();
        var spawner = CreateSpawner(player);
        var missiles = new MissileListModel();
        var layers = CreateBackgroundLayers();

        RegisterControllers(player, missiles, spawner, layers);
        RegisterViews(player, missiles, spawner, layers);
    }

    private PlayerModel CreatePlayer()
    {
        var startPosition = new Vector2(
            StateMachine.GameEngine.GraphicsDevice.Viewport.Width / 2,
            StateMachine.GameEngine.GraphicsDevice.Viewport.Height / 2);

        var player = new PlayerModel(startPosition);

        player.PositionChangedEvent += _camera.SetPosition;
        player.PositionChangedEvent += _grid.UpdateGridPosition;
        player.PlayerDeadEvent += OnGameOver;

        GameEvents.ChangeTragetEvent += player.SetTargetIndex;

        return player;
    }

    private List<LayerModel> CreateBackgroundLayers()
    {
        return new List<LayerModel>
        {
            new(AssetsManager.CloudTextures[0], 0.6f, 0.5f, StateMachine.GameEngine.GraphicsDevice.Viewport),
            new(AssetsManager.CloudTextures[1], 0.8f, 1f, StateMachine.GameEngine.GraphicsDevice.Viewport),
            new(AssetsManager.CloudTextures[2], 1.1f, 1.4f, StateMachine.GameEngine.GraphicsDevice.Viewport)
        };
    }

    private SpawnerModel CreateSpawner(PlayerModel player)
    {
        var spawner = new SpawnerModel();
        player.PositionChangedEvent += spawner.SetPosition;
        return spawner;
    }

    private void RegisterControllers(PlayerModel player, MissileListModel missiles, SpawnerModel spawner, List<LayerModel> layers)
    {
        var graphics = StateMachine.GameEngine.GraphicsDevice;

        _controllers.Add(new ParticleContorller());
        _controllers.Add(new PlayerController(player, missiles, spawner));
        _controllers.Add(new SpawnerController(spawner, graphics));
        _controllers.Add(new EnemyController(_grid, spawner, player, missiles));
        _controllers.Add(new MissileController(missiles));
        _controllers.Add(new CollisionController(_grid));
        _controllers.Add(new GridController(_grid, player, missiles, spawner));
        _controllers.Add(new BackGroundController(layers, player));
    }

    private void RegisterViews(PlayerModel player, MissileListModel missiles, SpawnerModel spawner, List<LayerModel> layers)
    {
        _views.Add(new BackgroundView(layers, player, _spriteBatch));
        _views.Add(new ParticleView(_spriteBatch));
        _views.Add(new PlayerView(player, _spriteBatch));
        _views.Add(new EnemyView(spawner, _spriteBatch));
        _views.Add(new MissilesView(missiles, _spriteBatch));
        _views.Add(new DebugView(_grid, player, spawner, missiles, _spriteBatch));

        for (int i = 0; i < player.Cooldowns.Count; i++)
        {
            var screenMargin = new Vector2(20 + 60 * i, 50);
            var bar = new CooldownBarView(missiles.Cooldowns[i], screenMargin, _spriteBatch,
                StateMachine.GameEngine.GraphicsDevice.Viewport, _camera);
            _views.Add(bar);
        }
    }

    public override void Exit()
    {
        _views.Clear();
        _controllers.Clear();
    }

    private void OnGameOver() => StateMachine.Change("GameOver");
}