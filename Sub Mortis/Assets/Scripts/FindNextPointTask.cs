using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNextPointTask : BTNode
{
    List<Transform> targets;
    NPCAI npcAI;
    int currTarget;

    public FindNextPointTask(List<Transform> targets, NPCAI npcAI)
    {
        this.targets = targets;
        this.npcAI = npcAI;
        this.currTarget = 0;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget != null || npcAI.patrolPoints.Count <= 0)
        {
            return NodeState.FAILURE;
        }
        //Debug.Log(npcAI.patrolTarget);
        float distance = Vector3.Distance(npcAI.transform.position, targets[currTarget].position);
        if (npcAI.patrolTarget == null)
        {
            npcAI.patrolTarget = targets[currTarget];
        }
        else if (npcAI.patrolTarget != null && npcAI.currentTarget == null)
        {
            if (currTarget == targets.Count)
            {
                currTarget = 0;
            }
            else if (distance <= 1.0f)
            {
                currTarget++;
            }
            npcAI.patrolTarget = targets[currTarget];
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
