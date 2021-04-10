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
    public PlayerIllumination playerVisibility;
    public NPCWeaponManager weaponManager;
    public NodeSequence attackSequence;
    public NodeSequence patrolSequence;
    public NodeSequence reloadSequence;
    public NodeSequence chaseSequence;
    public NodeSequence searchSequence;
    public NodeSequence investigateSequence;
    public NodeSelector topNode;
    public NodeSelector hasTargetSelector;
    public NodeSelector searchOptions;
    public NodeRepeater baseRepeat;
    public CanPatrolDecoratorNode hasPatrolNode;
    public HasTargetDecoratorNode hasTarget;
    public SearchingForTargetDecorator searching;
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
    public GameMenu gameMenu;
    public NPCColorChange colorIndicator;
    public float visionDistance;
    public bool targetInSight = false;
    public bool waiting = false;
    public bool targetReached = false;
    public bool staggered = false;
    public bool attacking = false;
    public bool currentlySearching = false;
    public GameObject head;
    public GameObject spine;
    public int patrolNum;
    public float minLightLevelSight = 20.0f;
    public float lightLevelThreshold = 40.0f;
    public float meleeAttackTime = 3.0f;
    public float timeBetweenAttacks = 3.0f;
    public float awarenessTimer = 30.0f;
    public float currentAwarenessTime = 0.0f;
    public float suspicion = 0.0f;
    public float investigateSuspiciousThreshold = 10.0f;
    public float minHearingVolume = 2.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;

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
        colorIndicator.SetNormal();
        patrolSequence = new NodeSequence(new List<BTNode> { new FindNextPointTask(patrolPoints, this), new MoveToTargetTask(this, agent.stoppingDistance), new WaitForTimeTask(this, 3.0f) });
        hasPatrolNode = new CanPatrolDecoratorNode(this, patrolSequence);
        searchSequence = new NodeSequence(new List<BTNode> { new FindRandomPoint(10, this, -1), new MoveToTargetTask(this, agent.stoppingDistance), new WaitForTimeTask(this, 1.0f) });
        searchOptions = new NodeSelector(new List<BTNode> { new CheckForEnemiesTask(this), new NodeSequence(new List<BTNode> { new FindLastKnownTargetPositionTask(this), new MoveToTargetTask(this, agent.stoppingDistance), new WaitForTimeTask(this, 1.0f) }), searchSequence });
        searching = new SearchingForTargetDecorator(this, new NodeRepeater(-1, searchOptions) );
        chaseSequence = new NodeSequence(new List<BTNode> { new CheckForEnemiesTask(this), new ChaseTask(this), new MoveToTargetTask(this, agent.stoppingDistance) });
        attackSequence = new NodeSequence(new List<BTNode> { new WithinRangeDecoratorNode(this, 1.5f), new MeleeAttackTask(this) });
        investigateSequence = new NodeSequence(new List<BTNode> { new FindLastKnownTargetPositionTask(this), new MoveToTargetTask(this, agent.stoppingDistance), new WaitForTimeTask(this, 1.0f) });
        hasTargetSelector = new NodeSelector(new List<BTNode> { attackSequence, chaseSequence, searching });
        hasTarget = new HasTargetDecoratorNode(this, hasTargetSelector);
        topNode = new NodeSelector(new List<BTNode> { hasTarget, hasPatrolNode });
        baseRepeat = new NodeRepeater(-1, topNode);
    }

    // Update is called once per frame
    void Update()
    {
        //if (npcHealth.currentHealth > 0)
        //{
        if (!gameMenu.gamePaused)
        {
            topNode.Evaluate();
            if (targetInSight)
            {
                transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
            }
        }
        Debug.DrawRay(visionTransform.position, visionTransform.forward * visionDistance, Color.red);
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

    public void TargetSetTargetInSight(bool value)
    {
        Debug.Log("Setting target in sight to " + value);
        targetInSight = value;
    }

    public void SetSpeedToWalk()
    {
        agent.speed = walkSpeed;
    }

    public void SetSpeedToRun()
    {
        agent.speed = runSpeed;
    }

    public void HeardSound(Vector3 position, float volume)
    {
        float distance = Vector3.Distance(position, transform.position);
        //Debug.Log((volume / distance) * 10 + " " + volume + " " + distance);
        if (volume/distance * 10 > minHearingVolume)
        {
            lastKnownPosition = position;
            suspicion = 10.0f;
            Debug.Log("Heard something");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SetTarget(collision.transform);
        }
    }

    //public void DamageCharacter(int amount)
    //{
    //    npcQuip.Quip(painQuip, true);
    //}
}
