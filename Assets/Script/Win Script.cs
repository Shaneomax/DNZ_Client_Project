using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For regular Unity UI components (like Buttons, Images)
using TMPro;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{

    public TextMeshProUGUI winText;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Pickup"))
        {
            // Deactivate the collided object (making it disappear).
            //other.gameObject.SetActive(false);

            if (winText != null)
            {
                winText.text = "Congratulations You Win";
            }

            StartCoroutine(LoadLevelAfterDelay());
        }
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        // 1. Get the index number of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 2. Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // 3. Check if the next scene actually exists in our build settings
        // (This prevents an error from crashing the game when you finish Level 5)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }
}
