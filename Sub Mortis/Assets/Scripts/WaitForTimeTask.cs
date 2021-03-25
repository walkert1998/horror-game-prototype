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
        Debug.Log("Wait reached");
        if (timer >= timeInSeconds)
        {
            timer = 0;
            npcAI.waiting = false;
            return NodeState.SUCCESS;
        }
        else if (timer < timeInSeconds)
        {
            timer += Time.deltaTime;
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
