using AceOfAces.BehaiviourTree;
using AceOfAces.Core;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AceOfAces.Controllers;

public class EnemyController : IController
{
    private readonly List<EnemyModel> _enemies;
    private readonly PlayerModel _player;
    private readonly MissileListModel _missiles;
    private readonly Grid _grid;

    private readonly EnemyBehaiviourTreeBuilder _builder;
    private readonly Dictionary<EnemyModel, Node> _decisionTrees = [];

    private readonly Random _random = new();

    public EnemyController(Grid grid,SpawnerModel spawner, PlayerModel player, MissileListModel missiles)
    {
        _enemies = spawner.Enemies;
        _player = player;
        _missiles = missiles;
        _grid = grid;

        _builder = new EnemyBehaiviourTreeBuilder();

        _builder.UpdateTargetPosition = new ActionNode((enemy, dt) => UpdateTargetPosition(enemy));
        _builder.Move = new ActionNode((enemy, dt) => Move(enemy, dt));
        _builder.CheckEvasion = new ConditionNode((enemy) => IsEvading(enemy));
        _builder.GenerateNewTarget = new ActionNode((enemy, dt) => GenerateNewTarget(enemy));
        _builder.Fire = new ActionNode((enemy, dt) => Fire(enemy));
        _builder.IsInFieldOfView = new ConditionNode((enemy) => IsPlayerInFieldOfView(enemy));
        _builder.IsPursuing = new ConditionNode((enemy) => IsPursuing(enemy));
        _builder.EvasionMove = new ActionNode((enemy, dt) => EvasionMove(enemy, dt));

        spawner.OnEnemySpawnedEvent += CreateNewBehaiviourTree;
    }

    public void Update(float deltaTime)
    {
        ResolveOverlaps();

        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];

            enemy.FireDelayTimer -= deltaTime;

            _decisionTrees[enemy].Evaluate(enemy, deltaTime);

        }
    }

    private void ResolveOverlaps()
    {
        float minDistance = 80f;
        float pushStrength = 0.3f;

        for (int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            var nearbyObjects = _grid.GetNearbyObjects(enemy.Position);
            for (int j = i + 1; j < nearbyObjects.Count; j++)
            {
                if (nearbyObjects[j] is EnemyModel other)
                {
                    Vector2 delta = other.Position - enemy.Position;
                    float distance = delta.Length();

                    if (distance < minDistance && distance > 0.001f)
                    {
                        Vector2 correction = Vector2.Normalize(delta) * (minDistance - distance) * pushStrength;
                        enemy.Position -= correction * 0.7f;
                        other.Position += correction * 0.7f;
                    }
                }
            }
        }
    }

    private bool IsPursuing(EnemyModel enemy)
    {
        float distance = Vector2.Distance(enemy.Position, _player.Position);
        enemy.IsPursuingPlayer = distance < enemy.PursuitRadius;
        return enemy.IsPursuingPlayer;
    }

    private void CreateNewBehaiviourTree(EnemyModel enemy)
    {
        _missiles.AddCooldowns(enemy.Cooldowns);
        _decisionTrees.Add(enemy, _builder.CreateBehaiviourTree());
        enemy.DestroyedEvent += OnEnemyDestroyed;
    }

    private void UpdateTargetPosition(EnemyModel enemy)
    {
        if (enemy.IsPursuingPlayer)
        {
            enemy.TargetPosition = _player.Position + _player.Velocity * 1.2f;
        }
        else
        {
            GenerateNewTarget(enemy);
        }
    }

    private void GenerateNewTarget(EnemyModel enemy)
    {
        float angle = (float)(_random.NextDouble() * Math.PI);
        float radius = (float)Math.Sqrt(_random.NextDouble()) * 50f;
        var randomTargetPos = new Vector2((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);
        enemy.TargetPosition = _player.Position + randomTargetPos;
    }

    private float GetAngleDiff(EnemyModel enemy)
    {
        Vector2 toTarget = enemy.TargetPosition - enemy.Position;
        Vector2 direction = Vector2.Normalize(toTarget);
        float targetAngle = (float)Math.Atan2(direction.Y, direction.X);
        return MathHelper.WrapAngle(targetAngle - enemy.Rotation);
    }

    private void Move(EnemyModel enemy, float deltaTime)
    {
        float angleDiff = GetAngleDiff(enemy);
        float deltaTurn = enemy.RotationSpeed * deltaTime;
        
        enemy.Rotation += MathHelper.Clamp(angleDiff, -deltaTurn, deltaTurn);

        Vector2 desiredDirection = new Vector2(
            (float)Math.Cos(enemy.Rotation),
            (float)Math.Sin(enemy.Rotation)
        );

        enemy.Velocity = desiredDirection * enemy.Speed;
        enemy.Position += enemy.Velocity * deltaTime;
    }

    private void EvasionMove(EnemyModel enemy, float deltaTime)
    {
        float angleDiff = GetAngleDiff(enemy);

        enemy.Speed *= enemy.EvasionKoeff;
        enemy.Rotation += enemy.RotationSpeed * enemy.EvasionKoeff * deltaTime * -Math.Sign(angleDiff);

        Vector2 desiredDirection = new Vector2(
            (float)Math.Cos(enemy.Rotation),
            (float)Math.Sin(enemy.Rotation)
        );

        enemy.Velocity = desiredDirection * enemy.Speed;
        enemy.Position += enemy.Velocity * deltaTime;
    }

    private bool IsEvading(EnemyModel enemy)
    {
        float distanceToPlayer = Vector2.Distance(enemy.Position, _player.Position);
        enemy.IsEvading = distanceToPlayer < enemy.EvasionRadius;
        return enemy.IsEvading;
    }

    private void Fire(EnemyModel enemy)
    {
        var availableCooldown = enemy.Cooldowns.FirstOrDefault(c => c.AvailableToFire);
        if (availableCooldown == null || !enemy.CanFire
            || (enemy.FiredMissileCount >= enemy.MaxMissileCount))
        {
            return;
        }

        _missiles.CreateMissile(enemy.Position + enemy.MissileJointPosition, enemy, _player);
        enemy.FiredMissileCount++;
        enemy.FireDelayTimer = enemy.ResetRandomFireDelay();
    }

    private bool IsPlayerInFieldOfView(EnemyModel enemy)
    {
        Vector2 toPlayer = _player.Position - enemy.Position;
        if (toPlayer.LengthSquared() == 0)
        {
            return false;
        }

        toPlayer.Normalize();

        Vector2 forward = new(
            (float)Math.Cos(enemy.Rotation),
            (float)Math.Sin(enemy.Rotation)
        );

        float angleToPlayer = (float)Math.Acos(Vector2.Dot(forward, toPlayer));

        return Math.Abs(angleToPlayer) <= enemy.FieldOfViewAngle / 2f;
    }

    private void OnEnemyDestroyed(GameObjectModel obj)
    {
        var enemy = obj as EnemyModel;

        enemy.DestroyedEvent -= OnEnemyDestroyed;
        if (enemy.IsTargeted) 
        { 
            GameEvents.ChangeTarget((enemy.Id + 1) % _enemies.Count);
            enemy.IsTargeted = false;
        }

        _missiles.RemoveCooldowns(enemy.Cooldowns);
        _enemies.Remove(enemy);
        _decisionTrees.Remove(enemy);
    }
}