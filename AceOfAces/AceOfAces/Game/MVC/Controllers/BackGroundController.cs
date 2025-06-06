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

    public void Update(float deltaTime)
    {
        foreach (var layer in _layers)
        {
            Vector2 direction = new((float)Math.Sin(_player.Rotation), -(float)Math.Cos(_player.Rotation));
            layer.Position += direction * _player.CurrentSpeed * 1.5f * layer.LayerSpeed * deltaTime; 
        }
    }
}