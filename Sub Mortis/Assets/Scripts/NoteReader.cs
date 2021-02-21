using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class NoteReader : MonoBehaviour
{
    public GameObject notePanel;
    public TMP_Text noteContent;
    public TMP_Text pageCount;
    public TMP_Text noteName;
    public bool readingNote;
    //PauseMenu pauseMenu;
    AudioSource source;
    public AudioClip openNoteSound;
    public AudioClip markLocationSound;
    public Note activeNote;
    public FirstPersonController firstPersonController;
    public NotesApp app;
    public int currentPage = 1;
    // Start is called before the first frame update
    void Start()
    {
        //pauseMenu = GetComponent<PauseMenu>();
        notePanel.SetActive(false);
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void ReadNote(Note note)
    {
        currentPage = 1;
        activeNote = note;
        source.PlayOneShot(openNoteSound);
        //pauseMenu.Pause();
        notePanel.SetActive(true);
        firstPersonController.m_CanMove = false;
        firstPersonController.m_CanLook = false;
        firstPersonController.GetMouseLook().SetCursorLock(false);
        DynamicCursor.ChangeCursor_Static(CursorType.None);
        PlayerInteraction.LockInteraction();
        readingNote = true;
        noteContent.text = note.noteContent;
        noteContent.ForceMeshUpdate();
        pageCount.text = currentPage + "/" + noteContent.textInfo.pageCount;
        //noteName.text = note.noteName;
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
        app.AddNote(activeNote);
        activeNote = null;
        notePanel.SetActive(false);
        firstPersonController.GetMouseLook().SetCursorLock(true);
        firstPersonController.m_CanMove = true;
        firstPersonController.m_CanLook = true;
        PlayerInteraction.UnlockInteraction();
        //noteName.text = "";
        currentPage = 1;
        readingNote = false;
    }

    public void NextPage()
    {
        if (currentPage < noteContent.textInfo.pageCount)
        {
            currentPage++;
            noteContent.pageToDisplay = currentPage;
            pageCount.text = currentPage + "/" + noteContent.textInfo.pageCount;
            //noteContent.text = noteContent.textInfo.pageInfo.GetValue(currentPage).ToString();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            noteContent.pageToDisplay = currentPage;
            pageCount.text = currentPage + "/" + noteContent.textInfo.pageCount;
            //noteContent.text = noteContent.textInfo.pageInfo.GetValue(currentPage).ToString();
        }
    }
}
