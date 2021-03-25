using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNextPointTask : BTNode
{
    List<Transform> patrolPoints;
    NPCAI npcAI;
    int currTarget;

    public FindNextPointTask(List<Transform> targets, NPCAI npcAI)
    {
        patrolPoints = targets;
        this.npcAI = npcAI;
        currTarget = 0;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.patrolPoints.Count <= 0)
        {
            return NodeState.FAILURE;
        }
        float distance = Vector3.Distance(npcAI.transform.position, patrolPoints[currTarget].position);
        if (npcAI.patrolTarget == null)
        {
            npcAI.patrolTarget = patrolPoints[currTarget];
            npcAI.targetDestination = patrolPoints[currTarget].position;
        }
        else if (npcAI.patrolTarget != null)
        {
            npcAI.patrolNum = currTarget;
            npcAI.patrolTarget = patrolPoints[currTarget];
            npcAI.targetDestination = patrolPoints[currTarget].position;
            if (distance <= npcAI.agent.stoppingDistance && !npcAI.waiting)
            {
                currTarget++;
            }
            if (currTarget == patrolPoints.Count)
            {
                currTarget = 0;
            }
            Debug.Log(npcAI.patrolTarget);
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }
}
