//using System.Collections;
//using UnityEngine;

//public class BallDrop : MonoBehaviour
//{
//    private Rigidbody2D rb;

//    void Start()
//    {
//        // 1. Get the Rigidbody2D component attached to the ball
//        rb = GetComponent<Rigidbody2D>();

//        // 2. Disable physics so the ball hangs in the air
//        rb.simulated = false;

//        // 3. Start the 1-second timer
//        StartCoroutine(DropTimer());
//    }

//    private IEnumerator DropTimer()
//    {
//        // Wait for exactly 1 second
//        yield return new WaitForSeconds(7f);

//        // Turn physics back on so gravity takes over and it falls
//        rb.simulated = true;
//    }
//}

using UnityEngine;

public class BallDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasDropped = false; 

    [Header("Drop Settings")]
    [SerializeField] private float dropImpulse = 2f; // Slight downward push

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Ensure the ball starts stationary and non-physical
        rb.simulated = false;
    }

    public void DropBall()
    {
        if (hasDropped) return;
        hasDropped = true;

        // Wake up the physics engine
        rb.simulated = true;

        // Optional: A tiny bit of force or torque makes the drop look less "robotic"
        rb.AddForce(Vector2.down * dropImpulse, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse); 
    }
}