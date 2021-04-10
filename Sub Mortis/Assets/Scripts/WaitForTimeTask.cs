using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTimeTask : BTNode
{
    float timeInSeconds;
    float timer;
    NPCAI npcAI;
    public WaitForTimeTask(NPCAI ai, float time)
    {
        timeInSeconds = time;
        timer = 0.0f;
        npcAI = ai;
    }

    public override NodeState Evaluate()
    {
        if (timer >= timeInSeconds)
        {
            timer = 0;
            npcAI.waiting = false;
            return NodeState.SUCCESS;
        }
        else if (npcAI.agent.isStopped && timer < timeInSeconds)
        {
            if (npcAI.currentTarget != null)
            {
                npcAI.animator.SetBool("Aggressive", true);
                npcAI.animator.SetBool("Running", false);
            }
            else
            {
                //npcAI.animator.SetBool("Aggressive", true);
                npcAI.animator.SetBool("Walking", false);
            }
            timer += Time.deltaTime;
            //Debug.Log(timer);
            npcAI.waiting = true;
            return NodeState.RUNNING;
        }
        timer = 0;
        npcAI.waiting = false;
        return NodeState.FAILURE;
    }

    IEnumerator TickTime()
    {
        while (timer < timeInSeconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
