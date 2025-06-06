using AceOfAces.Core;
using AceOfAces.Models;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class CollisionController : IController
{
    private readonly Grid _grid;
    
    private readonly float _collisionCooldownTime = 0.2f;
    private readonly Dictionary<GameObjectModel, float> _collisionCooldowns = [];

    public CollisionController(Grid grid)
    {
        _grid = grid;
    }

    public void Update(float deltaTime)
    {
        UpdateCooldowns(deltaTime);
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (var objA in _grid.ActiveObjects)
        {
            if (objA.IsDestroyed || !objA.Collider.IsEnable)
            {
                continue;
            }

            var nearbyObjects = _grid.GetNearbyObjects(objA.Position);

            foreach (var objB in nearbyObjects)
            {
                if (CanSkipCollisionCheck(objA, objB) || !objB.Collider.IsEnable)
                {
                    continue;
                }

                if (objA.Collider.Bounds.Intersects(objB.Collider.Bounds))
                {
                    HandleCollision(objA, objB);
                    StartCooldown(objA, objB);
                }
            }
        }
    }

    private bool CanSkipCollisionCheck(GameObjectModel objA, GameObjectModel objB)
    {
        return objB.IsDestroyed ||
               objA == objB ||
               !(!IsOnCooldown(objA) && !IsOnCooldown(objB));
    }

    private bool IsOnCooldown(GameObjectModel obj)
    {
        return _collisionCooldowns.TryGetValue(obj, out var cooldown) && cooldown > 0;
    }

    private void StartCooldown(GameObjectModel a, GameObjectModel b)
    {
        _collisionCooldowns[a] = _collisionCooldownTime;
        _collisionCooldowns[b] = _collisionCooldownTime;
    }

    private void UpdateCooldowns(float deltaTime)
    {
        foreach (var key in _collisionCooldowns.Keys)
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
        TryHandleCollision<MissileModel>(b, a, HandleMissileCollision);
        TryHandleCollision<MissileModel>(a, b, HandleMissileCollision);
        TryHandleCollision<PlayerModel>(a, b, HandlePlayerCollision);
    }

    private void TryHandleCollision<T>(GameObjectModel a, GameObjectModel b, Action<T, GameObjectModel> handler)
        where T : GameObjectModel
    {
        if (a is T typedA)
        {
            handler(typedA, b);
        }
    }

    private void HandlePlayerCollision(PlayerModel player, GameObjectModel other)
    {
        switch (other)
        {
            case MissileModel missile:
                HandleMissileCollision(missile, player);
                break;
        }
    }

    private void HandleMissileCollision(MissileModel missile, GameObjectModel other)
    {
        if (other is MissileModel || IsFriendlyFire(missile, other))
        {
            return;
        }

        switch (other)
        {
            case EnemyModel enemy:
                enemy.TakeDamage(missile.Damage);
                missile.Dispose();
                break;

            case PlayerModel player:
                player.TakeDamage(missile.Damage);
                missile.Dispose();
                break;
        }
    }

    private bool IsFriendlyFire(MissileModel missile, GameObjectModel other)
    {
        return (missile.Source == GameObjectType.Player && other is PlayerModel) ||
               (missile.Source == GameObjectType.Enemy && other is EnemyModel);
    }
}