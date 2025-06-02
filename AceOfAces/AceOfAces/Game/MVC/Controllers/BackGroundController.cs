using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class BackGroundController : IController
{
    private readonly List<LayerModel> _layers = [];
    private readonly PlayerModel _player;

    public BackGroundController(List<LayerModel> layers, PlayerModel player)
    {
        _player = player;
        _layers.AddRange(layers);
    }

    public BackGroundController(List<LayerModel> layers)
    {
        _layers.AddRange(layers);
        _player = null;
    }

    public void Update(float deltaTime)
    {
        var rotation = _player == null ? 1f : _player.Rotation;
        var speed = _player == null ? 10f : _player.CurrentSpeed;

        foreach (var layer in _layers)
        {
            Vector2 direction = new((float)Math.Sin(rotation), -(float)Math.Cos(rotation));
            layer.Position += direction * speed * 1.5f * layer.LayerSpeed * deltaTime; 
        }
    }
}