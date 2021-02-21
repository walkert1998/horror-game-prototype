using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveNode
{
    public int id;
    public bool active;
    public ObjectiveGoal objective;
    [TextArea]
    public string description;
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignObject()
    {
        objective.playerInventory = playerInventory;
        //Debug.Log(description + " " + objective.goal);
        if (objective.goal.Equals(GoalType.Kill))
        {
            if (objective.killTarget != null)
            {
                QuestRelated qr = objective.killTarget.gameObject.AddComponent<QuestRelated>() as QuestRelated;
                qr.objective = this;
                qr.objective.objective = objective;
                qr.type = objective.goal;
                //objective.killTarget.gameObject.GetComponent<QuestRelated>().objective = this;
                //objective.killTarget.gameObject.GetComponent<QuestRelated>().type = objective.goal;
                Debug.Log(objective.killTarget.gameObject + " " + objective.killTarget.gameObject.GetComponent<QuestRelated>().objective.description);
            }
            else
            {
                objective.CompleteObjectiveGoal();
            }
        }
        else if (objective.goal.Equals(GoalType.Conversation))
        {
            //if (objective.conversationTarget != null)
            //{
            //    QuestRelated qr = objective.conversationTarget.gameObject.AddComponent<QuestRelated>() as QuestRelated;
            //    qr.objective = this;
            //    qr.objective.objective = objective;
            //    qr.type = objective.goal;
            //    /*
            //    objective.conversationTarget.gameObject.AddComponent<QuestRelated>();
            //    objective.conversationTarget.GetComponent<QuestRelated>().objective = this;
            //    objective.conversationTarget.GetComponent<QuestRelated>().type = objective.goal;
            //    Debug.Log(objective.conversationTarget.gameObject + " " + objective.conversationTarget.gameObject.GetComponent<QuestRelated>().objective.description);
            //    */
            //}
        }
        else if (objective.questItem != null)
        {
            //QuestRelated qr = objective.questItem.gameObject.AddComponent<QuestRelated>() as QuestRelated;
            //qr.objective = this;
            //qr.objective.objective = objective;
            //qr.type = objective.goal;
            /*
            objective.questItem.gameObject.AddComponent<QuestRelated>();
            objective.questItem.GetComponent<QuestRelated>().objective = this;
            objective.questItem.GetComponent<QuestRelated>().type = objective.goal;
            Debug.Log(objective.questItem.gameObject + " " + objective.questItem.gameObject.GetComponent<QuestRelated>().objective.description);
            */
        }
        else if (objective.goal.Equals(GoalType.Travel))
        {
            if (objective.objectiveDestination != null)
            {
                QuestRelated qr = objective.objectiveDestination.gameObject.AddComponent<QuestRelated>() as QuestRelated;
                qr.objective = this;
                qr.objective.objective = objective;
                qr.type = objective.goal;
                /*
                objective.objectiveDestination.gameObject.AddComponent<QuestRelated>();
                objective.objectiveDestination.GetComponent<QuestRelated>().objective = this;
                objective.objectiveDestination.GetComponent<QuestRelated>().type = objective.goal;
                Debug.Log(objective.objectiveDestination.gameObject + " " + objective.objectiveDestination.gameObject.GetComponent<QuestRelated>().objective.description);
                */
            }
            else
            {
                objective.CompleteObjectiveGoal();
            }
        }
    }
}
