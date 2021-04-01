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
            NodeState state = node.Evaluate();
            Debug.Log(node + " " + state);
            switch (state)
            {
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    nodeRunning = true;
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                default:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
            }
        }
        _nodeState = nodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        //Debug.Log(_nodeState);
        return _nodeState;
    }
}
