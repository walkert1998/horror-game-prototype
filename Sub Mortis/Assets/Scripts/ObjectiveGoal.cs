using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveGoal
{
    public bool objectiveComplete;
    public bool objectiveFailed;
    public bool objectiveOptional;
    public GoalType goal;
    public int quantity;
    public Health killTarget;
    //public Conversation conversationTarget;
    public ObjectiveDestination objectiveDestination;
    public Item questItem;
    public string questItemName;
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ObjectiveFailed ()
    {
        //if (goal.Equals(GoalType.Kill) && (killTarget != null && !killTarget.dead) || killTarget == null)
        //{
        //    return true;
        //}
        //else if (goal.Equals(GoalType.CharacterAlive) && killTarget.dead || killTarget == null)
        //{
        //    return true;
        //}
        //else if (goal.Equals(GoalType.Conversation) && conversationTarget.gameObject.GetComponent<EnemyController>().dead)
        //{
        //    return true;
        //}
        return false;
    }

    public bool ObjectiveComplete()
    {
        if (questItem != null)
        {
            questItemName = questItem.itemName;
        }
        //if (goal.Equals(GoalType.Kill) && killTarget != null && killTarget.dead && !objectiveOptional)
        //{
        //    return true;
        //}
        //else if (goal.Equals(GoalType.CharacterAlive) && !killTarget.dead)
        //{
        //    return true;
        //}
        //else if (goal.Equals(GoalType.Conversation))
        //{
        //    if (conversationTarget != null && !conversationTarget.gameObject.GetComponent<EnemyController>().dead)
        //    {
        //        return true;
        //    }
        //}
        else if (goal.Equals(GoalType.Retrieve) &&  playerInventory.FindItem(questItem) != null)
        {
            return true;
        }
        else if (goal.Equals(GoalType.Travel) && objectiveDestination == null)
        {
            return true;
        }
        return false;
    }

    public void CompleteObjectiveGoal()
    {
        //objectiveComplete = ObjectiveComplete();
        objectiveComplete = true;
        //Debug.Log(objectiveComplete);
    }

    public void FailObjective()
    {
        objectiveFailed = ObjectiveFailed();
        Debug.Log(objectiveComplete);
    }
}

public enum GoalType
{
    Kill,
    Retrieve,
    Travel,
    CharacterAlive,
    Conversation,
    Other
}
