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

    private float nextSpawnTime;
    private int dropsSpawned = 0;

    void Update()
    {
        // Determine if we are in unlimited mode
        bool isUnlimited = GameManager.Instance != null && GameManager.Instance.unlimitedWater;

        // Condition: Spawn if active AND (we haven't hit the limit OR we are in unlimited mode)
        if (isSpawning && (isUnlimited || dropsSpawned < maxDrops))
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnDrop();
                nextSpawnTime = Time.time + spawnRate;
            }
        }
        // Only auto-stop if NOT in unlimited mode
        else if (!isUnlimited && dropsSpawned >= maxDrops)
        {
            isSpawning = false;
        }
    }

    public void StartSpawning()
    {
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
        
        // We still count drops just in case you want to see the number in the inspector, 
        // but it won't stop the spawning in unlimited mode.
        dropsSpawned++;
    }
}