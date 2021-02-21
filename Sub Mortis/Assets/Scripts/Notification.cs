using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public GameObject notificationObject;
    public TMP_Text notificationAppName;
    public TMP_Text notificationContent;
    public RawImage notificationIcon;
    public float notificationLength = 4.0f;
    public static Notification instance;
    public Texture messagesIcon;
    public Texture remindersIcon;
    public Texture notesIcon;
    public Texture picturesIcon;
    public AudioClip notificationSound;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //notificationObject.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void NewNotification(string notification, AppNotificationType notificationType)
    {
        switch (notificationType)
        {
            case AppNotificationType.Messages:
                notificationAppName.text = "Messages";
                notificationContent.text = notification;
                notificationIcon.texture = messagesIcon;
                break;
            case AppNotificationType.Notes:
                notificationAppName.text = "Notes";
                notificationContent.text = notification;
                notificationIcon.texture = notesIcon;
                break;
            case AppNotificationType.Pictures:
                notificationAppName.text = "Pictures";
                notificationContent.text = notification;
                notificationIcon.texture = picturesIcon;
                break;
            case AppNotificationType.Reminders:
                notificationAppName.text = "Reminders";
                notificationContent.text = notification;
                notificationIcon.texture = remindersIcon;
                break;
        }
        StartCoroutine(DisplayNotification());
    }

    IEnumerator DisplayNotification()
    {
        notificationObject.SetActive(true);
        //if (GetComponentInParent<Phone>().focusState.Equals(PhoneFocusState.HorizontalFocused))
        //{
        //    GetComponent<PhoneUIOrientation>().SetToHorizontal();
        //}
        //else
        //{
        //    GetComponent<PhoneUIOrientation>().SetToVertical();
        //}
        source.PlayOneShot(notificationSound);
        yield return new WaitForSeconds(notificationLength);
        notificationObject.SetActive(false);
    }

    public static void DisplayNewNotification(string notification, AppNotificationType notificationType)
    {
        instance.NewNotification(notification, notificationType);
    }

    public static void HideNotification()
    {
        instance.notificationObject.SetActive(false);
    }
}

public enum AppNotificationType
{
    Messages,
    Notes,
    Pictures,
    Reminders
}
