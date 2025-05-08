using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Controllers;

public class SpawnerController : IController
{
    private readonly SpawnerModel _spawner;
    private readonly GraphicsDeviceManager _graphics;
    private readonly Random _random = new Random();        

    public SpawnerController(SpawnerModel spawner, GraphicsDeviceManager graphics) 
    { 
        _spawner = spawner;
        _graphics = graphics;
    }

    private Vector2 GenerateSpawnPosition()
    {
        int screenWidth = _graphics.GraphicsDevice.Viewport.Width;
        int screenHeight = _graphics.GraphicsDevice.Viewport.Height;
        int side = _random.Next(0, 4);

        float x = 0;
        float y = 0;

        switch (side)
        {
            case 0:
                x = _random.Next(0, screenWidth);
                y = -100f;
                break;
            case 1:
                x = screenWidth + 100f;
                y = _random.Next(0, screenHeight);
                break;
            case 2:
                x = _random.Next(0, screenWidth);
                y = screenHeight + 100f;
                break;
            case 3: 
                x = -100f;
                y = _random.Next(0, screenHeight);
                break;
        }

        return new Vector2(x, y);
    }

    public void Update(float deltaTime)
    {
        _spawner.GameTimer += deltaTime;
        _spawner.SpawnTimer += deltaTime;

        if (_spawner.SpawnTimer >= _spawner.SpawnInterval)
        {
            SpawnEmemy();
        }
    }

    private void SpawnEmemy()
    {
        for (int i = 0; i < _spawner.EnemiesPerSpawn; i++)
        {
            if(_spawner.Enemies.Count >= _spawner.MaxEnemies) return;

            _spawner.AddEnemy(GenerateSpawnPosition());
        }
    }
}

