using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Core.Particles;

public static class ParticleEmitter
{
    private static readonly Dictionary<ParticleType, ParticleEmitterData> _presets = new()
    {
        {
            ParticleType.Explosion,
            new ParticleEmitterData
            {
                EmitCount = 30,
                Interval = 0.01f,
                LifespanMin = 0.5f,
                LifespanMax = 1f,
                SpeedMin = 100f,
                SpeedMax = 300f,
                ParticleData = new ParticleData
                {
                    ColorStart = Color.Orange,
                    ColorEnd = Color.Red
                }
            }
        },
        {
            ParticleType.BulletTrail,
            new ParticleEmitterData
            {
                EmitCount = 3,
                Interval = 0.1f,
                LifespanMin = 0.2f,
                LifespanMax = 0.7f,
                SpeedMin = 10f,
                SpeedMax = 50f,
                ParticleData = new ParticleData
                {
                   ColorStart = Color.DarkGray,
                   ColorEnd = Color.LightSlateGray
                }
            }
        }
    };

    private static readonly Random _random = new();

    public static void Initialize()
    {
        GameEvents.ExplosionEvent += (position) => Emit(ParticleType.Explosion, position);
        GameEvents.BulletTrailEvent += (position) => Emit(ParticleType.BulletTrail, position);
    }

    private static void Emit(ParticleType type, Vector2 position)
    {
        if (!_presets.TryGetValue(type, out var data))
        {
            return;
        }

        for (int i = 0; i < data.EmitCount; i++)
        {
            ParticleData particle = data.ParticleData;

            particle.Lifespan = (float)(_random.NextDouble() * (data.LifespanMax - data.LifespanMin)) + data.LifespanMin;
            particle.Speed = (float)(_random.NextDouble() * (data.SpeedMax - data.SpeedMin)) + data.SpeedMin;

            var angleMin = data.Angle - data.AngleVariance;
            var angleMax = data.Angle + data.AngleVariance;
            particle.Angle = (float)(_random.NextDouble() * (angleMax - angleMin)) + angleMin;

            ParticleModel p = new(position, particle);
            ParticleSystem.AddParticle(p);
        }
    }
}