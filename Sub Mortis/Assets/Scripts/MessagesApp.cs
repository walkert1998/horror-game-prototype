using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagesApp : MonoBehaviour
{
    public TMP_Text conversationContactHeader;
    public TMP_Text messageWidgetText;
    public List<TextConversation> conversations;
    public List<TextMessage> messages;
    public GameObject conversationPrefab;
    public GameObject messagePrefab;
    public GameObject playerMessagePrefab;
    public Transform messagesScroll;
    public Transform conversationScroll;
    public GameObject notificationDisplay;
    public TMP_Text notificationText;
    public int unopenedItems = 0;
    // Start is called before the first frame update
    void Start()
    {
        notificationDisplay.SetActive(false);
        notificationText.text = "";
        PopulateMessages();
    }

    public void AddNewConversation(TextConversation newConversation)
    {
        if (!conversations.Contains(newConversation))
        {
            unopenedItems++;
            notificationDisplay.SetActive(true);
            notificationText.text = unopenedItems.ToString();
            conversations.Add(newConversation);
            //Notification.DisplayNewNotification(newConversation.messages[0].messageContent, AppNotificationType.Messages);
        }
        unopenedItems++;
        notificationDisplay.SetActive(true);
        notificationText.text = unopenedItems.ToString();
        GameObject reminder = Instantiate(conversationPrefab, conversationScroll);
        reminder.GetComponentInChildren<TMP_Text>().text = newConversation.contactName;
        reminder.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DisplayConversation(newConversation);
            }
        );
        messageWidgetText.text = newConversation.messages[0].messageContent;
    }

    public void AddNewMessage(TextMessage newMessage)
    {
        if (!messages.Contains(newMessage))
        {
            messages.Add(newMessage);
            messageWidgetText.text = newMessage.messageContent;
            //Notification.DisplayNewNotification(newMessage.messageContent, AppNotificationType.Messages);
        }
        GameObject message;
        if (newMessage.fromPlayer)
        {
            message = Instantiate(playerMessagePrefab, messagesScroll);
        }
        else
        {
            message = Instantiate(messagePrefab, messagesScroll);
        }
        message.GetComponentInChildren<TMP_Text>().text = newMessage.messageContent;
    }

    public void PopulateMessages()
    {
        foreach (Transform reminder in conversationScroll.transform)
        {
            Destroy(reminder.gameObject);
        }
        foreach (TextConversation convo in conversations)
        {
            AddNewConversation(convo);
        }
    }

    public void DisplayConversation(TextConversation conversation)
    {
        conversationContactHeader.text = conversation.contactName;
        foreach (Transform tform in messagesScroll.transform)
        {
            Destroy(tform.gameObject);
        }
        foreach (TextMessage message in conversation.messages)
        {
            //GameObject displayedReminder;
            //if (message.fromPlayer)
            //{
            //    displayedReminder = Instantiate(playerMessagePrefab, messagesScroll);
            //}
            //else
            //{
            //    displayedReminder = Instantiate(messagePrefab, messagesScroll);
            //}
            //displayedReminder.GetComponentInChildren<TMP_Text>().text = message.messageContent;
            AddNewMessage(message);
        }
    }

    public void ClearUnopenedItems()
    {
        unopenedItems = 0;
        notificationDisplay.SetActive(false);
        notificationText.text = "";
    }
}
