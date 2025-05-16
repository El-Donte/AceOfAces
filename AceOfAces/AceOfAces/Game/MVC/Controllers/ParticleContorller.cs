using AceOfAces.Core.Particles;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class ParticleContorller : IController
{
    private readonly List<ParticleModel> _particles = new List<ParticleModel>();
    public ParticleContorller() 
    {
        _particles = ParticleSystem.Particles;
    }

    public void Update(float deltaTime)
    {
        foreach (var particle in _particles)
        {
            particle.LifespanLeft -= deltaTime;
            if(particle.isFinished)
            {
                break;
            }
            particle.Position += particle.Direction * particle.ParticleData.speed * deltaTime;
        }

        _particles.RemoveAll(p => p.isFinished);
    }
}