using UnityEngine;
using Water2D;

public class RockCollisionHandler : MonoBehaviour
{
    [Header("Settings")]
    public string waterTag = "BlueWater";
    public string ballTag = "Ball"; // Tag for your soccer ball
    public GameObject rockParticleEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. If WATER hits the rock, turn off the fire and stop the spawner
        if (collision.CompareTag(waterTag))
        {
            HandleImpact();
        }

        // 2. If the BALL hits the rock
        if (collision.CompareTag(ballTag))
        {
            CheckBallCollision(collision.gameObject);
        }
    }

    private void HandleImpact()
    {
        if (rockParticleEffect != null)
        {
            // Get all particle systems (including children like smoke/sparks)
            ParticleSystem[] allPS = rockParticleEffect.GetComponentsInChildren<ParticleSystem>();
            
            foreach (ParticleSystem ps in allPS)
            {
                var emission = ps.emission; 
                emission.enabled = false;  
            }
        }

        if (Water2D_Spawner.instance != null)
        {
            Water2D_Spawner.instance.StopAllCoroutines();
            Water2D_Spawner.instance.Dynamic = false;
        }
    }

    private void CheckBallCollision(GameObject ball)
    {
        if (rockParticleEffect != null)
        {
            ParticleSystem ps = rockParticleEffect.GetComponentInChildren<ParticleSystem>();
            
            // If the particle system is found AND it is still emitting...
            if (ps != null && ps.emission.enabled == true)
            {
                Debug.Log("Ball hit the fire! Destroying ball.");
                Destroy(ball);
            }
            else
            {
                Debug.Log("Ball hit the rock, but the fire is out. Ball is safe.");
            }
        }
    }
}