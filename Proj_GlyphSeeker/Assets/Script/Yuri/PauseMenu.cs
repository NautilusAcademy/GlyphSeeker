using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI, playerHUD, optionHUD, quitHUD, resumeButton, optionButton, quitButton;

    [SerializeField] List<MonoBehaviour> scriptToBlock;



    void Start()
    {
        gameIsPaused = false;
        Resume();
    }

    void Update()
    {

        if (GameManager.inst.inputManager.Player.Pause.WasPressedThisFrame())
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        optionHUD.SetActive(false);
        quitHUD.SetActive(false);
        playerHUD.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        EnableAllScripts(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameIsPaused = true;
        SelectResume();
        EnableAllScripts(false);
    }

    public void EnableAllScripts(bool enabled)
    {
        foreach (MonoBehaviour scr in scriptToBlock)
        {
            scr.enabled = enabled;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SelectOption()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionButton);

    }

    public void SelectResume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }


    public void SelectQuit()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitButton);
    }
}