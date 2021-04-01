using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateSuspiciousThingTask : BTNode
{
    NPCAI npcAI;

    public InvestigateSuspiciousThingTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget != null)
        {
            return NodeState.FAILURE;
        }
        npcAI.targetDestination = npcAI.lastKnownPosition;
        if (npcAI.waiting)
        {
            npcAI.waiting = false;
        }
        return NodeState.SUCCESS;
    }
}
