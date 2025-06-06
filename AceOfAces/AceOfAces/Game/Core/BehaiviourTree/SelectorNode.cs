using AceOfAces.Models;

namespace AceOfAces.BehaiviourTree;

public class SelectorNode : Node
{
    public SelectorNode() { }

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