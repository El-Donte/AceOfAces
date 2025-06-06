 namespace AceOfAces.BehaiviourTree;

public class EnemyBehaiviourTreeBuilder
{
    private ActionNode _updateTargetPosition;
    public ActionNode UpdateTargetPosition { get => _updateTargetPosition; set => _updateTargetPosition = value; }

    private ActionNode _move;
    public ActionNode Move { get => _move; set => _move = value; }

    private  ConditionNode _checkEvasion;
    public ConditionNode CheckEvasion { get => _checkEvasion; set => _checkEvasion = value; }

    private ActionNode _generateNewTarget;
    public ActionNode GenerateNewTarget { get => _generateNewTarget; set => _generateNewTarget = value; }

    private ActionNode _fire;
    public ActionNode Fire { get=> _fire; set => _fire = value; }

    private ConditionNode _isPursuing;
    public ConditionNode IsPursuing { get => _isPursuing; set => _isPursuing = value; }

    private ConditionNode _isInFieldOfView;
    public ConditionNode IsInFieldOfView { get => _isInFieldOfView; set => _isInFieldOfView = value; }

    private ConditionNode _isPatroling = new ConditionNode((enemy) => !enemy.IsPursuingPlayer);

    private ActionNode _evasionMove;
    public ActionNode EvasionMove { get => _evasionMove; set => _evasionMove = value; }

    private ConditionNode _isNormalMove = new ConditionNode((enemy) => !enemy.IsEvading);

    public EnemyBehaiviourTreeBuilder() { }

    private Node CreatePursueSequence()
    {
        var pursueSequence = new SequenceNode();
        pursueSequence.AddChild(_isPursuing);
        pursueSequence.AddChild(_updateTargetPosition);

        return pursueSequence;
    }

    private Node CreateEvasionSelector()
    {
        var evasionSequence = new SequenceNode();
        evasionSequence.AddChild(_checkEvasion);
        evasionSequence.AddChild(_evasionMove);

        var selector = new SelectorNode();
        selector.AddChild(evasionSequence);

        return selector;
    }

    private Node CreateFireSelector()
    {
        var fireSequence = new SequenceNode();
        fireSequence.AddChild(_isInFieldOfView);
        fireSequence.AddChild(_fire);

        var fireSelector = new SelectorNode();
        fireSelector.AddChild(fireSequence);

        return fireSelector;
    }

    private Node CreatePatrolSequence()
    {
        var patrolSequence = new SequenceNode();
        patrolSequence.AddChild(_isPatroling);
        patrolSequence.AddChild(_generateNewTarget);
        patrolSequence.AddChild(_move);

        return patrolSequence;
    }

    private Node CreateMoveSequence()
    {
        var moveSequence = new SequenceNode();
        moveSequence.AddChild(_isNormalMove);
        moveSequence.AddChild(_move);
        return moveSequence;
    }

    public Node CreateBehaiviourTree()
    {
        var pursueSequence = CreatePursueSequence();
        var fireSelector = CreateFireSelector();
        var patrolSequence = CreatePatrolSequence();
        var evasionSelector = CreateEvasionSelector();
        var moveSequence = CreateMoveSequence();

        fireSelector.AddChild(patrolSequence);
        moveSequence.AddChild(fireSelector);
        evasionSelector.AddChild(moveSequence);
        pursueSequence.AddChild(evasionSelector);

        var startNode = new SelectorNode();
        startNode.AddChild(pursueSequence);
        startNode.AddChild(patrolSequence);

        return startNode;
    }
}