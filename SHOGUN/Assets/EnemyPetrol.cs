using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float speed = 2f;                    // Speed of the enemy
                 
    public LayerMask playerLayer;               // Layer that represents the player
    public float attackRange = 2f;              // Range within which the enemy will attack the player
    public float attackCooldown = 1f;           // Time between attacks

           // The current target to move towards
    private bool facingRight = true;            // Check if the enemy is facing right
    private bool isAttacking = false;           // Check if the enemy is attacking
    public Transform attackPoint;
    private float lastAttackTime = 0f;        // Track the time since the last attack

    public Animator animator;

    void Start()
    {
        
        
    }

    void Update()
    {
        // Check if the player is in range using Physics2D.OverlapCircle
        if (PlayerInRange())
        {
            AttackPlayer();
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
        

       
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("attack");
            lastAttackTime = Time.time;  // Reset the attack timer
            // Insert your attack logic here (e.g., deal damage, play attack animation)
            Debug.Log("Attacking the player!");
        }
    }

  

   
    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
