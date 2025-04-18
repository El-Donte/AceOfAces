using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AceOfAces.Controllers;

public class PlayerController : IController
{
    private readonly MissileListModel _missiles;
    private readonly PlayerModel _model;

    private Vector2 _inputDirection;
    private EnemyModel _enemy;

    public PlayerController(PlayerModel playerModel,MissileListModel missileList, EnemyModel enemyModel)
    {
        _model = playerModel;
        _missiles = missileList;
        _enemy = enemyModel;

        _model.Destroyed += OnPlayerDestroyed;
        _model.OnDamaged += StartBlinkingEffect;
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
        var rotation = _inputDirection.X * _model.RotationSpeed * deltaTime;
        _model.SetRoration(rotation);

        Vector2 direction = new Vector2((float)Math.Sin(_model.Rotation), -(float)Math.Cos(_model.Rotation));

        if (_inputDirection.Y > 0)
        {
            UpdateSpeed(1, false, deltaTime);
        }
        else
        {
            var keyKoeff = _inputDirection.Y < 0 ? 2 : 1;
            UpdateSpeed(keyKoeff, true, deltaTime);
        }

        var velocity = (_inputDirection.Y + 2) * direction * _model.CurrentSpeed;
        _model.SetVelocity(velocity);

        var position = _model.Velocity * deltaTime;
        _model.SetPosition(position);
    }

    private void UpdateSpeed(int k, bool isBreaking, float deltaTime)
    {
        var currentSpeed = _model.CurrentSpeed;
        if (isBreaking)
        {
            currentSpeed -= k * _model.Decceleration * deltaTime;
        }
        else
        {
            currentSpeed += k * _model.Acceleration * deltaTime;
        }

        currentSpeed = MathHelper.Clamp(currentSpeed, _model.MinSpeed, _model.MaxSpeed);
        _model.SetCurrentSpeed(currentSpeed);
    }

    private void InputUpdate()
    {
        InputManager.Update();
        _inputDirection = InputManager.InputDirection;
    }

    private void Fire()
    {
        if (InputManager.IsKeyPressed(Keys.Space) && (_missiles.Cooldowns[0].AvailableToFire || _missiles.Cooldowns[1].AvailableToFire))
        {
            _missiles.CreateMissile(_model.Position + _model.MissileJointPosition, _enemy, Core.GameObjectType.Player);
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
        _model.Destroyed -= OnPlayerDestroyed;
    }
}

