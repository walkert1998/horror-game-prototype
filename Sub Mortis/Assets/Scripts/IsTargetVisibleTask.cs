using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetVisibleTask : BTNode
{
    private NPCAI npcAI;

    public IsTargetVisibleTask(NPCAI nPCAI)
    {
        this.npcAI = nPCAI;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget == null)
        {
            return NodeState.FAILURE;
        }

        RaycastHit hit;
        if(Physics.Raycast(npcAI.visionTransform.position, npcAI.visionTransform.forward, out hit))
        {
            if (hit.collider.transform.root == npcAI.currentTarget)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}