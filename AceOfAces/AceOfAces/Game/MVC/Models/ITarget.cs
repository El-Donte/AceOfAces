using AceOfAces.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AceOfAces.Models;

public interface ITarget
{
    public Vector2 Velocity { get;}

    public Vector2 Position { get;}

    public GameObjectType Type { get;}

    public List<MissileCooldownModel> Cooldowns { get;}
}

