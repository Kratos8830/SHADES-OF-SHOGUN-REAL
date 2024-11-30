using UnityEngine;

public class NewEnemyA : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Detection")]
    public float detectionRange = 5f;
    public float attackRange = 1f; // Range at which the enemy will stop and attack
    public Transform player;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

    private Rigidbody2D rb;
    private Vector2 knockbackDirection;

    [Header("Attack")]
    public float attackCooldown = 1f;
    private bool isAttacking = false;

    [Header("Flip Logic")]
    public bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnockedBack || isAttacking)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StopMovement();
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            Debug.Log("Player detected! Moving toward player.");
            FollowPlayer();
        }
        else
        {
            Debug.Log("Player out of range. Stopping movement.");
            StopMovement();
        }

        FlipTowardsPlayer();
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    public void ApplyKnockback(Vector2 attackPosition)
    {
        if (isKnockedBack)
            return;

        isKnockedBack = true;
        knockbackDirection = (transform.position - (Vector3)attackPosition).normalized;
        rb.velocity = knockbackDirection * knockbackForce;

        Debug.Log("Enemy knocked back!");

        // Reset knockback after a short duration
        Invoke(nameof(ResetKnockback), knockbackDuration);
    }

    void ResetKnockback()
    {
        isKnockedBack = false;
        Debug.Log("Knockback reset.");
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Debug.Log("Enemy attacking player!");
            // Add attack logic here (e.g., reducing player health)
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        // Check the player's position relative to the enemy
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the enemy by scaling its X axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Debug.Log("Enemy flipped: " + (isFacingRight ? "Right" : "Left"));
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection and attack ranges in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
