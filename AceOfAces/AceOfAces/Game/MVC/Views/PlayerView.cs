using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class PlayerView : IView
{
    private readonly List<MissileCooldownModel> _missileCooldowns = new List<MissileCooldownModel>();
    private readonly Texture2D _missileTexture;
    private readonly Vector2 _missileOrigin;

    private readonly PlayerModel _model;
    private float _alpha = 1f;

    public SpriteBatch SpriteBatch { get; set; }

    public PlayerView(PlayerModel playerModel, Texture2D missileTexture, List<MissileCooldownModel> cooldowns)
    {
        _model = playerModel;

        _missileCooldowns = cooldowns;
        _missileTexture = missileTexture;
        _missileOrigin = new Vector2(_missileTexture.Width / 2, _missileTexture.Height / 2);

        _model.OnBlinkPhaseChanged += SetAlpha;
    }

    public void Draw()
    {
        Color color = Color.White * _alpha;

        if (_missileCooldowns[0].AvailableToFire)
        {
            DrawMissile(-_model.MissileJointPosition, color);
        }

        if (_missileCooldowns[1].AvailableToFire)
        {
            DrawMissile(_model.MissileJointPosition, color);
        }

        DrawPlayer(color);
    }

    private void DrawMissile(Vector2 offset, Color color)
    {
        SpriteBatch.Draw(
            _missileTexture,
            _model.Position + offset,
            null,
            color,
            _model.Rotation,
            _missileOrigin,
            1f,
            SpriteEffects.None,
            0f
        );
    }

    private void DrawPlayer(Color color)
    {
        SpriteBatch.Draw(
            _model.Texture,
            _model.Position,
            null,
            color,
            _model.Rotation,
            _model.Origin,
            1f,
            SpriteEffects.None,
            0f
        );
    }

    private void SetAlpha(float phase)
    {
        _alpha = 0.5f + 0.5f * (float)Math.Sin(phase);
    }
}

