using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTask : BTNode
{
    NPCAI npcAI;

    public ChaseTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget == null)
        {
            return NodeState.FAILURE;
        }
        if (npcAI.waiting)
        {
            npcAI.waiting = false;
        }
        npcAI.targetDestination = npcAI.lastKnownPosition;
        return NodeState.SUCCESS;
    }
}
