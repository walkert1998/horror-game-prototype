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
        float distance = Vector3.Distance(npcAI.transform.position, npcAI.lastKnownPosition);
        RaycastHit hit;
        if (Physics.Raycast(npcAI.visionTransform.position, npcAI.visionTransform.forward, out hit))
        {
            if (hit.transform == npcAI.currentTarget)
            {
                npcAI.lastKnownPosition = npcAI.currentTarget.position;
                Debug.Log("Player seen at " + npcAI.lastKnownPosition);
            }
        }
        if (distance > 20)
        {
            npcAI.agent.isStopped = false;
            //npcAI.animator.SetBool("WalkingNormal", true);
            npcAI.animator.SetBool("WalkingAlert", true);
            npcAI.agent.SetDestination(npcAI.lastKnownPosition);
            return NodeState.RUNNING;
        }
        else
        {
            npcAI.agent.isStopped = true;
            //npcAI.animator.SetBool("WalkingNormal", false);
            npcAI.animator.SetBool("WalkingAlert", false);
            return NodeState.SUCCESS;
        }
    }
}
