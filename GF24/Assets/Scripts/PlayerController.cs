using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        // Get the CapsuleCollider2D component on start
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the capsule collider is touching the layer tagged "Ground"
        isGrounded = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.bounds.size, capsuleCollider.direction, 0f, groundLayer);

        // Log the grounded status to the console for debugging
        Debug.Log("Grounded: " + isGrounded);

        // Move the player
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Jump logic
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Add upward force to simulate jumping
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
