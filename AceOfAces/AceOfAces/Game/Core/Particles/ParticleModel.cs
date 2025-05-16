using Microsoft.Xna.Framework;
using System;

namespace AceOfAces.Core.Particles;

public class ParticleModel
{
    private readonly ParticleData _data;
    public ParticleData ParticleData => _data;

    public bool isFinished = false;

    private Color _color;
    public Color Color => _color;

    private float _opacity;
    public float Opacity => _opacity;

    private float _scale;
    public float Scale => _scale;

    private Vector2 _direction;
    public Vector2 Direction => _direction;

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    private float _lifespanAmount;
    private float _lifespanLeft;
    public float LifespanLeft
    {
        get => _lifespanLeft;
        set
        {
            _lifespanLeft = value;
            if(_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
        }
    }

    public ParticleModel(Vector2 pos, ParticleData data)
    {
        _data = data;
        _lifespanLeft = data.lifespan;
        _lifespanAmount = 1f;
        _position = pos;
        _color = data.colorStart;
        _opacity = data.opacityStart;
        if (data.speed != 0)
        {
            _data.angle = MathHelper.ToRadians(_data.angle);
            _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
        }
        else
        {
            _direction = Vector2.Zero;
        }
    }
}