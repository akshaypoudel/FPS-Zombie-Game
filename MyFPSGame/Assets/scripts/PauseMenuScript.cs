using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public static bool isGamePaused=false;

    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Cursor.lockState=CursorLockMode.Locked;
        Time.timeScale = 1f;
        isGamePaused=false;
    }
    void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused=true;
        Cursor.lockState=CursorLockMode.None;
        PauseMenuUI.SetActive(true);
    }
    public void NewGame(string name)
    {
        Time.timeScale = 1f;
        Cursor.lockState=CursorLockMode.Locked;
        SceneManager.LoadScene(name);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting GAme: ");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    } 
    public void HomeButton(int buildINdex)
    {
        Cursor.lockState=CursorLockMode.None;
        SceneManager.LoadScene(buildINdex);
    }
}
