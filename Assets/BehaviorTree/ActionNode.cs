using System;


public class ActionNode : BehaviorTreeNode
{
    private System.Action action;

    public ActionNode(System.Action action)
    {
        this.action = action;
    }

    public bool Execute()
    {
        action();
        return true; // For simplicity, we assume actions always succeed.
    }

}