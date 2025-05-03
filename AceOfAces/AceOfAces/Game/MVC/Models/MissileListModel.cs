using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AceOfAces.Models;

public class MissileListModel
{
    private readonly List<MissileModel> _missiles = [];
    public List<MissileModel> Missiles => _missiles;

    private readonly List<MissileCooldownModel> _cooldowns = [];
    public List<MissileCooldownModel> Cooldowns => _cooldowns;

    public void AddCooldown(List<MissileCooldownModel> cooldowns) => _cooldowns.AddRange(cooldowns);

    public void CreateMissile(Vector2 position,ITarget source, ITarget target)
    {
        var missile = new MissileModel(position)
        {
            Target = target,
            Source = source.Type
        };
        missile.DestroyedEvent += OnMissileDestroyed;

        AddMissle(missile, source);
    }

    private void AddMissle(MissileModel missile, ITarget source)
    {
        foreach (var cooldown in source.Cooldowns)
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
        missile.DestroyedEvent -= OnMissileDestroyed;
        _missiles.Remove((MissileModel)missile);
    }
}

