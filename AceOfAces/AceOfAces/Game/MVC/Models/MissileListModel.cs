using AceOfAces.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class MissileListModel
{
    private readonly List<MissileModel> _missiles = new List<MissileModel>();
    public List<MissileModel> Missiles => _missiles;

    private readonly List<MissileCooldownModel> _cooldowns;
    public List<MissileCooldownModel> Cooldowns => _cooldowns;
    

    public MissileListModel(List<MissileCooldownModel> cooldowns)
    {
        _cooldowns = cooldowns;
    }

    public void CreateMissile(Vector2 position, ITarget target, GameObjectType source)
    {
        var missile = new MissileModel(position);
        missile.SetTarget(target);
        missile.SetSource(source);
        missile.Destroyed += OnMissileDestroyed;

        AddMissle(missile);
    }

    private void AddMissle(MissileModel missile)
    {
        foreach (var cooldown in _cooldowns)
        {
            if (cooldown.AvailableToFire)
            {
                cooldown.StartCooldown();
                _missiles.Add(missile);
                return;
            }
        }
    }

    private void OnMissileDestroyed(GameObjectModel missile)
    {
        missile.Destroyed -= OnMissileDestroyed;
        _missiles.Remove((MissileModel)missile);
    }
}

