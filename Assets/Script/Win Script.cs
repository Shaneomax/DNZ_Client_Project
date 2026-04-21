using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public TextMeshProUGUI winText;

    [Header("Speed Settings")]
    [Tooltip("How much to slow the ball down. 0 is a dead stop, 0.1 is 10% speed.")]
    [Range(0f, 1f)]
    public float slowDownFactor = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object the player collided with has the "Pickup" tag.
        if (other.gameObject.CompareTag("Pickup"))
        {
            // 1. Slow down the ball
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Multiply current speed and rotation by the slow down factor
                rb.linearVelocity = rb.linearVelocity * slowDownFactor;
                rb.angularVelocity = rb.angularVelocity * slowDownFactor;
            }

            // 2. Display Win Text
            if (winText != null)
            {
                winText.text = "Congratulations You Win";
            }

            // 3. Start the timer to load the next level
            StartCoroutine(LoadLevelAfterDelay());
        }
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        // Get the index number of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene actually exists in our build settings
        // (This prevents an error from crashing the game when you finish Level 5)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}