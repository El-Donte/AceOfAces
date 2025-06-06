using AceOfAces.Core.Particles;

namespace AceOfAces.Controllers;

public class ParticleContorller : IController
{
    public void Update(float deltaTime)
    {
        var particles = ParticleSystem.Particles;

        for (int i = 0; i < particles.Count; i++)
        {
            var p = particles[i];
            p.LifespanLeft -= deltaTime;

            if (!p.isFinished)
            {
                p.Position += p.Direction * p.ParticleData.Speed * deltaTime;
            }
        }

        ParticleSystem.Particles.RemoveAll(p => p.isFinished);
    }
}