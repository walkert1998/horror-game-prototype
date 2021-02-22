using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesApp : MonoBehaviour
{
    public GameObject notePanel;
    public GameObject noteListItemPrefab;
    public Transform notesList;
    public List<Note> discoveredNotes;
    public TMP_Text noteContent;
    public TMP_Text noteName;
    public bool readingNote;
    //PauseMenu pauseMenu;
    //AudioSource source;
    public AudioClip openNoteSound;
    public AudioClip markLocationSound;
    public Note activeNote;
    public GameObject notificationIcon;
    public TMP_Text notificationCount;
    public GameObject notificationDisplay;
    public TMP_Text notificationText;
    public int unopenedItems = 0;
    //public FirstPersonController firstPersonController;
    // Start is called before the first frame update
    void Start()
    {
        //pauseMenu = GetComponent<PauseMenu>();
        notificationDisplay.SetActive(false);
        notificationText.text = "";
        PopulateNotes();
        //notePanel.SetActive(false);
        //source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public void PopulateNotes()
    {
        foreach (Transform note in notesList.transform)
        {
            Destroy(note.gameObject);
        }
        foreach (Note note in discoveredNotes)
        {
            GameObject newNote = Instantiate(noteListItemPrefab, notesList);
            newNote.GetComponentInChildren<TMP_Text>().text = note.name;
        }
    }

    public void AddNote(Note newNote)
    {
        if (!discoveredNotes.Contains(newNote))
        {
            discoveredNotes.Add(newNote);
            unopenedItems++;
            notificationDisplay.SetActive(true);
            notificationText.text = unopenedItems.ToString();
            if (gameObject.activeSelf)
            {
                Notification.DisplayNewNotification("1 new Note!", AppNotificationType.Notes);
            }
            //notificationCount.text = unopenedNotes.ToString();
            GameObject note = Instantiate(noteListItemPrefab, notesList);
            note.GetComponentInChildren<TMP_Text>().text = newNote.noteName;
            note.GetComponent<Button>().onClick.AddListener(
                ()=>
                {
                    ReadNote(newNote);
                }
            );
        }
    }

    public void ReadNote(Note note)
    {
        activeNote = note;
        //source.PlayOneShot(openNoteSound);
        //pauseMenu.Pause();
        //notePanel.SetActive(true);
        //firstPersonController.m_CanMove = false;
        //firstPersonController.GetMouseLook().SetCursorLock(false);
        //readingNote = true;
        noteContent.text = note.noteContent;
        noteName.text = note.noteName;
    }

    public void CloseNote()
    {
        noteContent.text = "";
        //pauseMenu.ResumeGame();
        /*
        if (activeNote.gameObject.GetComponent<MapLocation>() && !activeNote.gameObject.GetComponent<MapLocation>().marked)
        {
            activeNote.gameObject.GetComponent<MapLocation>().MarkLocation();
            source.PlayOneShot(markLocationSound);
        }
        */
        activeNote = null;
        //notePanel.SetActive(false);
        //firstPersonController.GetMouseLook().SetCursorLock(true);
        //firstPersonController.m_CanMove = true;
        noteName.text = "";
        //readingNote = false;
    }

    public void ClearUnopenedItems()
    {
        unopenedItems = 0;
        notificationDisplay.SetActive(false);
        notificationText.text = "";
    }
}
