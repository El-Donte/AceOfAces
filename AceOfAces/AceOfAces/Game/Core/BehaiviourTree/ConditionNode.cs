using AceOfAces.Models;
using System;

namespace AceOfAces.BehaiviourTree
{
    public class ConditionNode : Node
    {
        private Func<EnemyModel, float, bool> _condition;

        public ConditionNode(Func<EnemyModel, float, bool> condition) => _condition = condition;

        public override bool Evaluate(EnemyModel enemy, float deltaTime) => _condition(enemy, deltaTime);
    }
}
