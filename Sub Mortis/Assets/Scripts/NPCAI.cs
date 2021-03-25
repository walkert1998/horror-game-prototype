using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    //[SerializeField]
    public List<Transform> patrolPoints;
    public Health npcHealth;
    public NavMeshAgent agent;
    public NPCWeaponManager weaponManager;
    public NodeSequence attackSequence;
    public NodeSequence patrolSequence;
    public NodeSequence reloadSequence;
    public NodeSelector topNode;
    public NodeRepeater baseRepeat;
    public Transform currentTarget;
    public Transform patrolTarget;
    public Vector3 lastKnownPosition;
    public Vector3 targetDestination;
    public float attackRange;
    public int visionAngle;
    public Transform visionTransform;
    public PatrolTask patrol;
    public MoveToTargetTask move;
    public RangeAttackTask rangeAttack;
    public CheckForEnemiesTask checkForEnemies;
    public FindNextPointTask findPoint;
    public ChaseTask chaseTask;
    public GetLineOfSightTask getLineOfSight;
    public Animator animator;
    public ReloadWeaponTask reloadWeapon;
    public FindCoverTask findCover;
    public NPCColliders colliders;
    public AudioSource source;
    public float visionDistance;
    public bool targetInSight = false;
    public bool waiting = false;
    public GameObject head;
    public GameObject spine;
    public int patrolNum;

    [Header("Vocal Quips")]
    public NPCQuip npcQuip;
    public AudioClip targetSightedQuip;
    public AudioClip attackQuip;
    public AudioClip outOfRangeQuip;
    public AudioClip painQuip;
    public AudioClip deathQuip;
    public AudioClip targetDownQuip;
    public AudioClip allClearQuip;
    public AudioClip foundTargetQuip;
    public AudioClip reloadingQuip;
    //public AudioClip 

    // Start is called before the first frame update
    void Start()
    {
        //attackSequence = new NodeSequence(new List<BTNode>
        //{
        //    new MoveToTargetTask(this, transform.position, currentTarget.position, attackRange),
        //    new IsTargetVisibleTask(this),
        //    new RangeAttackTask(this)
        //});
        //patrol = new PatrolTask(this, patrolPoints, 0);
        patrolNum = 0;
        source = GetComponent<AudioSource>();
        if (patrolPoints.Count > 0)
        {
            targetDestination = patrolPoints[0].position;
        }
        patrolSequence = new NodeSequence(new List<BTNode> { new FindNextPointTask(patrolPoints, this), new WaitForTimeTask(this, 3.0f),  new MoveToTargetTask(this, agent.stoppingDistance) });
        topNode = new NodeSelector(new List<BTNode> { patrolSequence });
        baseRepeat = new NodeRepeater(-1, topNode);
    }

    // Update is called once per frame
    void Update()
    {
        //if (npcHealth.currentHealth > 0)
        //{
            NodeState state = topNode.Evaluate();
            Debug.Log(topNode);
        //}
        //Debug.Log(state);
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    KnockOver();
        //}
        //if (npcHealth.currentHealth <= 0 && agent.enabled)
        //{
        //    KnockOver();
        //    //source.PlayOneShot(deathQuip);
        //    npcQuip.Quip(deathQuip, true);
        //    colliders.DisableDamageSensors();
        //    this.enabled = false;
        //}
    }

    //private void LateUpdate()
    //{
    //    if (targetInSight)
    //    {
    //        spine.transform.LookAt(currentTarget);
    //    }
    //}

    public List<BTNode> ConstructPatrolSequence()
    {
        List<BTNode> patrolNodes = new List<BTNode>();
        Transform previousPoint = patrolPoints[patrolPoints.Count - 1];
        foreach(Transform point in patrolPoints)
        {
            //patrolNodes.Add(new MoveToTargetTask(this, transform, point.position, agent.stoppingDistance));
            //patrolNodes.Add(new WaitForTimeTask(3.0f));
            previousPoint = point;
        }
        return patrolNodes;
    }

    public void KnockOver()
    {
        animator.enabled = false;
        colliders.EnableRagdoll();
        agent.enabled = false;
    }

    public void SetTarget(Transform target)
    {
        Debug.Log("Setting target to: " + target);
        currentTarget = target;
        if (target != null)
            lastKnownPosition = target.position;
    }

    public void DamageCharacter(int amount)
    {
        npcQuip.Quip(painQuip, true);
    }
}
