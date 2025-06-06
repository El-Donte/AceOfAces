using AceOfAces.Models;

namespace AceOfAces.BehaiviourTree;

public class SequenceNode : Node
{
    public SequenceNode() { }

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