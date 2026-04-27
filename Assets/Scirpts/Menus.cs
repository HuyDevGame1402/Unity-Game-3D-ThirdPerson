using UnityEngine;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    public GameObject objectiveMenuUI;

    public static bool gameIsPaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (gameIsPaused)
            {
                RemoveObjective();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                ShowObjectives();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ShowObjectives()
    {
        objectiveMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RemoveObjective()
    {
        objectiveMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gameIsPaused = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gameIsPaused = false;
    }

    public void ReStart()
    {

    }

    public void LoadMenu()
    {
        // load menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }
}
