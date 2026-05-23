using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private PlayerJump playerScript; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            playerScript.Jump();
        }
    }
}