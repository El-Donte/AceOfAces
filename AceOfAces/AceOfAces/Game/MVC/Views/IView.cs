using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views;

public interface IView
{
    public SpriteBatch SpriteBatch { get; set; }

    public abstract void Draw();
}

