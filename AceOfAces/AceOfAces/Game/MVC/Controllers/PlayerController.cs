using AceOfAces.Core;
using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class PlayerController : IController
{
    private readonly MissileListModel _missiles;
    private readonly List<EnemyModel> _enemies;
    private readonly PlayerModel _player;

    private Vector2 _inputDirection;

    public PlayerController(PlayerModel playerModel,MissileListModel missileList, SpawnerModel spawner)
    {
        _player = playerModel;
        _missiles = missileList;
        _enemies = spawner.Enemies;
        _missiles.AddCooldowns(_player.Cooldowns);

        _player.DestroyedEvent += OnPlayerDestroyed;
        _player.OnDamagedEvent += StartBlinkingEffect;
    }

    public void Update(float deltaTime)
    {
        InputUpdate();
        UpdateMovement(deltaTime);
        UpdateBlinking();
        UpdateInvurTimer(deltaTime);
        ChangeTarget();
        Fire();
    }

    private void UpdateMovement(float deltaTime)
    {
        _player.Rotation += _inputDirection.X * _player.RotationSpeed * deltaTime;

        bool isAccelerating = _inputDirection.Y > 0;
        UpdateSpeed(isAccelerating ? 1 : 2, !isAccelerating, deltaTime);

        Vector2 direction = new((float)Math.Sin(_player.Rotation), -(float)Math.Cos(_player.Rotation));
        _player.Velocity = (_inputDirection.Y + 2) * direction * _player.CurrentSpeed;

        _player.SetPosition(_player.Velocity * deltaTime);
    }

    private void UpdateSpeed(int k, bool isBreaking, float deltaTime)
    {
        _player.CurrentSpeed += k * (isBreaking ? -_player.Decceleration : _player.Acceleration) * deltaTime;
    }

    private void InputUpdate()
    {
        InputManager.Update();
        _inputDirection = InputManager.InputDirection;
    }

    private void Fire()
    {
        if (_enemies.Count == 0 || _player.TargetIndex >= _enemies.Count)
        {
            return;
        }

        var target = _enemies[_player.TargetIndex];
        if (InputManager.IsKeyPressed(Keys.Space) && 
            (_player.Cooldowns[0].AvailableToFire || 
             _player.Cooldowns[1].AvailableToFire))
        {
            _missiles.CreateMissile(_player.Position + _player.MissileJointPosition,_player , target);
            _player.FiredMissileCount++;

            GameEvents.TriggerBulletTrail(_player.Position);
        }
    }

    private void ChangeTarget()
    {
        if (_enemies.Count == 0 || _player.TargetIndex >= _enemies.Count)
        {
            return;
        }

        if (InputManager.IsKeyPressed(Keys.E))
        {
            _enemies[_player.TargetIndex].IsTargeted = false;

            _player.TargetIndex = (_player.TargetIndex + 1) % _enemies.Count;

            _enemies[_player.TargetIndex].IsTargeted = true;
        }
    }

    private void UpdateBlinking()
    {
        if (_player.IsInvulnerable)
        {
            _player.BlinkPhase += 5f;
        }
    }

    private void UpdateInvurTimer(float deltaTime)
    {
        if (!_player.IsInvulnerable)
        {
            return;
        }

        _player.InvulnerabilityTimer -= deltaTime;
    }

    private void StartBlinkingEffect(bool isInvulnerable)
    {
        if (isInvulnerable)
        {
            _player.BlinkPhase = -2f;
        }
    }

    private void OnPlayerDestroyed(GameObjectModel player)
    {
        _player.DestroyedEvent -= OnPlayerDestroyed;
    }
}

