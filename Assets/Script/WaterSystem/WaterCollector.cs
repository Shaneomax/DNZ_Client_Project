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
        if (rb == null)
        {
            Debug.LogError($"WaterCollector on {gameObject.name} is missing a Rigidbody2D component!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFilled) return;

        if (collision.gameObject.CompareTag(targetWaterTag))
        {
            // --- NEW LOGIC: Make the object static ---
            if (rb != null)
            {
                //rb.bodyType = RigidbodyType2D.Static; 
                rb.simulated = false; 
            }
            // -----------------------------------------

            currentCollected++;

            if (currentCollected >= requiredDrops)
            {
                isFilled = true;
                if (rb != null)
                {
                    //rb.bodyType = RigidbodyType2D.Static; 
                    rb.simulated = false; 
                }
                Debug.Log($"{gameObject.name} filled with {targetWaterTag}!");
                OnBucketFilled?.Invoke();
            }
        }
    }

    public bool GetIsFilled()
    {
        return isFilled;
    }
}