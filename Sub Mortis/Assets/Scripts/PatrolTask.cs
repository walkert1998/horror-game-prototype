using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTask : BTNode
{
    Vector3 agentPosition;
    Vector3 targetPosition;
    List<Transform> waypoints;
    float range;
    NPCAI npcAI;
    int destination;

    public PatrolTask(NPCAI _npcAI, List<Transform> patrolPoints, float range)
    {
        this.npcAI = _npcAI;
        this.range = range;
        this.waypoints = patrolPoints;
        this.destination = 0;
    }

    public override NodeState Evaluate()
    {
        npcAI.agent.SetDestination(waypoints[destination].position);
        float distance = Vector3.Distance(npcAI.agent.transform.position, npcAI.agent.destination);
        
        if (distance > range)
        {
            npcAI.animator.SetBool("WalkingNormal", true);
            //if (npcAI.agent.destination != npcAI.agent.transform.position)
            //{
            //    return NodeState.FAILURE;
            //}
            npcAI.agent.isStopped = false;
            bool check = npcAI.agent.SetDestination(waypoints[destination].position);
            return NodeState.RUNNING;
        }
        else
        {
            npcAI.agent.isStopped = true;
            npcAI.animator.SetBool("WalkingNormal", false);
            //destination = 0;
            destination = (destination + 1) % waypoints.Count;
            return NodeState.SUCCESS;
        }
    }
}
