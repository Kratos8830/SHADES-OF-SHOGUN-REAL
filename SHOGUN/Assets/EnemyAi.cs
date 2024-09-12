using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;        // Speed of the enemy
    public float attackRange = 1.5f;    // Range to start attacking the player
    public float detectionRange = 5f;   // Range to detect the player
    public float attackCooldown = 1f;   // Time between attacks
    public float attackDamage = 10f;    // Damage dealt to player
    public Vector2 attackBoxSize = new Vector2(2f, 1f); // Size of attack hitbox
    public LayerMask playerLayer;       // Layer to detect the player

    private Transform player;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            // Always check if the player is in range
            bool isPlayerInRange = Physics2D.OverlapBox(transform.position, new Vector2(detectionRange, detectionRange), 0, playerLayer);

            if (isPlayerInRange)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // Flip the enemy to face the player
                FlipTowardsPlayer();

                if (distanceToPlayer > attackRange)
                {
                    ChasePlayer(); // Continue chasing the player
                }
                else
                {
                     // Attack if within range
                }
            }
            else
            {
                StopChasing(); // Stop chasing when the player is out of range
            }

            // Reduce the attack cooldown timer
            attackTimer -= Time.deltaTime;
        }
    }

    void ChasePlayer()
    {
        // Reset attack state when chasing
        isAttacking = false;

        // Move towards the player if not attacking
        if (!isAttacking)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Trigger movement animation
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
            }
        }
    }

    void StopChasing()
    {
        // Stop movement animation
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        // Ensure the attack state is reset when the player leaves the detection range
        isAttacking = false;
    }

   

    //void DealDamage(Collider2D playerCollider)
    //{
    //    // Example logic for damaging the player
    //    PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
    //    if (playerHealth != null)
    //    {
    //        playerHealth.TakeDamage(attackDamage);
    //    }
    //}

    void FlipTowardsPlayer()
    {
        // Flip towards the player's position
        if (player != null)
        {
            if (player.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (player.position.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Visualize the attack hitbox and detection range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackBoxSize); // Attack hitbox
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(detectionRange, detectionRange)); // Detection range
    }
}
