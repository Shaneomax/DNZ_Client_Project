using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BucketWaterPreserver : MonoBehaviour
{
    [Header("Preservation Settings")]
    [Tooltip("Make sure this matches the tag on your water prefab.")]
    public string waterTag = "BlueWater";
    
    [Tooltip("The new massive lifetime to prevent the water from disappearing.")]
    public float infiniteLifeTime = 999999f;

    [Tooltip("Should the water slow down when it hits the bucket so it settles faster?")]
    public bool dampVelocity = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering is water
        if (collision.CompareTag(waterTag))
        {
            // Grab the Metaball script from the water drop
            MetaballParticleClass metaball = collision.GetComponent<MetaballParticleClass>();
            
            if (metaball != null)
            {
                // Override the lifetime so it never disappears!
                metaball.LifeTime = infiniteLifeTime;
                
                // Set the boolean from your existing script just in case you need it later
                metaball.witinTarget = true; 
            }

            // Optional: Slow down the particles so they pool nicely instead of splashing wildly
            if (dampVelocity)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = rb.linearVelocity * 0.5f; 
                }
            }
        }
    }
}