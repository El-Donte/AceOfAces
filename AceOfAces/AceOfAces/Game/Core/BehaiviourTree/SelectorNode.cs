using AceOfAces.Models;
using System.Collections.Generic;

namespace AceOfAces.BehaiviourTree;

public class SelectorNode : Node
{
    public SelectorNode(List<Node> children) => Children.AddRange(children);

    public override bool Evaluate(EnemyModel enemy, float deltaTime)
    {
        foreach (var child in Children)
        {
            if (child.Evaluate(enemy, deltaTime))
            {
                return true;
            }
        }
        return false;
    }
}

