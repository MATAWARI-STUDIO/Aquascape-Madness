using System;


// ConditionNode.cs
public class ConditionNode : BehaviorTreeNode
{
    private System.Func<bool> condition;

    public ConditionNode(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public bool Execute()
    {
        return condition();
    }

}