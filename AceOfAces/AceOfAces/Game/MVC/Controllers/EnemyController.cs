using AceOfAces.Models;

namespace AceOfAces.Controllers;

public enum EnemyState { Spawn, Pursue, Attack, Evade }

public class EnemyController : IController
{
    private readonly EnemyModel _enemy;
    private readonly PlayerModel _player;

    public EnemyController(EnemyModel model, PlayerModel player)
    {
        _enemy = model;
        _player = player;
    }

    public void Update(float deltaTime)
    {
       
    }
}

