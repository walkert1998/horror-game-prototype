using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public NavMeshAgent agent;
    public NodeSelector baseLevel;
    public Health monsterHealth;

    NodeSequence patrolSequence;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
