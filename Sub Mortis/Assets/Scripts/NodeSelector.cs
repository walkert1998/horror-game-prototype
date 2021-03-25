using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : BTNode
{
    protected List<BTNode> nodes = new List<BTNode>();

    public NodeSelector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        foreach (BTNode node in nodes)
        {
            NodeState state = node.Evaluate();
            Debug.Log(node + " " + state);
            switch (state)
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                default:
                    continue;
            }
        }
        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
