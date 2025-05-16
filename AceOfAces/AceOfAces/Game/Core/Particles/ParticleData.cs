using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core.Particles;

public struct ParticleData
{
    public Texture2D texture = AssetsManager.ParticleTexture;
    public float lifespan = 1f;
    public Color colorStart = Color.Yellow;
    public Color colorEnd = Color.Red;
    public float opacityStart = 1f;
    public float opacityEnd = 0f;
    public float sizeStart = 32f;
    public float sizeEnd = 4f;
    public float speed = 10f;
    public float angle = 0f;

    public ParticleData(){ }
}

