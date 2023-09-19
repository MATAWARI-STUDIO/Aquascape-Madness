using System.Collections.Generic;

// SequenceNode.cs
public class SequenceNode : BehaviorTreeNode
{
    private BehaviorTreeNode[] children;

    public SequenceNode(params BehaviorTreeNode[] nodes)
    {
        children = nodes;
    }

    public bool Execute()
    {
        foreach (var node in children)
        {
            if (!node.Execute())
                return false;
        }
        return true;
    }
}
