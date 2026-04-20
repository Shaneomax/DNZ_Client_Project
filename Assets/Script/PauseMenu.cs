using UnityEngine;
using UnityEngine.SceneManagement; // Required for restarting and changing levels

public class PauseMenu : MonoBehaviour
{
    // Drag your Pause Menu UI panel into this slot in the Inspector
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freezes the game time
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Unfreezes the game time
    }

    public void RestartLevel()
    {
        // CRITICAL: Always reset time to 1 before loading a scene, 
        // otherwise the new scene will load completely frozen!
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Main Menu"); // Type your exact main menu scene name here
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Note: This will close a built Android game, but won't stop the Unity Editor playback
        Application.Quit(); 
    }
}