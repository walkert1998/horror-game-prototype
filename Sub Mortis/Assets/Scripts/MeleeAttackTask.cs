using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTask : BTNode
{
    NPCAI npcAI;
    float timer;

    public MeleeAttackTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
        timer = npcAI.timeBetweenAttacks;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.currentTarget == null)
        {
            timer = npcAI.timeBetweenAttacks;
            return NodeState.FAILURE;
        }
        //Debug.Log(timer);
        if (timer < npcAI.timeBetweenAttacks && !npcAI.attacking)
        {
            timer += Time.deltaTime;
            return NodeState.RUNNING;
        }
        else if (timer >= npcAI.timeBetweenAttacks && !npcAI.attacking)
        {
            npcAI.attacking = true;
            npcAI.agent.isStopped = true;
            npcAI.colorIndicator.SetAngry();
            npcAI.currentlySearching = false;
            npcAI.currentTarget.SendMessage("DamageCharacter", 10, SendMessageOptions.DontRequireReceiver);
            timer = 0;
            return NodeState.RUNNING;
        }
        else if (npcAI.attacking)
        {
            timer += Time.deltaTime;
            if (timer >= npcAI.meleeAttackTime)
            {
                npcAI.attacking = false;
                npcAI.agent.isStopped = false;
                timer = 0;
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
        timer = npcAI.timeBetweenAttacks;
        return NodeState.FAILURE;
    }
}
