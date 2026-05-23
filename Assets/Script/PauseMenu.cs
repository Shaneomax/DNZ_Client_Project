using UnityEngine;
using UnityEngine.SceneManagement; // Required for restarting and changing levels

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Main Menu"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); 
    }
}