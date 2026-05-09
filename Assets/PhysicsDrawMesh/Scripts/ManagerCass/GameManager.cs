using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton instance to allow the Spawner to find the GameManager easily
    public static GameManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Dev Settings")]
    [Tooltip("If checked, water will spawn automatically and infinitely.")]
    public bool unlimitedWater = false;

    [Header("Level State")]
    [SerializeField] private WaterCollector[] allBuckets;
    private int filledBucketsCount = 0;
    private bool levelComplete = false;

    void Awake()
    {
        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (winText != null) winText.text = "";

        if (allBuckets == null || allBuckets.Length == 0)
        {
            Debug.LogError("No buckets assigned to the Game Manager!");
        }

        // If dev mode is on, tell all spawners to start immediately
        if (unlimitedWater)
        {
            WaterSpawner[] spawners = FindObjectsByType<WaterSpawner>(FindObjectsSortMode.None);
            foreach (WaterSpawner spawner in spawners)
            {
                spawner.StartSpawning();
            }
        }
    }

    public void CheckWinCondition()
    {
        if (levelComplete) return;

        filledBucketsCount = 0;
        foreach (WaterCollector bucket in allBuckets)
        {
            // Assuming GetIsFilled() is a public method in your WaterCollector script
            if (bucket.GetIsFilled()) 
            {
                filledBucketsCount++;
            }
        }

        if (filledBucketsCount >= allBuckets.Length)
        {
            StartCoroutine(HandleWin());
        }
    }

    private IEnumerator HandleWin()
    {
        levelComplete = true;
        if (winText != null) winText.text = "Congratulations You Win!";
        
        yield return new WaitForSeconds(5f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}