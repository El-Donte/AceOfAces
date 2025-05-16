using AceOfAces.BehaiviourTree;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class EnemyController : IController
{
    private readonly List<EnemyModel> _enemies;
    private readonly PlayerModel _player;
    private readonly MissileListModel _missiles;

    private readonly EnemyBehaiviourTreeBuilder _builder;
    private readonly Dictionary<EnemyModel, Node> _decisionTrees = [];
    private readonly Random _random = new();

    public EnemyController(SpawnerModel spawner, PlayerModel player, MissileListModel missiles)
    {
        _enemies = spawner.Enemies;
        _player = player;
        _missiles = missiles;

        var updateTargetAction = new ActionNode((enemy, dt) => UpdateTarget(enemy));
        var moveAction = new ActionNode((enemy, dt) => Move(enemy, dt));
        var checkEvasionAction = new ActionNode((enemy, dt) => CheckEvasion(enemy, dt));
        var generateNewTargetAction = new ActionNode((enemy, dt) => GenerateNewTarget(enemy));
        var fireAction = new ActionNode((enemy, dt) => Fire(enemy));

        _builder = new EnemyBehaiviourTreeBuilder(player, updateTargetAction, moveAction, 
                    checkEvasionAction, generateNewTargetAction, fireAction);

        spawner.OnEnemySpawnedEvent += CreateNewBehaiviourTree;
    }

    public void Update(float deltaTime)
    {
        for(int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            enemy.FireDelayTimer -= deltaTime;
            _decisionTrees[enemy].Evaluate(enemy, deltaTime);
        }
    }

    private void CreateNewBehaiviourTree(EnemyModel enemy)
    {
        _missiles.AddCooldowns(enemy.Cooldowns);
        _decisionTrees.Add(enemy, _builder.CreateBehaiviourTree());
        enemy.DestroyedEvent += OnEnemyDestroyed;
    }

    private void OnEnemyDestroyed(GameObjectModel enemy)
    {
        ((EnemyModel)enemy).IsTargeted = false;
        _missiles.RemoveCooldowns(((EnemyModel)enemy).Cooldowns);
        _decisionTrees.Remove((EnemyModel)enemy);
    }

    private void UpdateTarget(EnemyModel enemy)
    {
        float distanceToPlayer = Vector2.Distance(enemy.Position, _player.Position);
        enemy.IsPursuingPlayer = distanceToPlayer < enemy.PursuitRadius;

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
        float radius = (float)Math.Sqrt(_random.NextDouble()) * 150f;
        var randomTargetPos = new Vector2((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);
        enemy.TargetPosition = _player.Position + randomTargetPos;
    }

    private void Move(EnemyModel enemy, float deltaTime)
    {
        Vector2 toTarget = enemy.TargetPosition - enemy.Position;

        if (toTarget.LengthSquared() < 10f)
        {
            GenerateNewTarget(enemy);
            return;
        }

        Vector2 direction = Vector2.Normalize(toTarget);
        float targetAngle = (float)Math.Atan2(direction.Y, direction.X);
        float angleDifference = MathHelper.WrapAngle(targetAngle - enemy.Rotation);

        var deltaTurn = enemy.RotationSpeed * deltaTime;
        enemy.Rotation += MathHelper.Clamp(angleDifference, -deltaTurn, deltaTurn);

        var desiredDirection = new Vector2((float)Math.Cos(enemy.Rotation), (float)Math.Sin(enemy.Rotation));
        enemy.Velocity = desiredDirection * enemy.CurrentSpeed;
        enemy.SetPosition(enemy.Velocity * deltaTime);
    }

    private void CheckEvasion(EnemyModel enemy, float deltaTime)
    {
        if (!enemy.IsPursuingPlayer) return;

        Vector2 toPlayer = _player.Position - enemy.Position;
        float angleToPlayer = (float)Math.Atan2(toPlayer.Y, toPlayer.X);
        float angleDiff = MathHelper.WrapAngle(angleToPlayer - enemy.Rotation);

        bool isEvading = false;
        if (Math.Abs(angleDiff) > enemy.EvasionAngle)
        {
            float evasionDirection = Math.Sign(angleDiff);
            enemy.Rotation += enemy.RotationSpeed * 2f * deltaTime * evasionDirection;
            isEvading = true;
        }

        float speed = enemy.Acceleration * deltaTime;
        enemy.SetCurrentSpeed(speed, isEvading);
    }

    private void Fire(EnemyModel enemy)
    {
        if (!IsPlayerInFieldOfView(enemy)) return;

        if (enemy.CanFire && (enemy.Cooldowns[0].AvailableToFire ||
             enemy.Cooldowns[1].AvailableToFire))
        {
            _missiles.CreateMissile(enemy.Position + enemy.MissileJointPosition, enemy, _player);
            enemy.FiredMissileCount++;
            enemy.FireDelayTimer = 2f;
        }
    }

    private bool IsPlayerInFieldOfView(EnemyModel enemy)
    {
        Vector2 toPlayer = _player.Position - enemy.Position;
        if (toPlayer.LengthSquared() == 0) return false;

        toPlayer.Normalize();

        Vector2 forward = new(
            (float)Math.Cos(enemy.Rotation),
            (float)Math.Sin(enemy.Rotation)
        );

        float angleToPlayer = (float)Math.Acos(Vector2.Dot(forward, toPlayer));

        return Math.Abs(angleToPlayer) <= enemy.FieldOfViewAngle / 2f;
    }
}