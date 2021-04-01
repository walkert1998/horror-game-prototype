using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLineOfSightTask : BTNode
{
    NPCAI npcAI;

    public GetLineOfSightTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget == null)
        {
            return NodeState.FAILURE;
        }
        npcAI.animator.SetBool("Alert", true);
        RaycastHit hit;
        Vector3 direction = npcAI.currentTarget.position - npcAI.visionTransform.position;
        //direction.z = -direction.z;
        //direction.x = -direction.x;
        //direction.y = -direction.y;
        Debug.DrawRay(npcAI.visionTransform.position, direction * npcAI.visionDistance, Color.blue);
        float angle = Vector3.Angle(direction, npcAI.visionTransform.forward);
        if (angle < npcAI.visionAngle * 0.5f)
        {
            if (Physics.Raycast(npcAI.visionTransform.position, direction.normalized * npcAI.visionDistance, out hit))
            {
                if (hit.transform == npcAI.currentTarget)
                {
                    //Debug.Log("Found target");
                    npcAI.animator.SetBool("WalkingNormal", false);
                    npcAI.animator.SetBool("WalkingAlert", false);
                    npcAI.npcQuip.Quip(npcAI.foundTargetQuip);
                    npcAI.TargetSetTargetInSight(true);
                    npcAI.lastKnownPosition = npcAI.currentTarget.position;
                    return NodeState.SUCCESS;
                }
            }
        }
        //Debug.Log("Looking");
        float distance = Vector3.Distance(npcAI.transform.position, npcAI.agent.destination);
        if (distance > 1)
        {
            npcAI.TargetSetTargetInSight(false);
            npcAI.animator.SetBool("WalkingNormal", false);
            npcAI.animator.SetBool("WalkingAlert", true);
            //npcAI.source.PlayOneShot(npcAI.outOfRangeQuip);
            npcAI.npcQuip.Quip(npcAI.outOfRangeQuip);
            npcAI.agent.isStopped = false;
            bool check = npcAI.agent.SetDestination(npcAI.lastKnownPosition);
        }
        else
        {
            npcAI.TargetSetTargetInSight(false);
            npcAI.animator.SetBool("WalkingNormal", false);
            npcAI.animator.SetBool("WalkingAlert", false);
            //npcAI.animator.SetBool("Alert", false);
            //npcAI.source.PlayOneShot(npcAI.outOfRangeQuip);
            npcAI.npcQuip.Quip(npcAI.allClearQuip);
            npcAI.agent.isStopped = true;
        }
        return NodeState.RUNNING;
    }
}
