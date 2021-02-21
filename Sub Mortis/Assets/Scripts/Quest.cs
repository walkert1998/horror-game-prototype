using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string questGiver;
    public bool completed;
    public bool failed;
    [TextArea]
    public string description;
    public ObjectiveNode currentObjective;
    public List<ObjectiveNode> objectives;
    //public List<QuestReward> rewards;

    public void FailQuest()
    {
        failed = true;
    }

    public void CompleteQuest()
    {
        completed = true;
    }
}
