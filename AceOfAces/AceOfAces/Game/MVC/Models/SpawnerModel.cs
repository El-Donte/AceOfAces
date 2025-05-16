using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class SpawnerModel
{
    private readonly List<EnemyModel> _enemies = [];
    public List<EnemyModel> Enemies => _enemies;

    private float _gameTimer = 0f;
    public float GameTimer
    {
        get => _gameTimer;
        set
        {
            _gameTimer = value;

            if( _gameTimer > 10f && _enemiesPerSpawn < 5)
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
                _spawnTimer = 0;
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

    private float _spawnInterval = 3f;
    public float SpawnInterval
    {
        get => _spawnInterval;
        set => _spawnInterval = value;
    }

    private int _maxEnemies = 3;
    public int MaxEnemies
    {
        get => _maxEnemies;
        set => _maxEnemies = value;
    }

    public Action<EnemyModel> OnEnemySpawnedEvent { get; set; }

    public SpawnerModel() { }

    public void AddEnemy(Vector2 position)
    {
        var enemy = new EnemyModel(position);
        enemy.DestroyedEvent += OnEnemyDestroyed;
        _enemies.Add(enemy);
        OnEnemySpawnedEvent?.Invoke(enemy);
    }

    private void OnEnemyDestroyed(GameObjectModel enemy)
    {
        enemy.DestroyedEvent -= OnEnemyDestroyed;
        _enemies.Remove((EnemyModel)enemy);
    }
}

