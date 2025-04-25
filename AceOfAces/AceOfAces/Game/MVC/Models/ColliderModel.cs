using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class ColliderModel
{
    public Rectangle Bounds { get; private set; }

    public ColliderModel(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void UpdateBounds(Rectangle newBounds)
    {
        Bounds = newBounds;
    }
}

