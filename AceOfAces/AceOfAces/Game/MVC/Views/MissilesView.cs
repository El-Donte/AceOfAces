using AceOfAces.Managers;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class MissilesView : IView
{
    private readonly Texture2D _missileTexture = AssetsManager.MissileTexture;
    private readonly Vector2 _missileOrigin;
    private readonly MissileListModel _missileList;

    private readonly SpriteBatch _spriteBatch;

    public MissilesView(MissileListModel missileList, SpriteBatch spriteBatch)
    {
        _missileList = missileList;
        _missileOrigin = new Vector2(_missileTexture.Width / 2f, _missileTexture.Height / 2f + 10);

        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        for(int i = 0; i < _missileList.Missiles.Count; i++)
        {
            var missile = _missileList.Missiles[i];
            _spriteBatch.Draw(
                _missileTexture,
                missile.Position,
                null,
                Color.White,
                missile.Rotation + MathHelper.PiOver2,
                _missileOrigin,
                1,
                SpriteEffects.None,
                0f);
        }
    }
}

