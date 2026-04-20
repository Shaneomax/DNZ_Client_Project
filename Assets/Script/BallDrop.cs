using System.Collections;
using UnityEngine;

public class BallDrop : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        // 1. Get the Rigidbody2D component attached to the ball
        rb = GetComponent<Rigidbody2D>();

        // 2. Disable physics so the ball hangs in the air
        rb.simulated = false;

        // 3. Start the 1-second timer
        StartCoroutine(DropTimer());
    }

    private IEnumerator DropTimer()
    {
        // Wait for exactly 1 second
        yield return new WaitForSeconds(7f);

        // Turn physics back on so gravity takes over and it falls
        rb.simulated = true;
    }
}