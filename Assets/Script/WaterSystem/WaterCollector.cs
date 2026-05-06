using UnityEngine;
using UnityEngine.Events;

public class WaterCollector : MonoBehaviour
{
    [Header("Collection Settings")]
    [Tooltip("Tag of the water drop allowed in this bucket (e.g., 'BlueWater' or 'RedWater')")]
    public string targetWaterTag = "BlueWater";
    
    [Tooltip("How many drops are needed to fill the bucket and win")]
    public int requiredDrops = 50;

    [Header("Events")]
    [Tooltip("Fires when the bucket is successfully filled")]
    public UnityEvent OnBucketFilled;

    private int currentCollected = 0;
    private bool isFilled = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore if already filled
        if (isFilled) return;

        // Check if the correct colored water went into the bucket
        if (collision.gameObject.CompareTag(targetWaterTag))
        {
            currentCollected++;
            
            // Destroy the drop so it looks like it went "into" the liquid
            Destroy(collision.gameObject);

            if (currentCollected >= requiredDrops)
            {
                isFilled = true;
                Debug.Log($"{gameObject.name} filled with {targetWaterTag}!");
                OnBucketFilled?.Invoke();
            }
        }
        else
        {
            // Optional: Handle wrong color entering the bucket!
            // Destroy(collision.gameObject);
            // Debug.Log("Wrong water color!");
        }
    }

    // Add this inside WaterCollector.cs
    public bool GetIsFilled()
    {
        return isFilled;
    }
}