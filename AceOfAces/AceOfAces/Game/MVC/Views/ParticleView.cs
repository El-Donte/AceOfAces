using AceOfAces.Managers;
using AceOfAces.Core.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Views;

public class ParticleView : IView
{
    private readonly List<ParticleModel> _particles;
    private Vector2 _origin;
    private SpriteBatch _spriteBatch;

    public ParticleView(SpriteBatch spriteBatch)
    {
        _particles = ParticleSystem.Particles;
        _origin = new Vector2(AssetsManager.ParticleTexture.Width / 2, AssetsManager.ParticleTexture.Height / 2);
        _spriteBatch = spriteBatch;
    }

    public void Draw()
    {
        foreach (var particle in _particles) 
        { 
            _spriteBatch.Draw(particle.ParticleData.texture, 
                particle.Position, 
                null, 
                particle.Color * particle.Opacity, 
                0f, 
                _origin, 
                particle.Scale, 
                SpriteEffects.None, 
                1f);
        }
    }
}