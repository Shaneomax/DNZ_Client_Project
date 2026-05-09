using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterParticle : MonoBehaviour
{
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- NEW: Rock Interaction ---
        if (collision.CompareTag("Rock"))
        {
            HandleRockCollision(collision.gameObject);
            return;
        }

        // 1. If it hits the ground while in trigger mode, destroy it
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // 2. Ignore other water colors so they flow through each other
        if (collision.CompareTag("RedWater") || collision.CompareTag("BlueWater")) 
        {
            return; 
        }

        // 3. If it hits anything else (like the bucket rim), make it solid
        myCollider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // --- NEW: Rock Interaction (if already solid) ---
        if (collision.gameObject.CompareTag("Rock"))
        {
            HandleRockCollision(collision.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedWater") || collision.gameObject.CompareTag("BlueWater")) 
        {
            return;
        }

        myCollider.isTrigger = true;
    }

    // Helper method to handle the logic you requested
    private void HandleRockCollision(GameObject rock)
    {
        // 1. Tell the Spawner to stop (it looks at the parent because the spawner owns the drops)
        WaterSpawner spawner = GetComponentInParent<WaterSpawner>();
        if (spawner != null)
        {
            spawner.StopSpawner(rock);
        }

        // 2. Destroy the drop so it doesn't just sit on the rock
        Destroy(gameObject);
    }
}