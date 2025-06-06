using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class SpawnerModel
{
    private readonly List<EnemyModel> _enemies = [];
    public List<EnemyModel> Enemies => _enemies;

    private Vector2 _position;
    public Vector2 Position => _position;

    private float _gameTimer = 0f;
    public float GameTimer
    {
        get => _gameTimer;
        set
        {
            _gameTimer = value;

            if( _gameTimer > 40f && _enemiesPerSpawn < 5)
            {
                _gameTimer = 0;
                _enemiesPerSpawn++;
            }
        }
    }

    private float _spawnTimer = 0f;
    public float SpawnTimer
    {
        get => _spawnTimer;
        set
        {
            if(_spawnTimer >= _spawnInterval)
            {
                value = 0;
            }

            _spawnTimer = value;
        }
    }

    private int _enemiesPerSpawn = 1;
    public int EnemiesPerSpawn
    {
        get => _enemiesPerSpawn;
        set => _enemiesPerSpawn = value;
    }

    private float _spawnInterval = 10f;
    public float SpawnInterval
    {
        get => _spawnInterval;
        set => _spawnInterval = value;
    }

    private int _maxEnemies = 8;
    public int MaxEnemies
    {
        get => _maxEnemies;
        set => _maxEnemies = value;
    }

    public Action<EnemyModel> OnEnemySpawnedEvent { get; set; }

    public SpawnerModel() { }

    public void AddEnemy(Vector2 position)
    {
        var enemy = new EnemyModel(_enemies.Count, position);

        if(_enemies.Count == 0)
        {
            enemy.IsTargeted = true;
        }

        _enemies.Add(enemy);
        OnEnemySpawnedEvent?.Invoke(enemy);
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }
}

