using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce = 10f;
    public int maxHealth = 100;
    private int currentHealth;
    public LayerMask groundLayer;
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;
    public float enemySpawnInterval = 5f;
    private float nextSpawnTime;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.bounds.size, capsuleCollider.direction, 0f, groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (horizontalInput < 0)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0)
            spriteRenderer.flipX = false;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Spawning enemies
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + enemySpawnInterval;
        }

        // Attacking enemies
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {

            // Trigger the attack animation
            GetComponent<Animator>().SetTrigger("isAttackingFire");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(100); // Destroy enemy in one hit
                }
            }
        }
    }


    void SpawnEnemy()
    {
        if (enemyPrefab != null && enemySpawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, enemySpawnPoints.Length);
            Transform spawnPoint = enemySpawnPoints[randomIndex];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(100); // Destroy enemy in one hit
            }
        }

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            // Player dies
            Destroy(gameObject); 
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
}
