using AceOfAces.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace AceOfAces.BehaiviourTree
{
    public class EnemyBehaiviourTree
    {
        private readonly PlayerModel _player;
        

        public EnemyBehaiviourTree(PlayerModel player)
        {
            _player = player;
        }

        public Node CreateBehaiviourTree(ActionNode updateTarget, ActionNode move, ActionNode checkEvasion, ActionNode generateNewTarget)
        {
            var pursueSequence = new SequenceNode(new List<Node>
            {
                new ConditionNode((enemy, dt) =>
                {
                    float distance = Vector2.Distance(enemy.Position, _player.Position);
                    return distance < enemy.PursuitRadius;
                }),
                updateTarget,
                move,
                checkEvasion
            });

            var patrolSequence = new SequenceNode(new List<Node>
            {
                generateNewTarget,
                move
            });

            return new SelectorNode(new List<Node>
            {
                pursueSequence,
                patrolSequence
            });
        }
    }

}
