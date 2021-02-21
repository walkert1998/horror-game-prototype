using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCoverTask : BTNode
{
    NPCAI npcAI;

    public FindCoverTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
    }

    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}
