using AceOfAces.Models;
using System;

namespace AceOfAces.BehaiviourTree;

public class ConditionNode : Node
{
    private readonly Func<EnemyModel, bool> _condition;

    public ConditionNode(Func<EnemyModel, bool> condition) => _condition = condition;

    public override bool Evaluate(EnemyModel enemy, float deltaTime) => _condition(enemy);
}