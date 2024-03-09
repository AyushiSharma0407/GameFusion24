using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int damage = 50;
    private Transform player;
    private Animator animator;
    public float detectionRange = 10f; // The range at which the enemy can detect the player
    private bool canDetect = false; // Whether the enemy can detect the player

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
        }
        else
        {
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
