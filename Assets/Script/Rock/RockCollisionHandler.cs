using UnityEngine;
using Water2D;

public class RockCollisionHandler : MonoBehaviour
{
    [Header("Settings")]
    public string waterTag = "BlueWater";
    
    [Tooltip("Drag the Fire/Particle object from your Hierarchy here")]
    public ParticleSystem rockParticleEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(waterTag))
        {
            HandleImpact();
        }
    }

    private void HandleImpact()
    {
        // 1. Stop the particles using every available method
        if (rockParticleEffect != null)
        {
            // Stop emitting new particles
            rockParticleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            
            // Force the emission module off just to be safe
            var emission = rockParticleEffect.emission;
            emission.enabled = false;

            // Optional: Completely disable the object if it still won't stop
            // rockParticleEffect.gameObject.SetActive(false);
        }

        // 2. Stop the water spawner
        if (Water2D_Spawner.instance != null)
        {
            Water2D_Spawner.instance.StopAllCoroutines();
            Water2D_Spawner.instance.Dynamic = false;
            Debug.Log("Water stopped and particles should be cleared!");
        }
    }
}