using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemiesTask : BTNode
{
    NPCAI npcAI;

    public CheckForEnemiesTask(NPCAI npcAI)
    {
        this.npcAI = npcAI;
    }

    public override NodeState Evaluate()
    {
        return EnemySpotted() ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    public bool EnemySpotted()
    {
        //Vector3 direction = angle * Vector3.forward;
        //Debug.Log(angle);
        if (npcAI.npcHealth.currentHealth <= 0)
        {
            //npcAI.animator.SetBool("Alert", false);
            return false;
        }
        if (npcAI.currentTarget == null)
        {
            Collider[] hits = Physics.OverlapSphere(npcAI.visionTransform.position, npcAI.visionDistance);
            foreach (Collider hit in hits)
            {
                Vector3 direction = hit.transform.position - npcAI.visionTransform.position;
                float angle = Vector3.Angle(direction, npcAI.visionTransform.forward);
                float distance = Vector3.Distance(hit.transform.position, npcAI.visionTransform.position);
                //Debug.Log(npcAI.playerVisibility.lightLevel / distance);
                if (Mathf.Abs(angle) <= npcAI.visionAngle * 0.5f)
                {
                    RaycastHit ray;
                    if (Physics.Raycast(npcAI.visionTransform.position, direction, out ray, npcAI.visionDistance))
                    {
                        Debug.DrawRay(npcAI.visionTransform.position, direction * 4, Color.red);
                        if (ray.collider.GetComponent<Health>() && ray.collider.transform.root != npcAI.transform)
                        {
                            //if (npcOpinion.IsHostileCharacter(ray.collider.gameObject))
                            //{
                            if (npcAI.playerVisibility.lightLevel / distance >= npcAI.lightLevelThreshold)
                            {
                                npcAI.SetTarget(ray.collider.transform);
                                npcAI.TargetSetTargetInSight(true);
                                npcAI.lastKnownPosition = npcAI.currentTarget.position;
                                npcAI.colorIndicator.SetAngry();
                                npcAI.currentlySearching = false;
                                return true;
                            }
                            //chase_target = currentTarget.transform;
                            //vision.GetComponent<EnemyVision>().idle = false;
                            //npcAI.animator.SetBool("Alert", true);
                            //npcAI.npcQuip.Quip(npcAI.targetSightedQuip);
                            //npcAI.source.PlayOneShot(npcAI.targetSightedQuip);
                            //}
                        }
                        else if (ray.collider.GetComponent<DamageLocation>() && ray.collider.transform.root != npcAI.transform)
                        {
                            npcAI.SetTarget(ray.collider.transform);
                            npcAI.lastKnownPosition = npcAI.currentTarget.transform.position;
                            npcAI.colorIndicator.SetAngry();
                            npcAI.currentlySearching = false;
                            //npcAI.npcQuip.Quip(npcAI.targetSightedQuip);
                            return true;
                        }
                    }
                }
            }
            //npcOpinion.ClearModifiers();
            //npcAI.animator.SetBool("Alert", false);
            //npcAI.animator.SetBool("WalkingAlert", false);
            return false;
        }
        else if (npcAI.currentTarget != null)
        {
            Vector3 direction = npcAI.currentTarget.position - npcAI.visionTransform.position;
            float angle = Vector3.Angle(direction, npcAI.visionTransform.forward);
            if (Mathf.Abs(angle) <= npcAI.visionAngle * 0.75f)
            {
                Debug.Log("Player detected in angle");
                RaycastHit ray;
                Debug.DrawRay(npcAI.visionTransform.position, direction * 20, Color.red);
                if (Physics.Raycast(npcAI.visionTransform.position, direction, out ray, npcAI.visionDistance))
                {
                    Debug.DrawRay(npcAI.visionTransform.position, direction * 4, Color.red);
                    if (ray.transform == npcAI.currentTarget)
                    {
                        if (npcAI.currentTarget.GetComponent<Health>() && npcAI.currentTarget.GetComponent<Health>().currentHealth > 0)
                        {
                            //if (npcAI.playerVisibility.lightLevel >= npcAI.minLightLevelSight)
                            //{
                            npcAI.TargetSetTargetInSight(true);
                            npcAI.lastKnownPosition = npcAI.currentTarget.position;
                            npcAI.colorIndicator.SetAngry();
                            npcAI.currentlySearching = false;
                            return true;
                            //}
                        }
                        else if (npcAI.currentTarget.root.GetComponent<Health>() && npcAI.currentTarget.root.GetComponent<Health>().currentHealth > 0)
                        {
                            npcAI.lastKnownPosition = npcAI.currentTarget.transform.position;
                            npcAI.TargetSetTargetInSight(true);
                            npcAI.colorIndicator.SetAngry();
                            npcAI.currentlySearching = false;
                            return true;
                        }
                    }
                }
            }
            npcAI.TargetSetTargetInSight(false);
            return false;
        }
        else
        {
            //npcAI.animator.SetBool("Alert", false);
            //npcAI.source.PlayOneShot(npcAI.allClearQuip);
            //npcAI.npcQuip.Quip(npcAI.allClearQuip);
            return false;
        }
    }
}
