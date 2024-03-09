using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int damage = 50;
    private Transform player;
    private Animator animator;
    public float detectionRange = 2f; // The range at which the enemy can detect the player
    private bool canDetect = false; // Whether the enemy can detect the player
    private bool isJumping = false; // Flag to check if the enemy is currently jumping
    private bool isGrounded = true; // Flag to check if the enemy is on the ground
    private float jumpForce = 5f; // The force applied when the enemy jumps

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                // Check if the player is within the detection range and has the "Player" tag
                if (player.CompareTag("Player"))
                {
                    canDetect = true;
                }
                else
                {
                    canDetect = false;
                }
            }
            else
            {
                canDetect = false;
            }
        }

        if (canDetect)
        {
            // If the enemy can detect the player, perform actions like moving towards the player or triggering animations
            MoveTowardsPlayer();
            animator.SetBool("canDetect", true);

            if (!isJumping && isGrounded)
            {
                // Check if the enemy is not currently jumping and is on the ground
                StartCoroutine(JumpCoroutine());
            }
        }
        else
        {
            if (!isJumping && isGrounded)
            {
                // Check if the enemy is not currently jumping and is on the ground
                StartCoroutine(JumpCoroutine());
            }
            animator.SetBool("canDetect", false);
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * 3f);
        }
    }

    IEnumerator JumpCoroutine()
    {
        // Set the jumping flag to true to avoid multiple jumps at the same time
        isJumping = true;

        // Perform actions before jumping (e.g., coming down to the ground and returning to idle animation)
        // Set the "isGrounded" flag to false before jumping
        isGrounded = false;
        animator.SetBool("isGrounded", false);

        // Wait for a short duration to simulate the time before the jump
        yield return new WaitForSeconds(1f);

        // Apply jump force
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Wait for a short duration to simulate the jump animation
        yield return new WaitForSeconds(1f);

        // Set the "isGrounded" flag to true after the jump is complete
        isGrounded = true;
        animator.SetBool("isGrounded", true);

        // Set the jumping flag to false to allow the enemy to jump again
        isJumping = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
