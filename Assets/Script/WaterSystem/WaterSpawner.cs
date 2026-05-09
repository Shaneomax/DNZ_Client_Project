using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject waterDropPrefab;
    public Transform spawnPoint;
    public float spawnRate = 0.05f;
    public int maxDrops = 100;

    [Header("State")]
    public bool isSpawning = false;
    private bool stoppedByRock = false; // New flag to prevent respawning after hitting a rock

    private float nextSpawnTime;
    private int dropsSpawned = 0;

    void Update()
    {
        // If a rock has permanently disabled this spawner, do nothing
        if (stoppedByRock) return;

        bool isUnlimited = GameManager.Instance != null && GameManager.Instance.unlimitedWater;

        if (isSpawning && (isUnlimited || dropsSpawned < maxDrops))
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnDrop();
                nextSpawnTime = Time.time + spawnRate;
            }
        }
        else if (!isUnlimited && dropsSpawned >= maxDrops)
        {
            isSpawning = false;
        }
    }

    // This detects when the Spawner (or its collider) touches a Rock
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Rock"))
        {
            StopSpawner(other.gameObject);
        }
    }

    // Use this if your project is 3D

    // Inside WaterSpawner.cs

// Change StopSpawner to public so drops can call it
    public void StopSpawner(GameObject rock)
    {
        if (stoppedByRock) return; // Prevent multiple calls

        isSpawning = false;
        stoppedByRock = true; 

        // Look for particles in the rock or its children
        ParticleSystem ps = rock.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(); 
            // If you want it to vanish immediately:
            // ps.gameObject.SetActive(false);
        }

        Debug.Log("A drop hit a rock! Stopping spawner.");
    }

    public void StartSpawning()
    {
        if (stoppedByRock) return; // Don't start if we are blocked

        isSpawning = true;
        nextSpawnTime = Time.time;
        dropsSpawned = 0; 
    }

    private void SpawnDrop()
    {
        if (waterDropPrefab == null || spawnPoint == null) return;

        GameObject drop = Instantiate(waterDropPrefab, spawnPoint.position, Quaternion.identity);
        float randomX = Random.Range(-0.1f, 0.1f);
        drop.transform.position += new Vector3(randomX, 0, 0);
        drop.transform.SetParent(this.transform);
        dropsSpawned++;
    }
}