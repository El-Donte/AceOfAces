using AceOfAces.Models;
using System.Collections.Generic;

namespace AceOfAces.BehaiviourTree;

public class SequenceNode : Node
{
    public SequenceNode(List<Node> children) => Children.AddRange(children);

    public override bool Evaluate(EnemyModel enemy, float deltaTime)
    {
        foreach (var child in Children)
        {
            if (!child.Evaluate(enemy, deltaTime))
            {
                return false;
            }
        }
        return true;
    }
}


