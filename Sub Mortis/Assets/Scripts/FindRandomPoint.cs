using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindRandomPoint : BTNode
{
    public float radius;
    public NPCAI npcAI;
    int layerMask;
    Vector3 randomDirection;
    bool positionFound;

    public FindRandomPoint(float radius, NPCAI npcAI, int layerMask)
    {
        this.radius = radius;
        this.npcAI = npcAI;
        this.layerMask = layerMask;
        positionFound = false;
    }

    public override NodeState Evaluate()
    {
        randomDirection = Random.insideUnitSphere * radius;
        randomDirection += npcAI.transform.position;
        NavMeshHit navHit;
        float distance = Vector3.Distance(npcAI.transform.position, npcAI.targetDestination);
        if (positionFound && !npcAI.waiting)
        {
            //Debug.Log("Got here1 " + distance);
            if (npcAI.agent.isStopped)
            {
                positionFound = NavMesh.SamplePosition(randomDirection, out navHit, radius, layerMask);
                if (positionFound)
                {
                    npcAI.targetDestination = new Vector3(navHit.position.x, npcAI.transform.position.y, navHit.position.z);
                    //Debug.Log("New point: " + npcAI.targetDestination);
                    return NodeState.SUCCESS;
                }
                else
                {
                    //Debug.Log("Got here2");
                    return NodeState.RUNNING;
                }
            }
            //float distance = Vector3.Distance(npcAI.transform.position, navHit.position);
            //if (distance <= npcAI.agent.stoppingDistance)
            //{
            //    return NodeState.SUCCESS;
            //}
        }
        else if (!positionFound && !npcAI.waiting)
        {
            positionFound = NavMesh.SamplePosition(randomDirection, out navHit, radius, layerMask);
            //Debug.Log("Got here3");
            if (positionFound)
            {
                npcAI.targetDestination = new Vector3(navHit.position.x, npcAI.transform.position.y, navHit.position.z);
                //Debug.Log("New point: " + npcAI.targetDestination);
                return NodeState.SUCCESS;
            }
            else
            {
                //Debug.Log("Got here4");
                return NodeState.RUNNING;
            }
        }
        //Debug.Log("Got here5 " + !npcAI.waiting + " " + positionFound);
        return NodeState.SUCCESS;
    }
}
