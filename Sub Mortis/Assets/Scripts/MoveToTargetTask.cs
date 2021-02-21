using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetTask : BTNode
{
    Transform agentPosition;
    Transform targetPosition;
    float range;
    NPCAI npcAI;

    public MoveToTargetTask(NPCAI _npcAI, Transform origin, Transform target, float range)
    {
        this.npcAI = _npcAI;
        this.agentPosition = origin;
        this.targetPosition = target;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        //if (npcAI.agent.destination != targetPosition.position)
        //{
        //    return NodeState.FAILURE;
        //}
        //Debug.Log(targetPosition);
        if (targetPosition == null)
        {
            return NodeState.FAILURE;
        }
        float distance = Vector3.Distance(agentPosition.position, targetPosition.position);
        if (distance > range)
        {
            npcAI.animator.SetBool("WalkingNormal", true);
            //Debug.Log(distance + " " + range);
            npcAI.agent.isStopped = false;
            //Debug.Log(npcAI.agent.destination);
            bool check = npcAI.agent.SetDestination(targetPosition.position);
            //Debug.Log(check);
            return NodeState.RUNNING;
        }
        else
        {
            npcAI.agent.isStopped = true;
            npcAI.animator.SetBool("WalkingNormal", false);
            //Debug.Log("Point " + targetPosition + " reached");
            return NodeState.SUCCESS;
        }
    }
}
