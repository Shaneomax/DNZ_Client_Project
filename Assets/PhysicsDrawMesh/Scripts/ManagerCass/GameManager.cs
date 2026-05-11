using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Water2D; // Added to match your Spawner namespace

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Dev Settings")]
    [Tooltip("If checked, water will spawn automatically.")]
    public bool unlimitedWater = false;

    [Header("Level State")]
    [SerializeField] private WaterCollector[] allBuckets;
    private bool levelComplete = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (winText != null) winText.text = "";

        if (unlimitedWater && Water2D_Spawner.instance != null)
        {
            // Using the methods available in your Water2D_Spawner script
            Water2D_Spawner.instance.Dynamic = true;
            Water2D_Spawner.instance.Spawn();
        }
    }

    // This is called by the WaterCollectors whenever they get full
    public void CheckWinCondition()
    {
        if (levelComplete) return;

        bool allFull = true;
        foreach (WaterCollector bucket in allBuckets)
        {
            if (!bucket.GetIsFilled()) 
            {
                allFull = false;
                break;
            }
        }

        if (allFull)
        {
            StartCoroutine(HandleWin());
        }
    }

    private IEnumerator HandleWin()
    {
        levelComplete = true;
        if (winText != null) winText.text = "Congratulations You Win!";
        
        // Optional: Stop spawning water when you win
        if (Water2D_Spawner.instance != null)
            Water2D_Spawner.instance.Restore();

        yield return new WaitForSeconds(3f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else 
        {
            winText.text = "All Levels Complete!";
        }
    }
}