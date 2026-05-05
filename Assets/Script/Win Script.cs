using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Speed Settings")]
    [Tooltip("How much to slow the ball down. 0 is a dead stop, 0.1 is 10% speed.")]
    [Range(0f, 1f)]
    [SerializeField] private float slowDownFactor = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = rb.linearVelocity * slowDownFactor;
                rb.angularVelocity = rb.angularVelocity * slowDownFactor;
            }

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

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}