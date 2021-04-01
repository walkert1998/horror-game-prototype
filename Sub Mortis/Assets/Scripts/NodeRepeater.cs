using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRepeater : BTNode
{
    BTNode childNode;
    int loopNumber = -1;
    int loopIndex;

    public NodeRepeater(int loopNumber, BTNode child)
    {
        this.loopNumber = loopNumber;
        childNode = child;
    }

    public override NodeState Evaluate()
    {
        NodeState state = childNode.Evaluate();
        Debug.Log(childNode + " " + state);
        if (loopNumber == -1 && state != NodeState.RUNNING)
        {
            return state;
        }
        else
        {
            if (loopIndex < loopNumber && state == NodeState.RUNNING)
            {
                loopIndex++;
                return NodeState.RUNNING;
            }
            else
            {
                loopIndex = 0;
                return state;
            }
        }
    }
}
