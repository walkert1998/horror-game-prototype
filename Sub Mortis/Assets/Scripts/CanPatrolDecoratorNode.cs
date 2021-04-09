using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPatrolDecoratorNode : BTNode
{
    NPCAI npcAI;
    BTNode patrolSequence;

    public CanPatrolDecoratorNode(NPCAI npcAI, BTNode patrolSequence)
    {
        this.npcAI = npcAI;
        this.patrolSequence = patrolSequence;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.patrolPoints.Count > 0 && npcAI.currentTarget == null)
        {
            npcAI.SetSpeedToWalk();
            patrolSequence.Evaluate();
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
