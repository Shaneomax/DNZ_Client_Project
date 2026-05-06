using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterParticle : MonoBehaviour
{
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // This handles the drop when it is still a Trigger (falling through air/water)
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    // This handles the drop after it has become solid (isTrigger = false)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If a solid water drop touches the ground, destroy it
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    // As requested earlier: if it stops touching something solid, it becomes a trigger again
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedWater") || collision.gameObject.CompareTag("BlueWater")) 
        {
            return;
        }

        myCollider.isTrigger = true;
    }
}