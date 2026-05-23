using UnityEngine;
using UnityEngine.Events;

public class WaterCollector : MonoBehaviour
{
    [Header("Collection Settings")]
    public string targetWaterTag = "BlueWater"; 
    public int requiredDrops = 50;

    [Header("Events")]
    public UnityEvent OnBucketFilled;

    private int currentCollected = 0;
    private bool isFilled = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Optimization: If already filled, don't process more physics triggers
        if (isFilled) return;

        if (collision.gameObject.CompareTag(targetWaterTag))
        {
            currentCollected++;

            // Optional: You could 'deactivate' the water drop here so it doesn't 
            // bounce out of the bucket and stays inside.
            // collision.gameObject.GetComponent<MetaballParticleClass>().Active = false;

            if (currentCollected >= requiredDrops)
            {
                CompleteBucket();
            }
        }
    }

    private void CompleteBucket()
    {
        isFilled = true;
        
        // Freeze the bucket now that it's full
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop current movement
            rb.simulated = false; // Stop physics interactions
        }

        Debug.Log($"{gameObject.name} filled!");
        OnBucketFilled?.Invoke();

        // Tell the GameManager to check if this was the last bucket needed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckWinCondition();
        }
    }

    public bool GetIsFilled()
    {
        return isFilled;
    }
}