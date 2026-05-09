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
        if (collision.CompareTag("Rock"))
        {
            HandleRockCollision(collision.gameObject);
            return;
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("RedWater") || collision.CompareTag("BlueWater")) 
        {
            return; 
        }

        myCollider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

    public void HandleRockCollision(GameObject rock)
    {
        WaterSpawner spawner = GetComponentInParent<WaterSpawner>();
        if (spawner != null)
        {
            spawner.StopSpawner(rock);
        }

        Destroy(gameObject);
    }
}