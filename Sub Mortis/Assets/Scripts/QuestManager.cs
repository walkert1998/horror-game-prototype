using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests;
    public List<Quest> failedQuests;
    public List<Quest> completedQuests;
    public List<Quest> allQuests;
    //public JournalPanel journalPanel;
    public AudioClip objectiveCompleteSound;
    public AudioClip objectiveFailedSound;
    public bool journalOpen = false;
    public RemindersScreen remindersApp;
    // Start is called before the first frame update
    void Start()
    {
        //journalPanel.SelectQuest(activeQuests[0]);
        //journalPanel.gameObject.SetActive(false);
        AcceptQuest(allQuests[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (journalOpen)
                CloseJournal();
            else
                OpenJournal();
        }
    }

    public void PopulateAllQuests(Quest[] quests)
    {
        foreach (Quest quest in quests)
        {
            allQuests.Add(quest);
        }
    }

    public bool QuestActive(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            return true;
        }
        return false;
    }

    public bool QuestActive(string quest)
    {
        foreach (Quest questEntry in activeQuests)
        {
            if (questEntry.title.Equals(quest))
            {
                return true;
            }
        }
        return false;
    }

    public void CompleteObjective(ObjectiveNode objective)
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.objectives.Contains(objective))
            {
                CompleteObjective(quest, objective);
            }
        }
        //objective.objective.CompleteObjectiveGoal();
        GetComponent<AudioSource>().PlayOneShot(objectiveCompleteSound);
    }

    public void FailObjective(ObjectiveNode objective)
    {
        Debug.Log(objective.description);
        objective.objective.FailObjective();
        GetComponent<AudioSource>().PlayOneShot(objectiveFailedSound);
    }

    public void CompleteObjective(Quest quest, ObjectiveNode objective)
    {
        if (quest.objectives.Contains(objective))
        {
            objective.objective.CompleteObjectiveGoal();
            GetComponent<AudioSource>().PlayOneShot(objectiveCompleteSound);
            //Debug.Log("Completed Objective");
        }
        quest.completed = QuestComplete(quest);
        if (quest.completed)
            CompleteQuest(quest);
    }

    public void CompleteObjective(string quest, int objective)
    {
        foreach (Quest questEntry in activeQuests)
        {
            if (questEntry.title == quest)
            {
                CompleteObjective(questEntry, questEntry.objectives[objective]);
                questEntry.completed = QuestComplete(questEntry);
                if (questEntry.completed)
                    CompleteQuest(questEntry);
                break;
            }
        }
        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.title == quest)
            {
                CompleteObjective(questEntry, questEntry.objectives[objective]);
                questEntry.completed = QuestComplete(questEntry);
                if (questEntry.completed)
                    CompleteQuest(questEntry);
                break;
            }
        }
    }

    public void FailObjective(Quest quest, ObjectiveNode objective)
    {
        if (quest.objectives.Contains(objective))
        {
            objective.objective.FailObjective();
        }
        quest.failed = QuestFailed(quest);
        if (quest.failed)
            FailQuest(quest);
    }

    public void FailObjective(string quest, ObjectiveNode objective)
    {
        foreach (Quest questEntry in activeQuests)
        {
            if (questEntry.title == quest)
            {
                FailObjective(questEntry, objective);
                questEntry.failed = QuestFailed(questEntry);
                if (questEntry.failed)
                    FailQuest(questEntry);
                break;
            }
        }
        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.title == quest)
            {
                FailObjective(questEntry, objective);
                questEntry.failed = QuestFailed(questEntry);
                if (questEntry.failed)
                    FailQuest(questEntry);
                break;
            }
        }
    }

    public void CompleteQuest(Quest quest)
    {
        quest.CompleteQuest();
        completedQuests.Add(quest);
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
        }
        else
        {
            //journalPanel.AddQuest(quest);
        }
    }

    public void CompleteQuest(string quest)
    {
        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.title == quest)
            {
                questEntry.CompleteQuest();
                if (activeQuests.Contains(questEntry))
                {
                    activeQuests.Remove(questEntry);
                }
                else
                {
                    //journalPanel.AddQuest(questEntry);
                }
                completedQuests.Add(questEntry);
                break;
            }
        }

    }

    public void FailQuest(Quest quest)
    {
        if (allQuests.Contains(quest))
        {
            quest.FailQuest();
            failedQuests.Add(quest);
            if (!activeQuests.Contains(quest))
            {
                //journalPanel.AddQuest(quest);
            }
            else
            {
                activeQuests.Remove(quest);
            }
        }
    }

    public void FailQuest(string quest)
    {

        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.title == quest)
            {

                questEntry.FailQuest();
                failedQuests.Add(questEntry);
                if (!activeQuests.Contains(questEntry))
                {
                    //journalPanel.AddQuest(questEntry);
                }
                else
                {
                    activeQuests.Remove(questEntry);
                }
                break;
            }
        }
    }


    public bool QuestComplete(string quest)
    {
        foreach (Quest questEntry in completedQuests)
        {
            if (questEntry.title == quest)
            {
                Debug.Log("Checking for quest completion " + questEntry.title);
                return true;
            }
        }
        foreach (Quest questEntry in failedQuests)
        {
            if (questEntry.title == quest)
            {
                Debug.Log("Checking for quest completion " + questEntry.title);
                return true;
            }
        }
        Debug.Log("Checking for quest completion " + quest);
        return false;
    }

    public void AcceptQuest(Quest quest)
    {
        Debug.Log("Issuing Quest");
        if (!activeQuests.Contains(quest) && !completedQuests.Contains(quest) && !failedQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            remindersApp.AddReminder(quest);
            //journalPanel.AddQuest(quest);
            //foreach (ObjectiveNode node in quest.objectives)
            //{
            //    node.AssignObject();
            //}
        }
    }

    public void OpenJournal()
    {
        journalOpen = true;
        //GetComponent<FirstPersonController>().can_move = false;
        //GetComponent<FirstPersonController>().GetMouseLook().SetCursorLock(false);
        //if (activeQuests.Count > 0)
        //    journalPanel.SelectQuest(activeQuests[activeQuests.Count - 1]);
        //else if (completedQuests.Count > 0)
        //    journalPanel.SelectQuest(completedQuests[completedQuests.Count - 1]);
        //else if (failedQuests.Count > 0)
        //    journalPanel.SelectQuest(failedQuests[failedQuests.Count - 1]);
        //else
        //    journalPanel.ClearPanel();
        //foreach (Quest quest in activeQuests)
        //{
        //    journalPanel.UpdateQuest(quest);
        //}
        //foreach (Quest quest in allQuests)
        //{
        //    journalPanel.UpdateQuest(quest);
        //}
        //foreach (Quest quest in completedQuests)
        //{
        //    journalPanel.UpdateQuest(quest);
        //}
        //foreach (Quest quest in failedQuests)
        //{
        //    journalPanel.UpdateQuest(quest);
        //}
        //journalPanel.gameObject.SetActive(true);
    }

    public void CloseJournal()
    {
        journalOpen = false;
        //journalPanel.gameObject.SetActive(false);
        //GetComponent<FirstPersonController>().can_move = true;
        //GetComponent<FirstPersonController>().GetMouseLook().SetCursorLock(true);
    }

    public bool QuestComplete(Quest quest)
    {
        foreach (ObjectiveNode node in quest.objectives)
        {
            if (node.objective.ObjectiveFailed() && !node.objective.objectiveOptional)
            {
                Debug.Log(node.description);
                return false;
            }
            else if (!node.objective.ObjectiveComplete() && !node.objective.ObjectiveFailed())
            {
                Debug.Log(node.description);
                return false;
            }
            if (!node.objective.objectiveComplete)
            {
                //node.objective.CompleteObjectiveGoal();
            }
        }
        return true;
    }

    public bool ObjectiveComplete(string quest, int objective)
    {
        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.objectives.Count > objective && objective != -1 && questEntry.title == quest && questEntry.objectives[objective].objective.objectiveComplete)
            {

                Debug.Log(questEntry.title + " " + questEntry.objectives[objective].description);
                return true;
            }
            else if (questEntry.objectives.Count > objective && objective != -1 && questEntry.title == quest && questEntry.objectives[objective].objective.questItemName != null)
            {
                Debug.Log(questEntry.title + " " + questEntry.objectives[objective].description);
                return GetComponent<Inventory>().FindItem(questEntry.objectives[objective].objective.questItem) != null;
            }
        }
        return false;
    }

    public bool ObjectiveFailed(string quest, int objective)
    {
        foreach (Quest questEntry in allQuests)
        {
            if (questEntry.objectives.Count > (objective + 1) && questEntry.title == quest)
            {
                Debug.Log(objective);
                if (questEntry.objectives[objective].objective.objectiveFailed)
                {
                    Debug.Log(questEntry.title);
                    return true;
                }
            }
        }
        foreach (Quest questEntry in failedQuests)
        {
            if (questEntry.objectives.Count > objective && questEntry.objectives[objective].objective.objectiveFailed)
            {
                Debug.Log(questEntry.title);
                return true;
            }
        }
        return false;
    }

    public bool QuestFailed(Quest quest)
    {
        foreach (ObjectiveNode node in quest.objectives)
        {
            if (node.objective.ObjectiveFailed() && !node.objective.objectiveOptional)
            {
                if (!node.objective.objectiveFailed)
                {
                    node.objective.FailObjective();
                }
                return true;
            }
            else if (!node.objective.ObjectiveComplete() && !node.objective.ObjectiveFailed())
            {
                if (!node.objective.objectiveFailed)
                {
                    node.objective.FailObjective();
                }
                return true;
            }
        }
        return false;
    }
}
