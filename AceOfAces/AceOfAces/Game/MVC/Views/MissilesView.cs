using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public class MissilesView : IView
{
    private readonly MissileListModel _missileList;
    public SpriteBatch SpriteBatch { get; set; }

    public MissilesView(MissileListModel missileList)
    {
        _missileList = missileList;
    }

    public void Draw()
    {
        foreach (var missile in _missileList.Missiles)
        {
            SpriteBatch.Draw(
                missile.Texture,
                missile.Position,
                null,
                Color.White,
                missile.Rotation + MathHelper.PiOver2,
                missile.Origin,
                1,
                SpriteEffects.None,
                0f);
        }
    }
}

