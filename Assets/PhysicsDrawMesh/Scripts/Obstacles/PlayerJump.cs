using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; 
    [SerializeField] private bool alwaysJump = false; 
    
    [SerializeField] private float groundCheckDistance = 0.1f; 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (alwaysJump && IsGrounded())
        {
            Jump();
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);    
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

}