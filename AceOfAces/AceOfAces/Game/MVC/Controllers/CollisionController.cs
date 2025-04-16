using AceOfAces.Core;
using AceOfAces.Models;
using System.Collections.Generic;
using System;

namespace AceOfAces.Controllers;

public class CollisionController : IController
{
    private readonly List<GameObjectModel> _allObjects = new List<GameObjectModel>();
    private readonly Grid _grid;

    private const float COLLISION_COOLDOWN = 1f;
    private readonly Dictionary<GameObjectModel, float> _collisionCooldowns = new Dictionary<GameObjectModel, float>();

    private readonly PlayerModel _player;
    private readonly List<EnemyModel> _enemies;
    private readonly MissileListModel _missileList;

    public CollisionController(Grid grid, PlayerModel player,MissileListModel missiles, List<EnemyModel> enemies)
    {
        _grid = grid;
        _player = player;
        _missileList = missiles;
        _enemies = enemies;
    }

    public void Update(float deltaTime)
    {
        UpdateAllObjects();
        _grid.UpdateGridPosition(_player.Position);

        foreach (var obj in _allObjects)
        {
            UpdateCooldowns(deltaTime);
            if (!obj.IsDestroyed)
            {
                _grid.AddObject(obj);
            }
        }

        CheckAllCollisions();
    }


    private void UpdateAllObjects()
    {
        if (_allObjects != null)
        {
            _allObjects.Clear();
        }
        _allObjects.Add(_player);
        _allObjects.AddRange(_missileList.Missiles);
        _allObjects.AddRange(_enemies);
    }

    private void CheckAllCollisions()
    {
        for (int i = 0; i < _allObjects.Count; i++)
        {
            var objA = _allObjects[i];
            if (objA.IsDestroyed) continue;

            var nearbyObjects = _grid.GetNearbyObjects(objA.Position);

            foreach (var objB in nearbyObjects)
            {
                if (objB.IsDestroyed || objA == objB) continue;

                if (CanCollide(objA, objB) &&
                    objA.Collider.Bounds.Intersects(objB.Collider.Bounds))
                {
                    HandleCollision(objA, objB);
                    StartCooldown(objA);
                    StartCooldown(objB);
                }
            }
        }
    }

    private bool CanCollide(GameObjectModel a, GameObjectModel b)
    {
        return CanCollideWith(a) && CanCollideWith(b);
    }

    private bool CanCollideWith(GameObjectModel other)
    {
        if (_collisionCooldowns.TryGetValue(other, out var cooldown))
        {
            return cooldown <= 0;
        }
        return true;
    }

    private void StartCooldown(GameObjectModel other)
    {
        _collisionCooldowns[other] = COLLISION_COOLDOWN;
    }

    private void UpdateCooldowns(float deltaTime)
    {
        var keys = new List<GameObjectModel>(_collisionCooldowns.Keys);
        foreach (var key in keys)
        {
            _collisionCooldowns[key] -= deltaTime;
            if (_collisionCooldowns[key] <= 0)
            {
                _collisionCooldowns.Remove(key);
            }
        }
    }

    private void HandleCollision(GameObjectModel a, GameObjectModel b)
    {
        if (a is MissileModel missile)
        {
            HandleMissileCollision(missile, b);
        }
        else if (b is MissileModel missileB)
        {
            HandleMissileCollision(missileB, a);
        }

        if (a is PlayerModel player)
        {
            HandlePlayerCollision(player, b);
            return;
        }
        else if (b is PlayerModel playerB)
        {
            HandlePlayerCollision(playerB, a);
            return;
        }   
    }

    private void HandlePlayerCollision(PlayerModel player, GameObjectModel other)
    {
        switch (other.Collider.ObjectType)
        {
            case GameObjectType.Enemy:
                player.TakeDamage(10);
                Console.WriteLine(player.Health);
                break;
            case GameObjectType.Missile when other is MissileModel missile && missile.Source == GameObjectType.Enemy:
                player.TakeDamage(20);
                Console.WriteLine(player.Health);
                other.Dispose();
                break;
        }
    }

    private void HandleMissileCollision(MissileModel missile, GameObjectModel other)
    {
        if (other is MissileModel)
            return;

        bool isFriendlyFire = (missile.Source == GameObjectType.Player && other is PlayerModel) ||
                             (missile.Source == GameObjectType.Enemy && other is EnemyModel);

        if (isFriendlyFire)
            return;

        switch (other)
        {
            case EnemyModel enemy when missile.Source == GameObjectType.Player:
                enemy.TakeDamage(missile.Damage);
                missile.Dispose();
                break;

            case PlayerModel player when missile.Source == GameObjectType.Enemy:
                player.TakeDamage(missile.Damage);
                missile.Dispose();
                break;
        }
    }
}