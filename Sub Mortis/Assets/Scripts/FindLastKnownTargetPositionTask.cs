using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLastKnownTargetPositionTask : BTNode
{
    NPCAI npcAI;
    bool reachedLastKnownPosition;

    public FindLastKnownTargetPositionTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
        reachedLastKnownPosition = false;
    }

    public override NodeState Evaluate()
    {
        if (reachedLastKnownPosition)
        {
            return NodeState.FAILURE;
        }
        else if (npcAI.targetDestination != npcAI.lastKnownPosition)
        {
            npcAI.targetDestination = npcAI.lastKnownPosition;
            return NodeState.SUCCESS;
        }
        else if (npcAI.agent.remainingDistance <= npcAI.agent.stoppingDistance)
        {
            npcAI.colorIndicator.SetSearching();
            reachedLastKnownPosition = true;
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }
}
