using AceOfAces.Models;
using Microsoft.Xna.Framework;

namespace AceOfAces.BehaiviourTree;

public class EnemyBehaiviourTreeBuilder
{
    private readonly PlayerModel _player;

    private readonly ActionNode _updateTarget;
    private readonly ActionNode _move;
    private readonly ActionNode _checkEvasion;
    private readonly ActionNode _generateNewTarget;
    private readonly ActionNode _fire;
    private readonly ConditionNode _isPursuing;

    public EnemyBehaiviourTreeBuilder(PlayerModel player, ActionNode updateTarget,ActionNode move,
        ActionNode checkEvasion,ActionNode generateNewTarget,ActionNode fire)
    {
        _player = player;

        _isPursuing = new ConditionNode((enemy, dt) =>
        {
            float distance = Vector2.Distance(enemy.Position, _player.Position);
            return distance < enemy.PursuitRadius;
        });

        _updateTarget = updateTarget;
        _move = move;
        _checkEvasion = checkEvasion;
        _generateNewTarget = generateNewTarget;
        _fire = fire;
    }

    public Node CreateBehaiviourTree()
    {
        var pursueSequence = new SequenceNode(
        [
            _isPursuing,
            _updateTarget,
            _move,
            _checkEvasion,
            _fire
        ]);

        var patrolSequence = new SequenceNode(
        [
            _generateNewTarget,
            _move
        ]);

        return new SelectorNode(
        [

            pursueSequence,
            patrolSequence
        ]);
    }
}