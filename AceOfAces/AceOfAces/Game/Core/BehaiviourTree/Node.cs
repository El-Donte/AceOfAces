using AceOfAces.Models;
using System.Collections.Generic;

namespace AceOfAces.BehaiviourTree;

public class Node
{
    public readonly List<Node> Children = new();

    public void AddChild(Node child) => Children.Add(child);

    public virtual bool Evaluate(EnemyModel enemy, float deltaTime) => false;
}