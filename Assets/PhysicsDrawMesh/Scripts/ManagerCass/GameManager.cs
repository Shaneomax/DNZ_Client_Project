using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Level State")]
    [SerializeField]private WaterCollector[] allBuckets;
    private int filledBucketsCount = 0;
    private bool levelComplete = false;

    void Start()
    {
        if (winText != null) winText.text = "";

        // REMOVE THIS LINE:
        // allBuckets = FindObjectsByType<WaterCollector>(FindObjectsSortMode.None);

        // Instead, just verify the list isn't empty
        if (allBuckets == null || allBuckets.Length == 0)
        {
            Debug.LogError("No buckets assigned to the Game Manager!");
        }
        else
        {
            Debug.Log($"Game Manager: Tracking {allBuckets.Length} specific bucket(s).");
        }
    }

    // This is the function each bucket will call
    public void CheckWinCondition()
    {
        if (levelComplete) return;

        filledBucketsCount = 0;

        // Loop through all buckets to see if they are ALL filled
        foreach (WaterCollector bucket in allBuckets)
        {
            // We check the internal 'isFilled' state of the bucket
            // Note: Since 'isFilled' was private in your script, 
            // you need to make the change in Step 2 below.
            if (bucket.GetIsFilled()) 
            {
                filledBucketsCount++;
            }
        }

        Debug.Log($"Progress: {filledBucketsCount} / {allBuckets.Length} buckets filled.");

        if (filledBucketsCount >= allBuckets.Length)
        {
            StartCoroutine(HandleWin());
        }
    }

    private IEnumerator HandleWin()
    {
        levelComplete = true;
        
        if (winText != null)
        {
            winText.text = "Congratulations You Win!";
        }

        yield return new WaitForSeconds(5f);

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}