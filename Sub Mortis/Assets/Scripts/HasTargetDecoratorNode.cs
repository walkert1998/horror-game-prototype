using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTargetDecoratorNode : BTNode
{
    NPCAI npcAI;
    BTNode child;

    public HasTargetDecoratorNode(NPCAI npcAI, BTNode child)
    {
        this.npcAI = npcAI;
        this.child = child;
    }

    public override NodeState Evaluate()
    {
        if (EnemySpotted())
        {
            child.Evaluate();
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
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
                        if (ray.collider.GetComponent<Health>() && ray.collider.transform.root != npcAI.transform)
                        {
                            //if (npcOpinion.IsHostileCharacter(ray.collider.gameObject))
                            //{
                            if (npcAI.playerVisibility.lightLevel >= npcAI.lightLevelThreshold)
                            {
                                npcAI.SetTarget(ray.collider.transform);
                                npcAI.TargetSetTargetInSight(true);
                                npcAI.lastKnownPosition = npcAI.currentTarget.position;
                                return true;
                            }
                            else if (npcAI.playerVisibility.lightLevel >= npcAI.minLightLevelSight)
                            {
                                npcAI.SetTarget(ray.collider.transform);
                                npcAI.TargetSetTargetInSight(true);
                                npcAI.lastKnownPosition = npcAI.currentTarget.position;
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
            Debug.DrawRay(npcAI.visionTransform.position, direction * npcAI.visionDistance, Color.red);
            if (Mathf.Abs(angle) <= npcAI.visionAngle * 0.75f)
            {
                Debug.Log("Player detected in angle");
                RaycastHit ray;
                if (Physics.Raycast(npcAI.visionTransform.position, direction, out ray, npcAI.visionDistance))
                {
                    //Debug.DrawRay(npcAI.visionTransform.position, direction * 4, Color.magenta);
                    if (ray.transform == npcAI.currentTarget)
                    {
                        if (npcAI.currentTarget.GetComponent<Health>() && npcAI.currentTarget.GetComponent<Health>().currentHealth > 0)
                        {
                            if (npcAI.playerVisibility.lightLevel >= npcAI.lightLevelThreshold * 0.75f)
                            {
                                npcAI.TargetSetTargetInSight(true);
                                npcAI.lastKnownPosition = npcAI.currentTarget.position;
                                return true;
                            }
                        }
                        else if (npcAI.currentTarget.root.GetComponent<Health>() && npcAI.currentTarget.root.GetComponent<Health>().currentHealth > 0)
                        {
                            npcAI.lastKnownPosition = npcAI.currentTarget.transform.position;
                            npcAI.TargetSetTargetInSight(true);
                            return true;
                        }
                    }
                }
            }
            Debug.Log("Target not in sight");
            npcAI.TargetSetTargetInSight(false);
            return true;
        }
        else
        {
            //npcAI.animator.SetBool("Alert", false);
            //npcAI.source.PlayOneShot(npcAI.allClearQuip);
            //npcAI.npcQuip.Quip(npcAI.allClearQuip);
            npcAI.TargetSetTargetInSight(false);
            return false;
        }
    }
}
