using System.Collections.Generic;

public class SelectorNode : BehaviorTreeNode
{
    private BehaviorTreeNode[] children;

    public SelectorNode(params BehaviorTreeNode[] nodes)
    {
        children = nodes;
    }

    public bool Execute()
    {
        foreach (var node in children)
        {
            if (node.Execute())
                return true;
        }
        return false;
    }

}