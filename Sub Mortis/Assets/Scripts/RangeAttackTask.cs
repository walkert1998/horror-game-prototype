using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackTask : BTNode
{
    NPCAI npcAI;
    float timer;

    public RangeAttackTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
        this.timer = npcAI.weaponManager.timeBetweenShots;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.weaponManager.currentAmmo <= 0 || !npcAI.targetInSight)
        {
            return NodeState.FAILURE;
        }
        timer += Time.deltaTime;
        TurnToTarget();
        if (timer > npcAI.weaponManager.timeBetweenShots)
        {
            npcAI.agent.isStopped = true;
            npcAI.visionTransform.LookAt(npcAI.lastKnownPosition);
            npcAI.weaponManager.FireWeapon();
            //npcAI.source.PlayOneShot(npcAI.attackQuip);
            timer = 0;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
        //npcAI.StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.1f);
    }

    public void TurnToTarget()
    {
        Vector3 desiredPosition = new Vector3(npcAI.lastKnownPosition.x, npcAI.transform.position.y, npcAI.lastKnownPosition.z);
        Quaternion rotationalGoal = Quaternion.LookRotation(desiredPosition);
        //npcAI.spine.transform.LookAt(npcAI.currentTarget);
        //Debug.Log("Turning");
        npcAI.transform.LookAt(desiredPosition);
    }
}
