using AceOfAces.Core;
using Microsoft.Xna.Framework;

namespace AceOfAces.Models;

public class CollisionModel
{
    public Rectangle Bounds { get; private set; }
    public GameObjectType ObjectType { get; private set; }

    public CollisionModel(Rectangle bounds, GameObjectType type)
    {
        Bounds = bounds;
        ObjectType = type;
    }

    public void UpdateBounds(Rectangle newBounds)
    {
        Bounds = newBounds;
    }
}

