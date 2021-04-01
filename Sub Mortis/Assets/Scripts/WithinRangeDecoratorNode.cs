using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinRangeDecoratorNode : BTNode
{
    NPCAI npcAI;
    float distance;

    public WithinRangeDecoratorNode(NPCAI npcAI, float distance)
    {
        this.npcAI = npcAI;
        this.distance = distance;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(npcAI.transform.position, npcAI.currentTarget.position);
        //Debug.Log(dist + " " + distance + " " + npcAI.targetInSight);
        if (dist <= distance && npcAI.targetInSight)
        {
            return NodeState.SUCCESS;
        }
        else if (npcAI.attacking)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
