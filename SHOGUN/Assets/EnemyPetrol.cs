using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float speed = 2f;                    // Speed of the enemy
    public Transform pointA;                    // First patrol point
    public Transform pointB;                    // Second patrol point
    public LayerMask playerLayer;               // Layer that represents the player
    public float attackRange = 2f;              // Range within which the enemy will attack the player
    public float attackCooldown = 1f;           // Time between attacks

    private Transform currentTarget;            // The current target to move towards
    private bool facingRight = true;            // Check if the enemy is facing right
    private bool isAttacking = false;           // Check if the enemy is attacking
    public Transform attackPoint;
    private float lastAttackTime = 0f;          // Track the time since the last attack

    void Start()
    {
        // Set the initial target to point A
        currentTarget = pointA;
    }

    void Update()
    {
        // Check if the player is in range using Physics2D.OverlapCircle
        if (PlayerInRange())
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private bool PlayerInRange()
    {
        // Use OverlapCircle to detect if the player is within the attack range
        Collider2D playerInRange = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        // If playerInRange is not null, the player is within range
        return playerInRange != null;
    }

    private void AttackPlayer()
    {
        // Stop moving and face the player
        GameObject player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer).gameObject;

        if (player.transform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.transform.position.x > transform.position.x && facingRight)
        {
            Flip();
        }

        // Perform an attack if cooldown is over
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;  // Reset the attack timer
            // Insert your attack logic here (e.g., deal damage, play attack animation)
            Debug.Log("Attacking the player!");
        }
    }

    private void Patrol()
    {
        // Move between the patrol points when not attacking
        if (pointA != null && pointB != null && !isAttacking)
        {
            // Move the enemy towards the current target point
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, step);

            // Check if the enemy has reached the target point
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f) // Increased tolerance
            {
                // Switch the target between point A and point B
                currentTarget = currentTarget == pointA ? pointB : pointA;
                Flip();
            }
        }
    }

    // Method to flip the enemy sprite
    private void Flip()
    {
        facingRight = !facingRight; // Toggle the direction the enemy is facing

        // Multiply the enemy's x local scale by -1 to flip the sprite
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Optional: Visualize patrol points and attack range in the editor
    private void OnDrawGizmos()
    {
        // Visualize patrol points
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }

        // Visualize attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
