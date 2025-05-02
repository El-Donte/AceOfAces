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
    private readonly PlayerModel _model;

    private Vector2 _inputDirection;
    private List<EnemyModel> _enemy;

    public PlayerController(PlayerModel playerModel,MissileListModel missileList, List<EnemyModel> enemyModel)
    {
        _model = playerModel;
        _missiles = missileList;
        _enemy = enemyModel;

        _model.DestroyedEvent += OnPlayerDestroyed;
        _model.OnDamagedEvent += StartBlinkingEffect;
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
        _model.Rotation += _inputDirection.X * _model.RotationSpeed * deltaTime;

        bool isAccelerating = _inputDirection.Y > 0;
        UpdateSpeed(isAccelerating ? 1 : 2, !isAccelerating, deltaTime);

        Vector2 direction = new Vector2((float)Math.Sin(_model.Rotation), -(float)Math.Cos(_model.Rotation));
        _model.Velocity = (_inputDirection.Y + 2) * direction * _model.CurrentSpeed;

        _model.SetPosition(_model.Velocity * deltaTime);
    }

    private void UpdateSpeed(int k, bool isBreaking, float deltaTime)
    {
        _model.CurrentSpeed += k * (isBreaking ? -_model.Decceleration : _model.Acceleration) * deltaTime;
    }

    private void InputUpdate()
    {
        InputManager.Update();
        _inputDirection = InputManager.InputDirection;
    }

    private void Fire()
    {
        if (InputManager.IsKeyPressed(Keys.Space) && 
            (_missiles.Cooldowns[0].AvailableToFire || 
             _missiles.Cooldowns[1].AvailableToFire))
        {
            _missiles.CreateMissile(_model.Position + _model.MissileJointPosition, _enemy[0], Core.GameObjectType.Player);
            _model.FiredMissileCount++;
        }
    }

    private void UpdateBlinking()
    {
        if (_model.IsInvulnerable)
        {
            _model.BlinkPhase += 5f;
        }
    }

    private void UpdateInvurTimer(float deltaTime)
    {
        if (!_model.IsInvulnerable)
        {
            return;
        }

        _model.InvulnerabilityTimer -= deltaTime;
    }

    private void StartBlinkingEffect(bool isInvulnerable)
    {
        if (isInvulnerable)
        {
            _model.BlinkPhase = -2f;
        }
    }

    private void OnPlayerDestroyed(GameObjectModel player)
    {
        _model.DestroyedEvent -= OnPlayerDestroyed;
    }
}

