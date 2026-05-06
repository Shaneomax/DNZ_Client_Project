using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterParticle : MonoBehaviour
{
    private Collider2D myCollider;

    void Start()
    {
        // Grab the collider attached to this water drop
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("RedWater") || collision.CompareTag("BlueWater")) 
        {
            return; 
        }

        myCollider.isTrigger = false;
    }
}