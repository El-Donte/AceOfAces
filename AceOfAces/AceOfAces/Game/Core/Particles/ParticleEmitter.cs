using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Core.Particles;

public class ParticleEmitter
{
    private readonly Dictionary<ParticleType, ParticleEmitterData> _presets = new()
    {
        {
            ParticleType.Explosion,
            new ParticleEmitterData
            {
                emitCount = 20,
                interval = 0.1f,
                lifespanMin = 0.5f,
                lifespanMax = 1f,
                speedMin = 100f,
                speedMax = 300f,
                particleData = new ParticleData
                {
                    colorStart = Color.Orange,
                    colorEnd = Color.Red
                }
            }
        },
        {
            ParticleType.BulletTrail,
            new ParticleEmitterData
            {
                emitCount = 1,
                interval = 0.05f,
                lifespanMin = 0.5f,
                lifespanMax = 1f,
                speedMin = 50f,
                speedMax = 100f,
                particleData = new ParticleData
                {
                    colorStart = Color.DarkRed,
                    colorEnd = Color.Transparent
                }
            }
        }
    };

    private readonly Random _random = new();

    public ParticleEmitter()
    {
        GameEvents.OnExplosionEvent += (position) => Emit(ParticleType.Explosion, position);
        GameEvents.OnBulletTrailEvent += (position) => Emit(ParticleType.BulletTrail, position);
    }

    private void Emit(ParticleType type, Vector2 position)
    {
        if (!_presets.TryGetValue(type, out var data))
        {
            return;
        }

        for (int i = 0; i < data.emitCount; i++)
        {
            ParticleData d = data.particleData;

            d.lifespan = (float)(_random.NextDouble() * (data.lifespanMax - data.lifespanMin)) + data.lifespanMin;
            d.speed = (float)(_random.NextDouble() * (data.speedMax - data.speedMin)) + data.speedMin;

            var angleMin = data.angle - data.angleVariance;
            var angleMax = data.angle + data.angleVariance;
            d.angle = (float)(_random.NextDouble() * (angleMax - angleMin)) + angleMin;

            ParticleModel p = new(position, d);
            ParticleSystem.AddParticle(p);
        }
    }
}