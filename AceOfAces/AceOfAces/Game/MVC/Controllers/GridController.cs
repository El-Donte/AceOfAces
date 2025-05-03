using AceOfAces.Core;
using AceOfAces.Models;
using System.Collections.Generic;

namespace AceOfAces.Controllers;

public class GridController : IController
{
    private readonly Grid _grid;
    private readonly PlayerModel _player;
    private readonly List<EnemyModel> _enemies;
    private readonly MissileListModel _missileList;

    public GridController(Grid grid, PlayerModel player, MissileListModel missiles, List<EnemyModel> enemies) 
    { 
        _grid = grid;
        _player = player;
        _missileList = missiles;
        _enemies = enemies;
    }

    public void Update(float deltaTime)
    {
        UpdateActiveObjects();
        RegisterObjectsInGrid();
    }

    private void UpdateActiveObjects()
    {
        _grid.ActiveObjects.Clear();
        _grid.ActiveObjects.Add(_player);
        _grid.ActiveObjects.UnionWith(_missileList.Missiles);
        _grid.ActiveObjects.UnionWith(_enemies);
    }

    private void RegisterObjectsInGrid()
    {
        foreach (var obj in _grid.ActiveObjects)
        {
            if (!obj.IsDestroyed)
            {
                _grid.AddObject(obj);
            }
        }
    }
}

