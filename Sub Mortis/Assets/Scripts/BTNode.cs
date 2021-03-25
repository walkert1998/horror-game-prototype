using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BTNode
{
    protected NodeState _nodeState;
    public  NodeState nodeState
    {
        get
        {
            return _nodeState;
        }
    }
    public abstract NodeState Evaluate();

    public virtual void Reset()
    {
        _nodeState = NodeState.RUNNING;
    }
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}
