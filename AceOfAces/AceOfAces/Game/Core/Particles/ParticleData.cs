using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Core.Particles;

public struct ParticleData
{
    public Texture2D Texture { get; } = AssetsManager.ParticleTexture;
    public float Lifespan { get; set; } = 1f;
    public Color ColorStart { get; set; } = Color.Yellow;
    public Color ColorEnd { get; set; } = Color.Red;
    public float OpacityStart { get; set; } = 1f;
    public float OpacityEnd { get; set; } = 0f;
    public float SizeStart { get; set; } = 32f;
    public float SizeEnd { get; set; } = 4f;
    public float Speed { get; set; } = 10f;
    public float Angle { get; set; } = 0f;

    public ParticleData(){ }
}

