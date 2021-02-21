using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerInteraction : MonoBehaviour
{
    public List<Note> textFiles;
    public List<Lock> locks;
    public GameObject doorControlPrefab;
    public GameObject doorControlIconPrefab;
    public Computer currentComputer;
    public GameObject computerUI;
    public Note activeTextFile;
    public GameObject mainUIScreen;
    public GameObject loginScreen;
    public GameObject doorControlsScreen;
    public Transform doorsList;
    public GameObject textFileIconPrefab;
    public GameObject textFileReader;
    public TMP_InputField passwordField;
    public Button passwordButton;
    public TMP_Text textFileHeader;
    public TMP_Text textFileContent;
    FirstPersonController controller;
    public NotesApp notesApp;
    public bool locked = false;
    AudioSource source;
    public AudioClip unlockDoorSound;
    // Start is called before the first frame update
    void Start()
    {
        computerUI.SetActive(false);
        loginScreen.SetActive(false);
        textFileReader.SetActive(false);
        doorControlsScreen.SetActive(false);
        mainUIScreen.SetActive(false);
        controller = GetComponent<FirstPersonController>();
        source = controller.GetComponent<AudioSource>();
        passwordButton.onClick.AddListener(
            () =>
            {
                EnterPassword(passwordField.text);
            }
        );
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void ShowLoginScreen()
    {
        loginScreen.SetActive(true);

        //mainUIScreen.SetActive(false);
    }

    public void EnterPassword(string value)
    {
        if (value.Equals(currentComputer.password))
        {
            Login();
        }
        else
        {
            passwordField.text = "";
        }
    }

    public void Login()
    {
        currentComputer.locked = false;
        PopulateTextFiles();
        if (locks.Count > 0)
        {
            GameObject doorControlIcon = Instantiate(doorControlIconPrefab, mainUIScreen.transform);
            doorControlIcon.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    DisplayDoorControls();
                }
            );
        }
        loginScreen.SetActive(false);
        mainUIScreen.SetActive(true);
    }

    public void Logout()
    {
        loginScreen.SetActive(false);
        mainUIScreen.SetActive(false);
    }

    public void PopulateLockControls()
    {
        ClearAll(doorsList);
        foreach (Lock lockedItem in locks)
        {
            GameObject doorItem = Instantiate(doorControlPrefab, doorsList);
            doorItem.transform.Find("DoorLockStatusText").GetComponent<TMP_Text>().text = lockedItem.locked ?  "Locked" : "Unlocked";
            doorItem.GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = lockedItem.locked ? "Unlock" : "Lock";
            doorItem.GetComponentInChildren<Button>().onClick.AddListener(
                () =>
                {
                    lockedItem.Unlock();
                    source.PlayOneShot(unlockDoorSound);
                    doorItem.GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Lock";
                    doorItem.transform.Find("DoorLockStatusText").GetComponent<TMP_Text>().text = "Unlocked";
                }
            );
        }
    }

    public void HideDoorControls()
    {
        doorControlsScreen.SetActive(false);
    }

    public void DisplayDoorControls()
    {
        doorControlsScreen.SetActive(true);
        PopulateLockControls();
    }

    // Create icons for all text files
    public void PopulateTextFiles()
    {
        ClearAll(mainUIScreen.transform);
        foreach (Note file in textFiles)
        {
            GameObject newFile = Instantiate(textFileIconPrefab, mainUIScreen.transform);
            newFile.GetComponentInChildren<TMP_Text>().text = file.noteName;
            newFile.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    DisplayTextFile(file);
                }
            );
        }
    }

    public void CloseTextFileWindow()
    {
        textFileReader.SetActive(false);
    }

    public void DisplayTextFile(Note fileToDisplay)
    {
        activeTextFile = fileToDisplay;
        textFileContent.text = activeTextFile.noteContent;
        textFileHeader.text = activeTextFile.noteName;
        textFileReader.SetActive(true);
        // Add text file to players notes app
        notesApp.AddNote(activeTextFile);
    }

    public void DisplayComputerUI(Computer computerToDisplay)
    {
        PlayerInteraction.LockInteraction();
        DynamicCursor.ChangeCursor_Static(CursorType.None);
        controller.m_CanMove = false;
        controller.m_CanLook = false;
        controller.GetMouseLook().SetCursorLock(false);
        currentComputer = computerToDisplay;
        textFiles = currentComputer.textFiles;
        locks = currentComputer.locks;
        computerUI.SetActive(true);
        if (currentComputer.locked)
        {
            ShowLoginScreen();
        }
        else
        {
            PopulateTextFiles();
            if (locks.Count > 0)
            {
                GameObject doorControlIcon = Instantiate(doorControlIconPrefab, mainUIScreen.transform);
                doorControlIcon.GetComponent<Button>().onClick.AddListener(
                    () =>
                    {
                        DisplayDoorControls();
                    }
                );
            }
        }
    }

    public void HideComputerUI()
    {
        computerUI.SetActive(false);
        controller.m_CanMove = true;
        controller.m_CanLook = true;
        controller.GetMouseLook().SetCursorLock(true);
        PlayerInteraction.UnlockInteraction();
    }

    public void ClearAll(Transform parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
