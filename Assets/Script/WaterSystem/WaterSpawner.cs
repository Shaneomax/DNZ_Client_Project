using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("The water droplet prefab containing a Rigidbody2D and CircleCollider2D")]
    public GameObject waterDropPrefab;
    
    [Tooltip("Where the water drops will spawn from")]
    public Transform spawnPoint;
    
    [Tooltip("Time in seconds between each spawned drop")]
    public float spawnRate = 0.05f;
    
    [Tooltip("Total amount of water to spawn before stopping")]
    public int maxDrops = 100;

    [Header("State")]
    public bool isSpawning = false;

    private float nextSpawnTime;
    private int dropsSpawned = 0;

    void Update()
    {
        if (isSpawning && dropsSpawned < maxDrops)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnDrop();
                nextSpawnTime = Time.time + spawnRate;
            }
        }
        else if (dropsSpawned >= maxDrops)
        {
            isSpawning = false; // Auto-stop when empty
        }
    }

    /// <summary>
    /// Call this via UI Button or Level Manager to open the pipe.
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
        nextSpawnTime = Time.time;
        dropsSpawned = 0; // Reset if you want to replay
    }

    private void SpawnDrop()
    {
        // Create the drop
        GameObject drop = Instantiate(waterDropPrefab, spawnPoint.position, Quaternion.identity);
        
        // Optional: Randomize the spawn position slightly so they don't form a perfect straight line
        float randomX = Random.Range(-0.1f, 0.1f);
        drop.transform.position += new Vector3(randomX, 0, 0);

        // Group them under this object to keep the hierarchy clean
        drop.transform.SetParent(this.transform);
        
        dropsSpawned++;
    }
}