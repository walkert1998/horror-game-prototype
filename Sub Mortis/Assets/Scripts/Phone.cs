using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Phone : MonoBehaviour
{
    public Vector3 horizontalFocusedPosition;
    public Quaternion horizontalFocusedRotation;
    public Vector3 verticalFocusedPosition;
    public Quaternion verticalFocusedRotation;
    public Vector3 unfocusedPosition;
    public Quaternion unfocusedRotation;
    public bool focused = false;
    public bool horizontal = false;
    public PhoneFocusState focusState;
    public PhoneFocusState lastFocusState;
    public List<RectTransform> appIcons;
    public GameObject widgetScreen;
    public GameObject remindersApp;
    public GameObject notesApp;
    public GameObject picturesApp;
    public GameObject homeScreen;
    public GameObject inventoryScreen;
    public GameObject cameraScreen;
    public GameObject messagesApp;
    public GameObject currentScreen;
    public GameObject lastOpenedApp;
    public PhoneCamera phoneCamera;
    public PicturesApp picturesApplication;
    public NotesApp notesApplication;
    public RemindersScreen remindersApplication;
    public MessagesApp messagesApplication;
    public PhoneUIOrientation[] uiElements;
    public FirstPersonController firstPersonController;
    // Start is called before the first frame update
    void Start()
    {
        //uiElements = GetComponentsInChildren<PhoneUIOrientation>();
        homeScreen.SetActive(false);
        inventoryScreen.SetActive(false);
        cameraScreen.SetActive(false);
        homeScreen.SetActive(false);
        picturesApp.SetActive(false);
        notesApp.SetActive(false);
        remindersApp.SetActive(false);
        messagesApp.SetActive(false);
        //homeScreen.SetActive(false);
        currentScreen = widgetScreen;
        UnFocus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (focusState.Equals(PhoneFocusState.HorizontalFocused) || focusState.Equals(PhoneFocusState.VerticalFocused))
            {
                UnFocus();
                Tooltip.HideToolTip_Static();
                //firstPersonController.GetMouseLook().SetCursorLock(true);
                //firstPersonController.m_CanMove = true;
                //firstPersonController.m_CanLook = true;
            }
            else if (!PlayerInteraction.interactionBlocked)
            {
                Debug.Log(lastFocusState);
                DynamicCursor.ChangeCursor_Static(CursorType.None);
                firstPersonController.GetMouseLook().SetCursorLock(false);
                Tooltip.HideToolTip_Static();
                firstPersonController.m_CanMove = true;
                firstPersonController.m_CanLook = false;
                if (lastFocusState.Equals(PhoneFocusState.HorizontalFocused))
                {
                    FocusHorizontal();
                }
                else if (lastFocusState.Equals(PhoneFocusState.VerticalFocused))
                {
                    FocusVertical();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (phoneCamera.cameraActive)
            {
                DisplayHomeScreen();
            }
            //if (!horizontal && !focusState.Equals(PhoneFocusState.VerticalFocused))
            //{
            //    FocusVertical();
            //}
            //else if (!horizontal && focusState.Equals(PhoneFocusState.VerticalFocused))
            //{
            //    UnFocus();
            //}
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    if (focusState.Equals(PhoneFocusState.VerticalFocused))
        //    {
        //        FocusHorizontal();
        //    }
        //    else if (focusState.Equals(PhoneFocusState.HorizontalFocused))
        //    {
        //        FocusVertical();
        //    }
        //    //else
        //    //{
        //    //    UnFocus();
        //    //}
        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!focusState.Equals(PhoneFocusState.UnFocused))
            {
                DisplayHomeScreen();
            }
            //else
            //{
            //    UnFocus();
            //}
        }
    }

    public void FocusHorizontal()
    {
        focused = true;
        horizontal = true;
        if (lastOpenedApp != null)
        {
            if (lastOpenedApp == cameraScreen)
            {
                DisplayCameraApp();
            }
            else
            {
                currentScreen.SetActive(false);
                currentScreen = lastOpenedApp;
                currentScreen.SetActive(true);
                PlayerInteraction.LockInteraction();
            }
        }
        else
        {
            currentScreen.SetActive(false);
            currentScreen = widgetScreen;
            currentScreen.SetActive(true);
            PlayerInteraction.LockInteraction();
        }
        focusState = PhoneFocusState.HorizontalFocused;
        lastFocusState = focusState;
        transform.localPosition = horizontalFocusedPosition;
        transform.localRotation = horizontalFocusedRotation;
        foreach (PhoneUIOrientation element in uiElements)
        {
            element.SetToHorizontal();
        }
        foreach (RectTransform rect in appIcons)
        {
            rect.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void FocusVertical()
    {
        focused = true;
        horizontal = false;
        focusState = PhoneFocusState.VerticalFocused;
        lastFocusState = focusState;
        transform.localPosition = verticalFocusedPosition;
        transform.localRotation = verticalFocusedRotation;
        foreach (PhoneUIOrientation element in uiElements)
        {
            element.SetToVertical();
        }
        foreach (RectTransform rect in appIcons)
        {
            rect.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public void UnFocus()
    {
        focused = false;
        horizontal = false;
        focusState = PhoneFocusState.UnFocused;
        DisplayWidget();
        transform.localPosition = unfocusedPosition;
        transform.localRotation = unfocusedRotation;
        currentScreen.SetActive(false);
        currentScreen = widgetScreen;
        currentScreen.SetActive(true);
        firstPersonController.GetMouseLook().SetCursorLock(true);
        Tooltip.HideToolTip_Static();
        firstPersonController.m_CanMove = true;
        firstPersonController.m_CanLook = true;
        phoneCamera.DeactivateCamera();
        foreach (PhoneUIOrientation element in uiElements)
        {
            element.SetToVertical();
        }
        foreach (RectTransform rect in appIcons)
        {
            rect.localRotation = Quaternion.Euler(0, 0, 90);
        }
        Notification.HideNotification();
        PlayerInteraction.UnlockInteraction();
    }

    public void DisplayWidget()
    {
        currentScreen.SetActive(false);
        currentScreen = widgetScreen;
        currentScreen.SetActive(true);
        //inventoryScreen.SetActive(false);
        //homeScreen.SetActive(false);
    }

    public void HideWidget()
    {
        //widgetScreen.SetActive(false);
        currentScreen.SetActive(false);
        currentScreen = homeScreen;
        currentScreen.SetActive(true);
    }

    public void DisplayHomeScreen()
    {
        phoneCamera.DeactivateCamera();
        firstPersonController.GetMouseLook().SetCursorLock(false);
        Tooltip.HideToolTip_Static();
        firstPersonController.m_CanLook = false;
        currentScreen.SetActive(false);
        currentScreen = homeScreen;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        //homeScreen.SetActive(true);
        //widgetScreen.SetActive(false);
        //inventoryScreen.SetActive(false);
        //cameraScreen.SetActive(false);
    }

    public void DisplayCameraApp()
    {
        currentScreen.SetActive(false);
        firstPersonController.GetMouseLook().SetCursorLock(true);
        Tooltip.HideToolTip_Static();
        firstPersonController.m_CanMove = true;
        firstPersonController.m_CanLook = true;
        currentScreen = cameraScreen;
        lastOpenedApp = currentScreen;
        phoneCamera.ActivateCamera();
        currentScreen.SetActive(true);
        //cameraScreen.SetActive(true);
        //homeScreen.SetActive(false);
        //widgetScreen.SetActive(false);
        //inventoryScreen.SetActive(false);
    }

    public void DisplayInventory()
    {
        currentScreen.SetActive(false);
        currentScreen = inventoryScreen;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        //cameraScreen.SetActive(true);
        //homeScreen.SetActive(false);
        //widgetScreen.SetActive(false);
        //inventoryScreen.SetActive(false);
    }

    public void DisplayNotes()
    {
        currentScreen.SetActive(false);
        currentScreen = notesApp;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        notesApplication.ClearUnopenedItems();
        //cameraScreen.SetActive(true);
        //homeScreen.SetActive(false);
        //widgetScreen.SetActive(false);
        //inventoryScreen.SetActive(false);
    }

    public void DisplayReminders()
    {
        currentScreen.SetActive(false);
        currentScreen = remindersApp;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        remindersApplication.ClearUnopenedItems();
        //cameraScreen.SetActive(true);
        //homeScreen.SetActive(false);
        //widgetScreen.SetActive(false);
        //inventoryScreen.SetActive(false);
    }

    public void DisplayPictures()
    {
        currentScreen.SetActive(false);
        currentScreen = picturesApp;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        picturesApplication.ClearUnopenedItems();
    }

    public void DisplayMessages()
    {
        currentScreen.SetActive(false);
        currentScreen = messagesApp;
        lastOpenedApp = currentScreen;
        currentScreen.SetActive(true);
        messagesApplication.ClearUnopenedItems();
    }
}

public enum PhoneFocusState
{
    HorizontalFocused,
    VerticalFocused,
    UnFocused
}
