using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public bool gamePaused = false;
    public FirstPersonController controller;
    //BackPack backPack;
    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(false);
        }
        //backPack = GetComponent<BackPack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuUI != null)
        {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        gamePaused = true;
        if (!InventoryManager.IsInventoryOpen_Static())
        {
            Cursor.lockState = CursorLockMode.None;
            controller.m_CanMove = false;
            controller.m_CanLook = false;
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        PlayerInteraction.LockInteraction();
        Time.timeScale = 0.0f;
        if (!InventoryManager.IsInventoryOpen_Static())
        {
            controller.GetMouseLook().SetCursorLock(false);
            DynamicCursor.ChangeCursor_Static(CursorType.None);
            DynamicCursor.HideCursor_Static();
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            controller.m_CanMove = false;
            controller.m_CanLook = false;
        }
        gamePaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        if (!InventoryManager.IsInventoryOpen_Static() && !NoteReader.ReadingNote() && !ComputerInteraction.usingComputer)
        {
            controller.GetMouseLook().SetCursorLock(true);
            DynamicCursor.ChangeCursor_Static(CursorType.Target);
            DynamicCursor.ShowCursor_Static();
            //Cursor.visible = false;
            controller.m_CanMove = true;
            controller.m_CanLook = true;
            PlayerInteraction.UnlockInteraction();
            Cursor.lockState = CursorLockMode.Locked;
        }
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    public void OpenControls()
    {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void CloseControls()
    {
        controlsMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
