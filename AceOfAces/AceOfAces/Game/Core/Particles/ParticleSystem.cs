using System.Collections.Generic;

namespace AceOfAces.Core.Particles;

public static class ParticleSystem
{
    private static readonly List<ParticleModel> _particles = [];
    public static List<ParticleModel> Particles => _particles;

    public static void AddParticle(ParticleModel p)
    {
        _particles.Add(p);
    }
}