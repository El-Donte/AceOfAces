using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;

namespace AceOfAces.Controllers;

public class SpawnerController : IController
{
    private readonly SpawnerModel _spawner;
    private readonly GraphicsDevice _graphics;
    private readonly Random _random = new ();        

    public SpawnerController(SpawnerModel spawner, GraphicsDevice graphics) 
    { 
        _spawner = spawner;
        _graphics = graphics;
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
            if (_spawner.Enemies.Count >= _spawner.MaxEnemies)
            {
                return;
            }

            _spawner.AddEnemy(GenerateSpawnPosition());
        }
    }

    private Vector2 GenerateSpawnPosition()
    {
        var viewport = _graphics.Viewport;
        int screenWidth = viewport.Width;
        int screenHeight = viewport.Height;

        float buffer = 100f;
        float centerX = _spawner.Position.X;
        float centerY = _spawner.Position.Y;

        float x = 0, y = 0;
        int side = _random.Next(0, 4);

        switch (side)
        {
            case 0: // Верх
                x = _random.Next((int)(centerX - screenWidth / 2), (int)(centerX + screenWidth / 2));
                y = centerY - screenHeight / 2 - buffer;
                break;

            case 1: // Право
                x = centerX + screenWidth / 2 + buffer;
                y = _random.Next((int)(centerY - screenHeight / 2), (int)(centerY + screenHeight / 2));
                break;

            case 2: // Низ
                x = _random.Next((int)(centerX - screenWidth / 2), (int)(centerX + screenWidth / 2));
                y = centerY + screenHeight / 2 + buffer;
                break;

            case 3: // Лево
                x = centerX - screenWidth / 2 - buffer;
                y = _random.Next((int)(centerY - screenHeight / 2), (int)(centerY + screenHeight / 2));
                break;
        }

        return new Vector2(x, y);
    }
}


