using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AceOfAces.Controllers;

public class PlayerController : IController
{
    private readonly PlayerModel _model;
    private readonly MissileList _missiles;

    private Vector2 _inputDirection;

    public PlayerController(PlayerModel playerModel,MissileList missileController)
    {
        _model = playerModel;
        _missiles = missileController;

        _model.Destroyed += OnPlayerDestroyed;
        _model.OnDamaged += StartBlinkingEffect;
    }

    public void Update(float deltaTime)
    {
        InputUpdate();
        UpdateMovement(deltaTime);
        UpdateBlinking();
        _model.Invulnerable(deltaTime);
    }

    private void UpdateMovement(float deltaTime)
    {
        _model.Rotate(_inputDirection, deltaTime);

        Vector2 direction = new Vector2((float)Math.Sin(_model.Rotation), -(float)Math.Cos(_model.Rotation));

        if (_inputDirection.Y > 0)
        {
            _model.ChangeSpeed(1, false, deltaTime);
        }
        else
        {
            var keyKoeff = _inputDirection.Y < 0 ? 2 : 1;
            _model.ChangeSpeed(keyKoeff, true, deltaTime);
        }

        _model.Move((_inputDirection.Y + 2) * direction, deltaTime);
    }

    private void InputUpdate()
    {
        InputManager.Update();
        _inputDirection = InputManager.InputDirection;
        if(InputManager.IsKeyPressed(Keys.Space)) Fire();
    }

    private void Fire()
    {
        _missiles.AddMissile(_model.Position);
    }

    private void UpdateBlinking()
    {
        if (_model.IsInvulnerable)
        {
            _model.BlinkPhase += 5f;
        }
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

