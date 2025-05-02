using AceOfAces.Models;
using System;

namespace AceOfAces.BehaiviourTree
{
    public class ActionNode : Node
    {
        private Action<EnemyModel, float> _action;

        public ActionNode(Action<EnemyModel, float> action) => _action = action;

        public override bool Evaluate(EnemyModel enemy, float deltaTime)
        {
            _action(enemy, deltaTime);
            return true;
        }
    }
}
