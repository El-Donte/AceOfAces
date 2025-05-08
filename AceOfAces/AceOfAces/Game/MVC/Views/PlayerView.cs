using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AceOfAces.Views;

public class PlayerView : IView
{
    private readonly Texture2D _missileTexture = AssetsManager.MissileTexture;
    private readonly Vector2 _missileOrigin;
    private readonly Texture2D _playerTexture = AssetsManager.PlayerTexture;
    private readonly Vector2 _playerOrigin;

    private readonly PlayerModel _player;
    private float _alpha = 1f;

    private readonly SpriteBatch _spriteBatch;

    public PlayerView(PlayerModel playerModel, SpriteBatch spriteBatch)
    {
        _player = playerModel;

        _missileOrigin = new Vector2(_missileTexture.Width / 2f, _missileTexture.Height / 2f);
        _playerOrigin = new Vector2(_playerTexture.Width / 2f, _playerTexture.Height / 2f + 10);

        _spriteBatch = spriteBatch;

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
        _spriteBatch.Draw(
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
        _spriteBatch.Draw(
            _playerTexture,
            _player.Position,
            null,
            color,
            _player.Rotation,
            _playerOrigin,
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

