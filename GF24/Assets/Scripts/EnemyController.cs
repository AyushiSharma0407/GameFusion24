using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public int damage = 50;
    private Transform player;
    private Animator animator;
    public float detectionRange = 2f; // The range at which the enemy can detect the player
    private bool canDetectFire = false; // Whether the enemy can detect the player for fire enemy
    private bool canDetectWater = false; // Whether the enemy can detect the player for water enemy
    private bool canDetectGrass = false; // Whether the enemy can detect the player for grass enemy
    private string enemyType; // Type of enemy

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (gameObject.CompareTag("FireEnemy"))
            enemyType = "Fire";
        else if (gameObject.CompareTag("WaterEnemy"))
            enemyType = "Water";
        else if (gameObject.CompareTag("GrassEnemy"))
            enemyType = "Grass";
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
                    SetCanDetectByType(true);
                }
                else
                {
                    SetCanDetectByType(false);
                }
            }
            else
            {
                SetCanDetectByType(false);
            }
        }

        // Set animation parameters based on enemy type and detection status
        switch (enemyType)
        {
            case "Fire":
                animator.SetBool("canDetectFire", true);

                break;
            case "Water":

                animator.SetBool("canDetectWater", true);

                break;
            case "Grass":

                animator.SetBool("canDetectGrass", true);
                break;
            default:
                break;
        }

        // Move towards player if grounded
        MoveTowardsPlayer();
    }

    void SetCanDetectByType(bool value)
    {
        switch (enemyType)
        {
            case "Fire":
                canDetectFire = value;
                break;
            case "Water":
                canDetectWater = value;
                break;
            case "Grass":
                canDetectGrass = value;
                break;
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

    public void TakeDamage(string weapon)
    {
        int damageAmount = CalculateDamage(weapon);
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    int CalculateDamage(string weapon)
    {
        int damageAmount = 0;

        switch (weapon)
        {
            case "Fire":
                damageAmount = CalculateDamageByType("Fire");
                break;
            case "Water":
                damageAmount = CalculateDamageByType("Water");
                break;
            case "Grass":
                damageAmount = CalculateDamageByType("Grass");
                break;
        }

        return damageAmount;
    }

    int CalculateDamageByType(string weaponType)
    {
        int damageAmount = 0;

        switch (enemyType)
        {
            case "Fire":
                damageAmount = CalculateDamageToFire(weaponType);
                break;
            case "Water":
                damageAmount = CalculateDamageToWater(weaponType);
                break;
            case "Grass":
                damageAmount = CalculateDamageToGrass(weaponType);
                break;
        }

        return damageAmount;
    }

    int CalculateDamageToFire(string weaponType)
    {
        switch (weaponType)
        {
            case "Fire":
                return 50;
            case "Water":
                return 100;
            case "Grass":
                return 25;
            default:
                return 0;
        }
    }

    int CalculateDamageToWater(string weaponType)
    {
        switch (weaponType)
        {
            case "Fire":
                return 25;
            case "Water":
                return 50;
            case "Grass":
                return 100;
            default:
                return 0;
        }
    }

    int CalculateDamageToGrass(string weaponType)
    {
        switch (weaponType)
        {
            case "Fire":
                return 100;
            case "Water":
                return 25;
            case "Grass":
                return 50;
            default:
                return 0;
        }
    }
}
