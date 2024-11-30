using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public float stopChaseBuffer = 1f;  // Additional distance before stopping the chase
    public float attackCooldown = 1f;
    public float attackDamage = 10f;
    public Vector2 attackBoxSize = new Vector2(2f, 1f);
    public LayerMask playerLayer;

    private Transform player;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    public Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isPlayerInChaseRange = false;
    public HealthManager eh;

    public bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Check if player is within the detection + buffer range
            isPlayerInChaseRange = distanceToPlayer <= detectionRange + stopChaseBuffer;

            if (isPlayerInChaseRange)
            {
                if (distanceToPlayer <= attackRange)
                {
                    StopChasing();
                    AttackPlayer();
                }
                else if (!isAttacking)
                {
                    animator.SetBool("isMoving", true);
                }
            }
            else if (distanceToPlayer > detectionRange + stopChaseBuffer)
            {
                StopChasing();
            }

            attackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInChaseRange && Vector2.Distance(transform.position, player.position) > attackRange && !isAttacking)
        {
            ChasePlayer();
        }
    }

    void StopChasing()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
        rb.velocity = Vector2.zero; // Ensure enemy stops completely
        isAttacking = false;
    }



    void ChasePlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            FlipTowardsPlayer();
        }
    }

   

    void AttackPlayer()
    {
        if (attackTimer <= 0f)
        {
            isAttacking = true;
            animator.SetTrigger("attack");

            attackTimer = attackCooldown;
            Collider2D playerCollider = Physics2D.OverlapBox(transform.position, attackBoxSize, 0, playerLayer);
            if (playerCollider != null)
            {
               eh.TakeDamage(5);

                Debug.Log("Player hit!");
            }
        }
    }

    void FlipTowardsPlayer()
    {
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackBoxSize);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange + stopChaseBuffer); // Visualize extended chase range
    }
}
