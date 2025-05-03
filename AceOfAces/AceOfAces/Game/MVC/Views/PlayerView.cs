using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Views;

public class PlayerView : IView
{
    private readonly Texture2D _missileTexture;
    private readonly Vector2 _missileOrigin;

    private readonly PlayerModel _player;
    private float _alpha = 1f;

    public SpriteBatch SpriteBatch { get; set; }

    public PlayerView(PlayerModel playerModel, Texture2D missileTexture)
    {
        _player = playerModel;

        _missileTexture = missileTexture;
        _missileOrigin = new Vector2(_missileTexture.Width / 2, _missileTexture.Height / 2);

        _player.OnBlinkPhaseChangedEvent += SetAlpha;
    }

    public void Draw()
    {
        Color color = Color.White * _alpha;

        if (_player.Cooldowns[0].AvailableToFire)
        {
            DrawMissile(-_player.MissileJointPosition, color);
        }

        if (_player.Cooldowns[1].AvailableToFire)
        {
            DrawMissile(_player.MissileJointPosition, color);
        }

        DrawPlayer(color);
    }

    private void DrawMissile(Vector2 offset, Color color)
    {
        SpriteBatch.Draw(
            _missileTexture,
            _player.Position + offset,
            null,
            color,
            _player.Rotation,
            _missileOrigin,
            1f,
            SpriteEffects.None,
            0f
        );
    }

    private void DrawPlayer(Color color)
    {
        SpriteBatch.Draw(
            _player.Texture,
            _player.Position,
            null,
            color,
            _player.Rotation,
            _player.Origin,
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

