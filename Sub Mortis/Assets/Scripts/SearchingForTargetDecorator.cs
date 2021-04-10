using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingForTargetDecorator : BTNode
{
    NPCAI npcAI;
    BTNode child;

    public SearchingForTargetDecorator(NPCAI npcAI, BTNode child)
    {
        this.npcAI = npcAI;
        this.child = child;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log(timer);
        if (npcAI.currentTarget == null)
        {
            npcAI.currentAwarenessTime = 0;
            npcAI.colorIndicator.SetNormal();
            return NodeState.FAILURE;
        }
        if (!npcAI.currentlySearching)
        {
            npcAI.currentlySearching = true;
            npcAI.currentAwarenessTime = 0;
        }
        if (npcAI.currentAwarenessTime >= npcAI.awarenessTimer)
        {
            npcAI.SetTarget(null);
            npcAI.animator.SetBool("Aggressive", false);
            npcAI.animator.SetBool("Running", false);
            npcAI.currentlySearching = false;
            npcAI.colorIndicator.SetNormal();
            return NodeState.FAILURE;
        }

        npcAI.currentAwarenessTime += Time.deltaTime;
        NodeState check = child.Evaluate();
        //Debug.Log(check);

        if (check == NodeState.SUCCESS)
        {
            return NodeState.SUCCESS;
        }
        else if (check == NodeState.FAILURE)
        {
            npcAI.currentAwarenessTime = 0;
            npcAI.colorIndicator.SetNormal();
            npcAI.currentlySearching = false;
            return NodeState.FAILURE;
        }
        npcAI.colorIndicator.SetSearching();
        return NodeState.RUNNING;
    }
}
