using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movement input
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Move the character
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip the sprite if moving left or right
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // face right
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // face left
        }
    }
}