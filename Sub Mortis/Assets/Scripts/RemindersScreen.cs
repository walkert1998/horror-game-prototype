using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemindersScreen : MonoBehaviour
{
    public TMP_Text reminderDetailHeader;
    public TMP_Text toggleCompletedText;
    public TMP_Text reminderWidget;
    public Button toggleCompletedButton;
    public bool showCompleted = false;
    public List<GameObject> reminders;
    public List<GameObject> completedReminders;
    public List<GameObject> subtasks;
    public GameObject reminderPrefab;
    public GameObject subtaskPrefab;
    public Transform subTaskScroll;
    public Transform reminderScroll;
    public QuestManager questManager;
    public GameObject notificationDisplay;
    public TMP_Text notificationText;
    public int unopenedItems = 0;
    // Start is called before the first frame update
    void Start()
    {
        PopulateReminders();
        toggleCompletedButton.onClick.AddListener(
            () =>
            {
                ToggleCompleted();
            }
         );
        notificationDisplay.SetActive(false);
        notificationText.text = "";
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void AddReminder(Quest newQuest)
    {
        //Notification.DisplayNewNotification(newQuest.title, AppNotificationType.Reminders);
        unopenedItems++;
        notificationDisplay.SetActive(true);
        notificationText.text = unopenedItems.ToString();
        GameObject reminder = Instantiate(reminderPrefab, reminderScroll);
        reminder.GetComponentInChildren<TMP_Text>().text = newQuest.title;
        reminder.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DisplayReminder(newQuest);
            }
        );
        reminderWidget.text = newQuest.title;
    }

    public void ToggleCompleted()
    {
        if (showCompleted)
        {
            HideCompleted();
        }
        else
        {
            ShowCompleted();
        }
    }

    public void ShowCompleted()
    {
        showCompleted = true;
        toggleCompletedText.text = "Hide Completed";
        foreach (GameObject compR in completedReminders)
        {
            compR.SetActive(true);
        }
    }

    public void HideCompleted()
    {
        showCompleted = false;
        toggleCompletedText.text = "Show Completed";
        foreach (GameObject compR in completedReminders)
        {
            compR.SetActive(false);
        }
    }

    public void PopulateReminders()
    {
        foreach (Transform reminder in reminderScroll.transform)
        {
            Destroy(reminder.gameObject);
        }
        foreach (Quest quest in questManager.activeQuests)
        {
            AddReminder(quest);
        }
    }

    public void DisplayReminder(Quest reminder)
    {
        reminderDetailHeader.text = reminder.title;
        foreach (Transform subtask in subTaskScroll.transform)
        {
            Destroy(subtask.gameObject);
        }
        foreach (ObjectiveNode subtask in reminder.objectives)
        {
            GameObject displayedReminder = Instantiate(subtaskPrefab, subTaskScroll);
            displayedReminder.GetComponentInChildren<Text>().text = subtask.description;
        }
    }

    public void ClearUnopenedItems()
    {
        unopenedItems = 0;
        notificationDisplay.SetActive(false);
        notificationText.text = "";
    }
}
