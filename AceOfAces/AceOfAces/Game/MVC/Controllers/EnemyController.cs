using AceOfAces.BehaiviourTree;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class EnemyController : IController
{
    private readonly List<EnemyModel> _enemies;
    private readonly EnemyBehaiviourTree _decisionTree;
    private readonly List<Node> _decisionTrees = new List<Node>();
    private readonly PlayerModel _player;
    private Random _random = new Random();

    private readonly ActionNode updateTargetAction;
    private readonly ActionNode moveAction;
    private readonly ActionNode checkEvasionAction;
    private readonly ActionNode generateNewTargetAction;

    public EnemyController(List<EnemyModel> models, PlayerModel player)
    {
        _enemies = models;
        _decisionTree = new EnemyBehaiviourTree(player);
        _player = player;

        updateTargetAction = new ActionNode((enemy, dt) => UpdateTarget(enemy));
        moveAction = new ActionNode((enemy, dt) => Move(enemy, dt));
        checkEvasionAction = new ActionNode((enemy, dt) => CheckEvasion(enemy, dt));
        generateNewTargetAction = new ActionNode((enemy, dt) => GenerateNewTarget(enemy));

        CreateNewBehaiviourTree();
    }

    public void Update(float deltaTime)
    {
        for(int i = 0; i < _enemies.Count; i++)
        {
            var enemy = _enemies[i];
            _decisionTrees[i].Evaluate(enemy, deltaTime);
        }
    }

    private void CreateNewBehaiviourTree()
    {
        _decisionTrees.Clear();
        foreach (var enemy in _enemies)
        {
            _decisionTrees.Add(_decisionTree.CreateBehaiviourTree(
                updateTargetAction, moveAction, 
                checkEvasionAction, generateNewTargetAction
                ));
        }
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
        if (!enemy.IsPursuingPlayer)
        {
            return;
        }

        Vector2 toPlayer = _player.Position - enemy.Position;
        float angleToPlayer = (float)Math.Atan2(toPlayer.Y, toPlayer.X);
        float angleDiff = MathHelper.WrapAngle(angleToPlayer - enemy.Rotation);

        float speed = enemy.Acceleration * deltaTime;
        if (Math.Abs(angleDiff) > enemy.EvasionAngle)
        {
            float evasionDirection = Math.Sign(angleDiff);
            enemy.Rotation += enemy.RotationSpeed * 1.5f * deltaTime * evasionDirection;
            speed *= 1.5f;
        }

        enemy.CurrentSpeed = speed;
    }
}