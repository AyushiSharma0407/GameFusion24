using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce = 10f;
    public int maxHealth = 100;
    private int currentHealth;
    public LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    public HealthUI healthUI;
    public GameObject[] enemyPrefabs; // Array to hold different enemy types
    public Transform[] enemySpawnPoints;
    public float enemySpawnInterval = 5f;
    private float nextSpawnTime;

    private string currentWeapon = "Fire"; // Default weapon

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // Changed to BoxCollider2D
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        isGrounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);

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

        // Switch weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = "Fire";
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = "Water";
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeapon = "Grass";

        // Attacking enemies
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            GetComponent<Animator>().SetTrigger("isAttacking" + currentWeapon);
            Attack(currentWeapon);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs != null && enemyPrefabs.Length > 0 && enemySpawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, enemySpawnPoints.Length);
            Transform spawnPoint = enemySpawnPoints[randomIndex];
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Select random enemy prefab
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void Attack(string weapon)
    {
        // Set the correct trigger based on the weapon
        string triggerName = "isAttacking" + weapon;
        GetComponent<Animator>().SetTrigger(triggerName);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(weapon); // Pass weapon type to enemy
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthUI.UpdateHearts(currentHealth);
        if (currentHealth <= 0)
        {
            // Play death animation
            GetComponent<Animator>().SetBool("isDead", true);

            // Invoke a method to destroy the player GameObject after the animation duration
            Invoke("DestroyPlayer", 1f);
        }
    }

    void DestroyPlayer()
    {
        // Destroy the player GameObject
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
}
