using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSequence : BTNode
{
    public List<BTNode> nodes = new List<BTNode>();

    public NodeSequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        bool nodeRunning = false;
        foreach (BTNode node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    nodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }
        }
        _nodeState = nodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
