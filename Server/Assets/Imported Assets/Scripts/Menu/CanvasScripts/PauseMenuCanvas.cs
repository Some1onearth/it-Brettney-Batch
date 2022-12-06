using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCanvas : MonoBehaviour
{
    public static bool isPaused;
    public GameObject pauseMenu, optionsMenu;
    void Paused()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnPaused()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        UnPaused();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                pauseMenu.SetActive(true);
                Paused();
                isPaused = true;
            }
            else
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    Paused();
                }
                else
                {
                    UnPaused();
                }
            }
        }
    }
}
