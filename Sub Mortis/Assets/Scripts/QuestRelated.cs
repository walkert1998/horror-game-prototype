using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRelated : MonoBehaviour
{
    public ObjectiveNode objective;
    public GoalType type;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("QuestRelated Object assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectiveCompleted() || ObjectiveFailed())
            UpdateQuest(false);
    }

    public bool ObjectiveCompleted()
    {
        //if (type.Equals(GoalType.Kill))
        //{
        //    if (GetComponent<EnemyController>().dead)
        //        return true;
        //}
        //else if (type.Equals(GoalType.Retrieve))
        //{
        //    if (GetComponent<PickUp>().picked_up)
        //        return true;
        //}
        return false;
    }

    public bool ObjectiveFailed()
    {
        //if (type.Equals(GoalType.CharacterAlive))
        //{
        //    if (GetComponent<EnemyController>().dead)
        //        return true;
        //}
        //else if (type.Equals(GoalType.Conversation))
        //{
        //    if (GetComponent<EnemyController>().dead)
        //        return true;
        //}
        return false;
    }

    public void UpdateQuest(bool conversationPass)
    {
        if (ObjectiveCompleted())
        {
            if (!type.Equals(GoalType.Conversation))
            {
                Debug.Log(objective);
                GameObject.FindGameObjectWithTag("Player").SendMessage("CompleteObjective", objective, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (type.Equals(GoalType.Conversation) && conversationPass)
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("CompleteObjective", objective, SendMessageOptions.DontRequireReceiver);
        }
        else if (type.Equals(GoalType.Conversation) && !conversationPass)
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("FailObjective", objective, SendMessageOptions.DontRequireReceiver);
        }
        else if (type.Equals(GoalType.Travel) && conversationPass)
        {

            GameObject.FindGameObjectWithTag("Player").SendMessage("CompleteObjective", objective, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("FailObjective", objective, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(this);
    }
}
