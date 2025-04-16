using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class BackGroundController : IController
{
    private readonly List<BackgroundModel> _models = new List<BackgroundModel>();
    private readonly PlayerModel _player;

    public BackGroundController(List<BackgroundModel> models, PlayerModel player)
    {
        _player = player;
        _models.AddRange(models);
    }

    public void Update(float deltaTime)
    {
        foreach (var model in _models)
        {
            Vector2 direction = new Vector2((float)Math.Sin(_player.Rotation), -(float)Math.Cos(_player.Rotation));

            var position = direction *_player.CurrentSpeed * model.SpeedKoeff * deltaTime;
            model.SetPosition(position);
        }
    }

}

