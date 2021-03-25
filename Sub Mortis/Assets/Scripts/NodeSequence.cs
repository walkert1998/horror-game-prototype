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
            switch (state)
            {
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    Debug.Log(node + " " + state);
                    return _nodeState;
                case NodeState.SUCCESS:
                    Debug.Log(node + " " + state);
                    continue;
                case NodeState.RUNNING:
                    nodeRunning = true;
                    _nodeState = NodeState.RUNNING;
                    Debug.Log(node + " " + state);
                    return _nodeState;
                default:
                    Debug.Log(node + " " + state);
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
            }
        }
        _nodeState = nodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        Debug.Log(_nodeState);
        return _nodeState;
    }
}
