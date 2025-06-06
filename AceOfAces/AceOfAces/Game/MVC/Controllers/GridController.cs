using AceOfAces.Core;
using AceOfAces.Models;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class GridController : IController
{
    private readonly PlayerModel _player;
    private readonly List<EnemyModel> _enemies;
    private readonly List<MissileModel> _missiles;
    private readonly Grid _grid;

    public GridController(Grid grid, PlayerModel player, MissileListModel missiles, SpawnerModel spawner)
    {
        _grid = grid;
        _player = player;
        _missiles = missiles.Missiles;
        _enemies = spawner.Enemies;
    }

    public void Update(float deltaTime)
    {
        _grid.UpdateGridPosition(_player.Position);

        UpdateObjects(_player);
        foreach (var enemy in _enemies)
        {
            UpdateObjects(enemy);
        }

        foreach (var missile in _missiles)
        {
            UpdateObjects(missile);
        }
    }

    private void UpdateObjects(GameObjectModel obj)
    {
        if (obj.IsDestroyed)
        {
            _grid.RemoveObject(obj);
        }
        else
        {
            _grid.AddObject(obj);
        }
    }
}
