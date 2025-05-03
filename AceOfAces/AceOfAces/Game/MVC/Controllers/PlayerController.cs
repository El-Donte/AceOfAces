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
    private readonly List<EnemyModel> _enemy;
    private readonly PlayerModel _player;

    private Vector2 _inputDirection;

    public PlayerController(PlayerModel playerModel,MissileListModel missileList, List<EnemyModel> enemyModel)
    {
        _player = playerModel;
        _missiles = missileList;
        _enemy = enemyModel;
        _missiles.AddCooldown(_player.Cooldowns);

        _player.DestroyedEvent += OnPlayerDestroyed;
        _player.OnDamagedEvent += StartBlinkingEffect;
    }

    public void Update(float deltaTime)
    {
        InputUpdate();
        UpdateMovement(deltaTime);
        UpdateBlinking();
        UpdateInvurTimer(deltaTime);
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
        if (InputManager.IsKeyPressed(Keys.Space) && 
            (_player.Cooldowns[0].AvailableToFire || 
             _player.Cooldowns[1].AvailableToFire))
        {
            _missiles.CreateMissile(_player.Position + _player.MissileJointPosition,_player , _enemy[0]);
            _player.FiredMissileCount++;
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

