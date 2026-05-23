using UnityEngine;
using Water2D;

public class RockCollisionHandler : MonoBehaviour
{
    [Header("Settings")]
    public string waterTag = "BlueWater";
    public string ballTag = "Ball"; 
    public GameObject rockParticleEffect;

    [Header("UI Reference")]
    [Tooltip("Drag your Game Over Panel from the Canvas here")]
    public GameObject gameOverPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. If WATER hits: Turn off fire and stop spawner
        if (collision.CompareTag(waterTag))
        {
            HandleImpact();
        }

        // 2. If BALL hits: Check if fire is on, then destroy and Game Over
        if (collision.CompareTag(ballTag))
        {
            CheckBallCollision(collision.gameObject);
        }
    }

    private void HandleImpact()
    {
        if (rockParticleEffect != null)
        {
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
            
            // Condition: Ball hits while fire is still emitting
            if (ps != null && ps.emission.enabled == true)
            {
                Debug.Log("Ball hit fire! Game Over.");
                
                // Show the UI Panel
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }

                // Destroy the ball
                Destroy(ball);
                
                // Optional: Stop time so the game pauses
                // Time.timeScale = 0f; 
            }
        }
    }
}