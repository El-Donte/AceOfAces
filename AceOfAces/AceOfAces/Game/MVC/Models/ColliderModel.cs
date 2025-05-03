using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class ColliderModel
{
    private Rectangle _bounds;
    public Rectangle Bounds => _bounds;

    public ColliderModel(Rectangle bounds) => _bounds = bounds;

    public void UpdateBounds(Rectangle newBounds)
    {
        _bounds = newBounds;
    }
}

