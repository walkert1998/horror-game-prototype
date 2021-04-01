using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetTask : BTNode
{
    float stoppingDistance;
    NPCAI npcAI;

    public MoveToTargetTask(NPCAI _npcAI, float range)
    {
        npcAI = _npcAI;
        stoppingDistance = range;
    }

    public override NodeState Evaluate()
    {
        //if (npcAI.agent.destination != targetPosition.position)
        //{
        //    return NodeState.FAILURE;
        //}
        //Debug.Log(npcAI.targetDestination);
        if (npcAI.targetDestination == null)
        {
            return NodeState.FAILURE;
        }
        float distance = Vector3.Distance(npcAI.transform.position, npcAI.targetDestination);
        if (distance > stoppingDistance && !npcAI.waiting)
        {
            //npcAI.animator.SetBool("WalkingNormal", true);
            //Debug.Log(distance);
            npcAI.targetReached = false;
            npcAI.agent.isStopped = false;
            //Debug.Log(npcAI.agent.destination);
            bool check = npcAI.agent.SetDestination(npcAI.targetDestination);
            return NodeState.RUNNING;
        }
        else
        {
            npcAI.agent.isStopped = true;
            npcAI.targetReached = true;
            //npcAI.animator.SetBool("WalkingNormal", false);
            //Debug.Log("Point " + npcAI.targetDestination + " reached");
            return NodeState.SUCCESS;
        }
    }
}
