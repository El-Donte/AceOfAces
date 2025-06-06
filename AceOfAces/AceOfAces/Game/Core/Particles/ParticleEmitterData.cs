namespace AceOfAces.Core.Particles;

public struct ParticleEmitterData
{
    public ParticleData ParticleData { get; set; } = new();
    public float Angle { get; set; } = 0f;
    public float AngleVariance { get; set; } = 45f;
    public float LifespanMin { get; set; } = 0.1f;
    public float LifespanMax { get; set; } = 2f;
    public float SpeedMin { get; set; } = 10f;
    public float SpeedMax { get; set; } = 100f;
    public float Interval { get; set; } = 1f;
    public int EmitCount { get; set; } = 1;

    public ParticleEmitterData()
    {
    }
}

