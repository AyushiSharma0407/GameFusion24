using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        // Get the CapsuleCollider2D component on start
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the capsule collider is touching the layer tagged "Ground"
        bool isGrounded = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.bounds.size, capsuleCollider.direction, 0f, groundLayer);

        // Log the grounded status to the console for debugging
        Debug.Log("Grounded: " + isGrounded);

        // Move the player only when grounded
        if (isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}
