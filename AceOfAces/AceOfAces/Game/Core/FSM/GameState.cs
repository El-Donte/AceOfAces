﻿using AceOfAces.Controllers;
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
    private readonly StateMachine _stateMachine;
    private PlayerModel _playerModel;

    public static bool IsDebugMode { get; set; } = false;

    public GameState(StateMachine stateMachine)
        : base(stateMachine)
    {
        var graphics = stateMachine.GameEngine.GraphicsDevice;
        _spriteBatch = new SpriteBatch(graphics);
        _grid = new Grid(64, graphics);
        _camera = new Camera(graphics.Viewport);
        _stateMachine = stateMachine;
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

    public override void Enter(params object[] args)
    {
        var graphics = _stateMachine.GameEngine.GraphicsDevice;

        ParticleEmitter particleEmitter = new ParticleEmitter();
        
        GameEvents.OnGameOverEvent += OnGameOver;

        #region Models
        var missles = new MissileListModel();
        var startPos = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);

        _playerModel = new PlayerModel(startPos);
        _playerModel.PositionChangedEvent += _camera.SetPosition;
        _playerModel.PositionChangedEvent += _grid.UpdateGridPosition;

        var spawner = new SpawnerModel();

        var layers = new List<LayerModel> {
            new(AssetsManager.CloudTextures[0], 0.6f, 0.5f, graphics.Viewport),
            new(AssetsManager.CloudTextures[1], 0.8f, 1f, graphics.Viewport),
            new(AssetsManager.CloudTextures[2], 1.1f, 1.4f, graphics.Viewport)
        };
        #endregion

        #region Controllers
        _controllers.Add(new ParticleContorller());
        _controllers.Add(new PlayerController(_playerModel, missles, spawner));
        _controllers.Add(new SpawnerController(spawner, graphics));
        _controllers.Add(new EnemyController(spawner, _playerModel, missles));
        _controllers.Add(new MissileController(missles));
        _controllers.Add(new CollisionController(_grid));
        _controllers.Add(new GridController(_grid, _playerModel, missles, spawner));
        _controllers.Add(new BackGroundController(layers, _playerModel));
        #endregion

        #region Views
        _views.Add(new BackgroundView(layers, _playerModel, _spriteBatch));
        _views.Add(new ParticleView(_spriteBatch));
        _views.Add(new PlayerView(_playerModel, _spriteBatch));
        _views.Add(new EnemyView(spawner, _spriteBatch));
        _views.Add(new MissilesView(missles, _spriteBatch));
        _views.Add(new DebugView(_grid, _playerModel, spawner, missles, _spriteBatch));

        for (int i = 0; i < _playerModel.Cooldowns.Count; i++)
        {
            var screenMargin = new Vector2(20 + 60 * i, 50);

            var bar = new CooldownBarView(missles.Cooldowns[i], screenMargin, _spriteBatch)
            { Camera = _camera, GraphicsDevice = graphics };

            _views.Add(bar);
        }
        #endregion
    }

    public override void Exit()
    {
        _playerModel.Dispose();
        _views.Clear();
        _controllers.Clear();
        GameEvents.OnGameOverEvent -= OnGameOver;
    }

    private void OnGameOver() => _stateMachine.Change("GameOver");
}

