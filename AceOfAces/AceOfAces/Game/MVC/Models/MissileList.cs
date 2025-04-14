using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class MissileList
{
    private readonly Texture2D _missileTexture;
    private ITarget _target;
    private List<MissileModel> _missiles = new List<MissileModel>();

    public List<MissileModel> Missiles => _missiles;

    public MissileList(Texture2D missileTexture, ITarget target)
    {
        _missileTexture = missileTexture;
        _target = target;
    }

    public void AddMissile(Vector2 position)
    {
        var missile = new MissileModel(_missileTexture, position);
        missile.SetTarget(_target);
        missile.Destroyed += OnMissileDestroyed;
        _missiles.Add(missile);
    }

    private void OnMissileDestroyed(GameObjectModel missile)
    {
        missile.Destroyed -= OnMissileDestroyed;
        _missiles.Remove((MissileModel)missile);
    }
}

