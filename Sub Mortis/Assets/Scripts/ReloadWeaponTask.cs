using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeaponTask : BTNode
{
    NPCAI npcAI;
    float timer;
    bool reloading = false;

    public ReloadWeaponTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
        this.timer = 0;
    }

    public override NodeState Evaluate()
    {
        if (npcAI.weaponManager.currentAmmo > 0)
        {
            return NodeState.FAILURE;
        }
        if (!reloading)
        {
            npcAI.npcQuip.Quip(npcAI.reloadingQuip);
            npcAI.animator.Play("Reload");
        }
        reloading = true;
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer > npcAI.animator.GetCurrentAnimatorStateInfo(0).length)
        {
            npcAI.animator.SetBool("WalkingNormal", false);
            npcAI.weaponManager.Reload();
            timer = 0;
            reloading = false;
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }
}
